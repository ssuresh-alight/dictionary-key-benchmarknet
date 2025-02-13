﻿using AutoFixture;
using BenchmarkDotNet.Attributes;
using NamedTupleKey = (int? providerId, string? taxId, short? procedureCatCode);

namespace BenchmarkMaps;

[MemoryDiagnoser]
public class DictionaryWriteBenchmarks
{
    private readonly Fixture _fixture = new();
    private Dictionary<string, Message> _stringMap;
    private Dictionary<TestRecord, Message> _recordMap;
    private Dictionary<NamedTupleKey, Message> _tupleMap;
    private Dictionary<TestRecordStruct, Message> _recordStructMap;
    private TestRecord _newKeyData;
    private Message _newMessage;

    public DictionaryWriteBenchmarks()
    {
        var recordKeys = _fixture.CreateMany<TestRecord>(100).ToArray();

        _newKeyData = _fixture.Create<TestRecord>();
        _newMessage = _fixture.Create<Message>();

        _stringMap = [];
        _recordMap = [];
        _recordStructMap = [];
        _tupleMap = [];

        Array.ForEach(
            recordKeys,
            r =>
            {
                var message = _fixture.Create<Message>();
                _stringMap[$"f{r.ProviderId}_t{r.TaxId}_p{r.ProcedureCatCode}"] =
                    message;
                _recordMap[r] = message;
                _tupleMap[(
                    providerId: r.ProviderId,
                    taxId: r.TaxId,
                    procedureCatCode: r.ProcedureCatCode
                )] = message;
                _recordStructMap[new()
                    {
                        ProviderId = r.ProviderId,
                        TaxId = r.TaxId,
                        ProcedureCatCode = r.ProcedureCatCode
                    }
                ] = message;
            }
        );
    }

    [Benchmark(Baseline = true)]
    public Dictionary<string, Message> Writing_Using_String_Key()
    {
        var key =
            $"f{_newKeyData.ProviderId}_t{_newKeyData.TaxId}_p{_newKeyData.ProcedureCatCode}";
        _stringMap[key] = _newMessage;
        return _stringMap;
    }

    [Benchmark]
    public Dictionary<TestRecord, Message> Writing_Using_RecordKey()
    {
        var key = new TestRecord()
        {
            ProviderId = _newKeyData.ProviderId,
            TaxId = _newKeyData.TaxId,
            ProcedureCatCode = _newKeyData.ProcedureCatCode,
        };
        _recordMap[key] = _newMessage;
        return _recordMap;
    }

    [Benchmark]
    public Dictionary<TestRecordStruct, Message> Writing_Using_RecordStructKey()
    {
        TestRecordStruct key = new()
        {
            ProviderId = _newKeyData.ProviderId,
            TaxId = _newKeyData.TaxId,
            ProcedureCatCode = _newKeyData.ProcedureCatCode
        };
        _recordStructMap[key] = _newMessage;
        return _recordStructMap;
    }

    [Benchmark]
    public Dictionary<NamedTupleKey, Message> Writing_Using_TupleKey()
    {
        var key = (
            providerId: _newKeyData.ProviderId,
            taxid: _newKeyData.TaxId,
            procedureCategoryCode: _newKeyData.ProcedureCatCode
        );
        _tupleMap[key] = _newMessage;
        return _tupleMap;
    }
}