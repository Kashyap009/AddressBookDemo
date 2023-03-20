using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDemo.Areas.MST_ContactCategory.Models
{
    public class MST_ContactCategoryModel
    {
        public int? ContactCategoryID { get; set; }
        [Required]
        [DisplayName("Category")]
        public string? ContactCategoryName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Modification { get; set; }


    }
}
