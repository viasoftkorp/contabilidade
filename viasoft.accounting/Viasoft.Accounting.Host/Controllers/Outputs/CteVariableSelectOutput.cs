namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class CteVariableSelectOutput
    {
        public CteVariableSelectOutput(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
        public string Description { get; set; }
    }
}