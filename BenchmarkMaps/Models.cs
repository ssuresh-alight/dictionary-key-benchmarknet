namespace BenchmarkMaps;

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
    public short? ProcedureCatCode { get; set; }
}

public record class TestRecord
{
    public int? ProviderId { get; set; }
    public string? TaxId { get; set; }
    public short? ProcedureCatCode { get; set; }
}

public record struct TestRecordStruct
{
    public int? ProviderId { get; set; }
    public string? TaxId { get; set; }
    public short? ProcedureCatCode { get; set; }
}
