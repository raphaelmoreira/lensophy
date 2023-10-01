using BenchmarkDotNet.Attributes;
using Jil;
using Lensophy.Dto.OpenAi;
using Lensophy.Util;

namespace Lensophy.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class JsonSerializerBenchmark
{
    private readonly CompletionChatRequest _completionChatRequest = new()
    {
        Messages = new List<CompletionChatMessage>(1)
        {
            new(){ Content = "hi" }
        }
    };
    
    [Benchmark(Baseline = true)]
    public string WhyDidYourself()
    {
        var serialize = Lensializer.CompletionChatRequest("hi");
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouSerializeWithJil()
    {
        var serialize = JSON.Serialize(_completionChatRequest);
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouSerializeWithNewtonsoft()
    {
        var serialize = Newtonsoft.Json.JsonConvert.SerializeObject(_completionChatRequest);
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouSerializeWithNative()
    {
        var serialize = System.Text.Json.JsonSerializer.Serialize(_completionChatRequest);
        return serialize;
    }
}