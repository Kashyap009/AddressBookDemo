using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDemo.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }
        [Required]
        [DisplayName("City Name")]
        public string? CityName { get; set; }
        [Required]
        [DisplayName("City Code")]
        public string? CityCode { get; set; }
        [Required]
        [DisplayName("State Name")]
        public int StateID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Modification { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int? CountryID { get; set; }
    }

}
