using BenchmarkDotNet.Running;

namespace BenchmarkMaps;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<DictionaryReadBenchmarks>();
    }
}

public class Message
{
    public required string TestField1 { get; set; }
    public required string TestField2 { get; set; }
    public required List<string> TestField3 { get; set; }
}

public class TestClass
{
    public int? ProviderId { get; set; }
    public string? TaxId { get; set; }
    public short? PrimaryProcedureCategoryCode { get; set; }
}

public record class TestRecord
{
    public int? ProviderId { get; set; }
    public string? TaxId { get; set; }
    public short? PrimaryProcedureCategoryCode { get; set; }
}