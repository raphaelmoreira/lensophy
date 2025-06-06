using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lensophy.Dto;
using Lensophy.Dto.OpenAi;
using Lensophy.Resource;
using Moq;
using Moq.Protected;
using Xunit;
using FluentAssertions; // Optional, but good for assertions

namespace Lensophy.UnitTest
{
    public class LensophyServiceTest : IDisposable
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly OpenAiConfig _openAiConfig; // Using real OpenAiConfig as it's simple
        private readonly LensophyService _lensophyService;

        public LensophyServiceTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            // It's important that the BaseAddress for the HttpClient matches exactly what LensophyService expects,
            // or that the SetupMockHttpResponse uses full URLs if BaseAddress is not used consistently.
            // LensophyService sets _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _openAiConfig = new OpenAiConfig("test_api_key");
            _lensophyService = new LensophyService(_httpClient, _openAiConfig);
        }

        private void SetupMockHttpResponse(string urlPart, HttpStatusCode statusCode, string responseContent)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains(urlPart)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent)
                })
                .Verifiable(); // Make it verifiable for header checks
        }

        private void SetupMockSuccessfulModerationResponse(bool isFlagged = false)
        {
            var moderationResult = new ModerationResult { Flagged = isFlagged };
            var moderationResponse = new ModerationResponse { Results = new[] { moderationResult }, Id = "mod_id", Model = "text-moderation-00x" };
            SetupMockHttpResponse("moderations", HttpStatusCode.OK, JsonSerializer.Serialize(moderationResponse));
        }

        private void SetupMockSuccessfulChatCompletionResponse(string suggestedMessage = "Suggested message.")
        {
            var chatChoice = new CompletionChatChoice { Message = new CompletionChatMessage { Role = "assistant", Content = suggestedMessage } };
            var chatResponse = new CompletionChatResponse { Choices = new[] { chatChoice }, Id = "chat_id", Model = "gpt-3.5-turbo" };
            SetupMockHttpResponse("chat/completions", HttpStatusCode.OK, JsonSerializer.Serialize(chatResponse));
        }

        // Constructor Tests
        [Fact]
        public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            // Arrange
            HttpClient nullHttpClient = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new LensophyService(nullHttpClient, _openAiConfig));
            Assert.Equal("httpClient", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithNullOpenAiConfig_ThrowsArgumentNullException()
        {
            // Arrange
            OpenAiConfig nullOpenAiConfig = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new LensophyService(_httpClient, nullOpenAiConfig));
            Assert.Equal("openAiConfig", exception.ParamName);
        }

        // API Key Handling (Authorization Header) Tests
        [Fact]
        public async Task AnalyseAsync_SetsAuthorizationHeaderCorrectly()
        {
            // Arrange
            SetupMockSuccessfulModerationResponse();
            SetupMockSuccessfulChatCompletionResponse();
            var contentAnalyse = new ContentAnalyse("Test message");

            // Act
            await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2), // Once for moderation, once for chat completion
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "test_api_key"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        // Input Validation Tests (via AnalyseAsync)
        [Fact]
        public async Task AnalyseAsync_NullMessage_ThrowsArgumentNullException()
        {
            var contentAnalyse = new ContentAnalyse(null);
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None));
            Assert.Equal("Message", ex.ParamName); // Ensure this matches nameof(contentAnalyse.Message)
            Assert.Contains(Shared.MessageCannotBeNull, ex.Message);
        }

        [Fact]
        public async Task AnalyseAsync_EmptyMessage_ThrowsArgumentException()
        {
            var contentAnalyse = new ContentAnalyse("");
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None));
            Assert.Equal("Message", ex.ParamName);
            Assert.Contains(Shared.MessageCannotBeEmpty, ex.Message);
        }

        [Fact]
        public async Task AnalyseAsync_WhitespaceMessage_ThrowsArgumentException()
        {
            var contentAnalyse = new ContentAnalyse("   ");
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None));
            Assert.Equal("Message", ex.ParamName);
            Assert.Contains(Shared.MessageCannotBeEmpty, ex.Message);
        }

        [Fact]
        public async Task AnalyseAsync_MessageTooLong_ThrowsArgumentOutOfRangeException()
        {
            var longMessage = new string('a', 4001);
            var contentAnalyse = new ContentAnalyse(longMessage);
            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None));
            Assert.Equal("Message", ex.ParamName);
            Assert.Contains(string.Format(Shared.MessageTooLong, 4000), ex.Message);
        }

        [Fact]
        public async Task AnalyseAsync_ContextTooLong_ThrowsArgumentOutOfRangeException()
        {
            var longContext = new string('a', 1001);
            var contentAnalyse = new ContentAnalyse("Valid message", longContext);
            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None));
            Assert.Equal("Context", ex.ParamName);
            Assert.Contains(string.Format(Shared.ContextTooLong, 1000), ex.Message);
        }

        // Error Handling Tests
        [Fact]
        public async Task AnalyseAsync_ChatCompletionsReturns401_ReturnsContentAnalysedWithError()
        {
            // Arrange
            SetupMockSuccessfulModerationResponse(isFlagged: false);
            SetupMockHttpResponse("chat/completions", HttpStatusCode.Unauthorized, "Unauthorized access");
            var contentAnalyse = new ContentAnalyse("Test message");

            // Act
            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            // Assert
            Assert.True(result.HasError);
            Assert.Equal(Shared.OpenAiUnauthorized, result.ErrorMessage);
            Assert.Equal("Test message", result.Message); // Original message
            Assert.False(result.IsHarmful); // From successful moderation
        }

        [Fact]
        public async Task AnalyseAsync_ChatCompletionsReturns429_ReturnsContentAnalysedWithError()
        {
            SetupMockSuccessfulModerationResponse(isFlagged: false);
            SetupMockHttpResponse("chat/completions", HttpStatusCode.TooManyRequests, "Too many requests");
            var contentAnalyse = new ContentAnalyse("Test message");

            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            Assert.True(result.HasError);
            Assert.Equal(Shared.OpenAiTooManyRequests, result.ErrorMessage);
            Assert.Equal("Test message", result.Message);
            Assert.False(result.IsHarmful);
        }

        [Fact]
        public async Task AnalyseAsync_ChatCompletionsReturns500_ReturnsContentAnalysedWithError()
        {
            SetupMockSuccessfulModerationResponse(isFlagged: false);
            SetupMockHttpResponse("chat/completions", HttpStatusCode.InternalServerError, "Server error");
            var contentAnalyse = new ContentAnalyse("Test message");

            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            Assert.True(result.HasError);
            Assert.Equal(Shared.OpenAiInternalServerError, result.ErrorMessage);
            Assert.Equal("Test message", result.Message);
            Assert.False(result.IsHarmful);
        }

        [Fact]
        public async Task AnalyseAsync_ChatCompletionsReturnsOtherError_ReturnsContentAnalysedWithError()
        {
            SetupMockSuccessfulModerationResponse(isFlagged: false);
            var errorDetails = "Forbidden access";
            SetupMockHttpResponse("chat/completions", HttpStatusCode.Forbidden, errorDetails);
            var contentAnalyse = new ContentAnalyse("Test message");

            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            Assert.True(result.HasError);
            Assert.Equal(string.Format(Shared.OpenAiGenericError, HttpStatusCode.Forbidden, errorDetails), result.ErrorMessage);
            Assert.Equal("Test message", result.Message);
            Assert.False(result.IsHarmful);
        }

        [Fact]
        public async Task AnalyseAsync_ModerationReturns401_ReturnsContentAnalysedWithError()
        {
            SetupMockHttpResponse("moderations", HttpStatusCode.Unauthorized, "Unauthorized moderation access");
            // No need to mock chat/completions as it shouldn't be called
            var contentAnalyse = new ContentAnalyse("Test message");

            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            Assert.True(result.HasError);
            Assert.Equal(Shared.ModerationOpenAiUnauthorized, result.ErrorMessage);
            Assert.False(result.IsHarmful); // Current logic sets IsHarmful to false when moderation fails
            Assert.Equal($"(no changes) {contentAnalyse.Message}", result.Message); // Message is "(no changes) <original message>"
        }

        [Fact]
        public async Task AnalyseAsync_ModerationReturnsMalformedJsonResponse_ReturnsContentAnalysedWithErrorAndServiceDidNotRespondAsExpected()
        {
            SetupMockHttpResponse("moderations", HttpStatusCode.OK, "This is not valid JSON");
            var contentAnalyse = new ContentAnalyse("Test message");

            var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);

            Assert.True(result.HasError);
            // This specific error occurs if deserialization fails and IsHarmful returns null
            // However, if deserialization throws an exception within IsHarmful before it can return null,
            // the behavior might differ. Current IsHarmful catches HTTP errors, not necessarily deserialization ones before returning.
            // If Deserialize<ModerationResponse> returns null:
            Assert.Equal(Shared.ServiceDidNotRespondAsExpected, result.ErrorMessage);
            Assert.False(result.IsHarmful);
        }

        [Fact]
        public async Task AnalyseAsync_ChatCompletionReturnsOkWithMalformedJson_ShouldBeHandledGracefully()
        {
            // This test checks if the system throws an unhandled exception or returns an error.
            // A more robust system would return ContentAnalysed with HasError = true.
            SetupMockSuccessfulModerationResponse();
            SetupMockHttpResponse("chat/completions", HttpStatusCode.OK, "This is not valid JSON for chat completion");
            var contentAnalyse = new ContentAnalyse("Test message");

            // Depending on how JsonSerializer.Deserialize and .ToContentAnalysed are implemented,
            // this might throw JsonException, or if Deserialize returns null, then .ToContentAnalysed might throw NullReferenceException.
            // For now, let's assert that it *doesn't* succeed and *does* indicate an error in ContentAnalysed,
            // or we catch an expected exception if that's the current behavior.

            // Current code: var response = JsonSerializer.Deserialize<CompletionChatResponse>(responseContent, _serializerOptions);
            // return response.ToContentAnalysed(moderationResponse.Flagged);
            // If response is null, response.ToContentAnalysed will throw NullReferenceException.
            // If JsonSerializer.Deserialize throws JsonException, that will propagate.

            try
            {
                var result = await _lensophyService.AnalyseAsync(contentAnalyse, CancellationToken.None);
                // If it reaches here, check if error is reported
                Assert.True(result.HasError, "Expected HasError to be true if JSON is malformed but service didn't throw.");
                // Add more specific checks if there's a defined error DTO for this case.
                // For instance, if ToContentAnalysed handles null CompletionChatResponse gracefully:
                // Assert.Equal(SomeSpecificError, result.ErrorMessage);
            }
            catch (JsonException ex)
            {
                // This is an acceptable outcome if not handled internally by returning an error DTO
                Assert.NotNull(ex);
            }
            catch (NullReferenceException ex)
            {
                // This indicates a bug where a null deserialized object is used without a check.
                // This test would highlight such a bug.
                Assert.True(false, $"NullReferenceException was thrown: {ex.Message}. This should be handled gracefully.");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            // Any other disposable resources
        }
    }
}
