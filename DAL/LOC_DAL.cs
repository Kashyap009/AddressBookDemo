
using AddressBookDemo.Areas.LOC_City.Models;
using AddressBookDemo.Areas.LOC_Country.Models;
using AddressBookDemo.Areas.LOC_State.Models;
using AddressBookMulti.DAL;
using AddressBookMVC.DAL;
using System.Data;

namespace AddressBook.DAL
{
    public class LOC_DAL : LOC_DALBase
    {
        internal bool dbo_PR_LOC_City_Insert(string connectionstr, LOC_CityModel modelLOC_City)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_LOC_City_UpdateByPK(string connectionstr, LOC_CityModel modelLOC_City)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_LOC_Country_Insert(string connectionstr, LOC_CountryModel modelLOC_Country)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_LOC_Country_UpdateByPK(string connectionstr, LOC_CountryModel modelLOC_Country)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_LOC_State_Insert(string connectionstr, LOC_StateModel modelLOC_State)
        {
            throw new NotImplementedException();
        }

        internal bool dbo_PR_LOC_State_UpdateByPK(string connectionstr, LOC_StateModel modelLOC_State)
        {
            throw new NotImplementedException();
        }
    }
}
