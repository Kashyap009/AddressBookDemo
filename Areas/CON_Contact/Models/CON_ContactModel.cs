using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDemo.Areas.CON_Contact.Models
{
    public class CON_ContactModel
    {
        public int? ContactID { get; set; }
        [Required]
        [DisplayName("Contact Name")]
        public string? ContactName { get; set; }
        [Required]
        [DisplayName("Contact Mobile")]
        public string? ContactMobile { get; set; }
        [Required]
        [DisplayName("Contact Address")]
        public string? ContactAddress { get; set; }
        [Required]
        [DisplayName("Email")]
        public string? ContactEmail { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int CountryID { get; set; }
        [Required]
        [DisplayName("State Name")]
        public int StateID { get; set; }
        public DateTime ContactCreationDate { get; set; }
        public DateTime ContactModification { get; set; }
        [Required]
        [DisplayName("City Name")]
        public int CityID { get; set; }
        [Required]
        [DisplayName("Contact Category")]
        public int ContactCategoryID { get; set; }
        public IFormFile? File { get; set; }
        public string? PhotoPath { get; set; }

        public string? PinCode { get; set; }
        public string? AlternetContact { get; set; }
        public string? BirthDate { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public string? Insta { get; set; }
        public string? Gender { get; set; }






    }
    public class LOC_StateDropDownModel
    {
        public int? StateID { get; set; }
        public string? StateName { get; set; }
    }
    public class LOC_CityDropDownModel
    {
        public int? CityID { get; set; }
        public string? CityName { get; set; }
    }
    public class LOC_CountryDropDownModel
    {
        public int? CountryID { get; set; }
        public string? CountryName { get; set; }
    }
    public class MST_ContactCategoryDropDownModel
    {
        public int? ContactCategoryID { get; set; }
        public string? ContactCategoryName { get; set; }
    }
}
