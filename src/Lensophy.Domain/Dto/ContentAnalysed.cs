namespace Lensophy.Domain.Dto;

/// <summary>
/// Structure of the content analyzed.
/// </summary>
/// <param name="SuggestedMessage">The reformulated <see cref="ContentAnalyse.Message"/>, whether based on
/// <see cref="ContentAnalyse.Context"/> or not. It can be null or empty if there is an <see cref="ErrorMessage"/>.</param>
/// <param name="IsHarmful">Indicates as <c>true</c> if the message was considered harmful; otherwise, <c>false</c>.</param>
/// <param name="ErrorMessage">Displays an error message if the external Api request fails. Please note that the
/// displayed message comes exclusively from the external Api and not from Lensophy. Whenever there is an error,
/// the <see cref="SuggestedMessage"/> will be null or empty.</param>
public readonly record struct ContentAnalysed(string? SuggestedMessage, bool IsHarmful, string ErrorMessage)
{
    /// <summary>
    /// Indicates as <c>true</c> if there was an error in the external API request; otherwise, <c>false</c>.
    /// </summary>
    /// <remarks>If it is <c>true</c>, <see cref="SuggestedMessage"/> will be null or empty, and the message related to
    /// the issue can be obtained from <see cref="ErrorMessage"/>. Otherwise, it will be <c>false</c>, and
    /// <see cref="SuggestedMessage"/> will contain the expected result.</remarks>
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
}