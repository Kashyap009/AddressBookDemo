using AddressBookDemo.Models;
//using AddressBookDemo.DAL;
using AddressBookDemo.Areas.CON_Contact.Models;
using AddressBookDemo.Areas.MST_ContactCategory.Models;

namespace AddressBookMVC.DAL
{
    public class CON_DAL : CON_DALBase
    {
        internal bool dbo_PR_CON_Contact_Insert(string connectionstr, CON_ContactModel modelCON_Contact)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_CON_Contact_UpdateByPK(string connectionstr, CON_ContactModel modelCON_Contact)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_MST_ContactCategory_Insert(string connectionstr,  MST_ContactCategoryModel modelMST_ContactCategory)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_MST_ContactCategory_UpdateByPK(string connectionstr, MST_ContactCategoryModel modelMST_ContactCategory)
        {
            throw new NotImplementedException();
        }
    }
}
