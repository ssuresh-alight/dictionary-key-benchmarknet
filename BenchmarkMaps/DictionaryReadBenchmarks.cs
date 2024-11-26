using AutoFixture;
using BenchmarkDotNet.Attributes;
using NamedTupleKey = (int? providerId, string? taxid, short? procedureCategoryCode);

namespace BenchmarkMaps;

public class DictionaryReadBenchmarks
{
    private readonly Fixture _fixture = new();

    private Dictionary<string, Message> _stringMap;
    private Dictionary<TestRecord, Message> _recordMap;
    private Dictionary<NamedTupleKey, Message> _tupleMap;
    private TestRecord _existingKeyData;

    public DictionaryReadBenchmarks()
    {
        var recordKeys = _fixture.CreateMany<TestRecord>(100).ToArray();

        _existingKeyData = _fixture.Create<TestRecord>();

        _stringMap = [];
        _recordMap = [];
        _tupleMap = [];

        Array.ForEach(
            recordKeys,
            r =>
            {
                var message = _fixture.Create<Message>();
                _stringMap[$"f{r.ProviderId}_t{r.TaxId}_p{r.PrimaryProcedureCategoryCode}"] =
                    message;
                _recordMap[r] = message;
                _tupleMap[(
                    providerId: r.ProviderId,
                    taxid: r.TaxId,
                    procedureCategoryCode: r.PrimaryProcedureCategoryCode
                )] = message;
            }
        );

        var existingMessage = _fixture.Create<Message>();
        _stringMap[
                $"f{_existingKeyData.ProviderId}_t{_existingKeyData.TaxId}_p{_existingKeyData.PrimaryProcedureCategoryCode}"] =
            existingMessage;
        _recordMap[_existingKeyData] = existingMessage;
        _tupleMap[(
            providerId: _existingKeyData.ProviderId,
            taxid: _existingKeyData.TaxId,
            procedureCategoryCode: _existingKeyData.PrimaryProcedureCategoryCode
        )] = existingMessage;
    }

    [Benchmark(Baseline = true)]
    public Message? Retrieving_Using_String_Key()
    {
        var key =
            $"f{_existingKeyData.ProviderId}_t{_existingKeyData.TaxId}_p{_existingKeyData.PrimaryProcedureCategoryCode}";
        if (_stringMap.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }

    [Benchmark]
    public Message? Retrieving_Using_RecordKey()
    {
        var key = new TestRecord()
        {
            ProviderId = _existingKeyData.ProviderId,
            TaxId = _existingKeyData.TaxId,
            PrimaryProcedureCategoryCode = _existingKeyData.PrimaryProcedureCategoryCode,
        };
        if (_recordMap.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }

    [Benchmark]
    public Message? Retrieving_Using_TupleKey()
    {
        var key = (
            providerId: _existingKeyData.ProviderId,
            taxid: _existingKeyData.TaxId,
            procedureCategoryCode: _existingKeyData.PrimaryProcedureCategoryCode
        );
        if (_tupleMap.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }
}