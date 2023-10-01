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
    public string WhyDoItYourself()
    {
        var serialize = Lensializer.CompletionChatRequest("hi");
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouUseJil()
    {
        var serialize = JSON.Serialize(_completionChatRequest);
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouUseNewtonsoft()
    {
        var serialize = Newtonsoft.Json.JsonConvert.SerializeObject(_completionChatRequest);
        return serialize;
    }
    
    [Benchmark]
    public string WhyDidntYouUseNative()
    {
        var serialize = System.Text.Json.JsonSerializer.Serialize(_completionChatRequest);
        return serialize;
    }
}