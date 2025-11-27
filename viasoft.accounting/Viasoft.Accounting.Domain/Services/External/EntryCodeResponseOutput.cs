namespace Viasoft.Accounting.Domain.Services.External;

public class EntryCodeResponseOutput {
    public Error Error { get; set; }
    public bool Success { get; set; }
    public int Value { get; set; }


}
public class Error {
    public string Code { get; set; }
    public string Message { get; set; }
}