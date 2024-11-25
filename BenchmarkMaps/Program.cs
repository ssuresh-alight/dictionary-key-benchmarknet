using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchmarkMaps;

using NamedTupleKey = (int? providerId, string? taxid, short? procedureCategoryCode);

public class DictionaryBenchmarks
{
    // TODO: Compare performance difference between construction and usage as dictionary key using benchmark tools:
    // 1. string interpolation
    // 2. tuple of string/numbers
    // 3. new record class
    // 4. new record struct

    private Fixture _fixture;

    public Dictionary<string, Message> EmptyStringKeyMap;
    public Dictionary<string, Message> FilledStringKeyMap;
    public string[] StringKeys;

    public Dictionary<TestRecord, Message> EmptyRecordKeyMap;
    public Dictionary<TestRecord, Message> FilledRecordKeyMap;
    private IEnumerable<object> RecordKeys;

    public Dictionary<NamedTupleKey, Message> EmptyTupleKeyMap;
    public Dictionary<NamedTupleKey, Message> FilledTupleMap;
    public NamedTupleKey[] TupleKeys;

    public IEnumerable<object> RecordKeysArgSource => this.RecordKeys;

    // public Dictionary<TestClass, Message> EmptyClassKeyMap;
    // public Dictionary<TestClass, Message> FilledClassKeyMap;
    // public TestClass[] ClassKeys;

    [GlobalSetup]
    public void Setup()
    {
        _fixture = new Fixture();

        EmptyRecordKeyMap = [];
        RecordKeys = _fixture.CreateMany<TestRecord>(1000).ToArray();
        FilledRecordKeyMap = [];
        foreach (var r in (RecordKeys as TestRecord[])!)
        {
            FilledRecordKeyMap[r] = _fixture.Create<Message>(); 
        }

        EmptyStringKeyMap = [];
        StringKeys = (RecordKeys as TestRecord[])!
            .Select(r => $"f{r.ProviderId}_t{r.TaxId}_p{r.PrimaryProcedureCategoryCode}")
            .ToArray();
        FilledStringKeyMap = [];
        Array.ForEach(StringKeys, s => { FilledStringKeyMap[s] = _fixture.Create<Message>(); });

        EmptyTupleKeyMap = [];
        TupleKeys = (RecordKeys as TestRecord[])!
            .Select(r => (r.ProviderId, r.TaxId, r.PrimaryProcedureCategoryCode))
            .ToArray();
        FilledTupleMap = [];
        Array.ForEach(TupleKeys, t => { FilledTupleMap[t] = _fixture.Create<Message>(); });
    }

    // [Benchmark(Baseline = true)]
    [Benchmark]
    [ArgumentsSource(nameof(RecordKeysArgSource))]
    public Message Constructing_And_Retrieving_Using_StringKeys(TestRecord lookupKeys)
    {
        var key = $"f{lookupKeys.ProviderId}_t{lookupKeys.TaxId}_p{lookupKeys.PrimaryProcedureCategoryCode}";
        return FilledStringKeyMap[key];
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
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