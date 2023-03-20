using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDemo.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        [Required]
        [DisplayName("State Name")]
        public string? StateName { get; set; }
        [Required]
        [DisplayName("State Code")]
        public string? StateCode { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int CountryID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Modification { get; set; }


    }
}
