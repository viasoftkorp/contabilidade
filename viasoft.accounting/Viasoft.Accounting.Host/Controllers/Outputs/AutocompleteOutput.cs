using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Viasoft.Accounting.Host.Controllers.Outputs {
    public class AutocompleteOutputValue {
        [Required] public string Key {get;set;}
        [Required] public string Value {get;set;}
    }
    public class AutocompleteOutputItems {
        [Required] public AutocompleteOutputValue Option {get;set;}

    }
    public class AutocompleteOutput {
        [Required] public int TotalCount {get; set;}
        [Required] public List<AutocompleteOutputItems> Items {get;set;}
    }
}