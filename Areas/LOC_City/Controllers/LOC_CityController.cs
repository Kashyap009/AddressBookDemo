using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBookDemo.Areas.CON_Contact.Models;
using AddressBookDemo.Areas.LOC_City.Models;
using AddressBook.DAL;

#region All Methods
namespace AddressBookDemo.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[Controller]/[action]")]
    #region Configuration Date
    public class LOC_CityController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region SelectAll Records
        public IActionResult Index()
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_City_SelectAll(str);
            return View("LOC_CityList", dt);
        }
        #endregion

        #region Delete any Record
        public IActionResult Delete(int CityID)
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_DeleteByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Add Records
        public IActionResult Add(int? CityID)
        {

            #region DropDown Country
            string str1 = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn1 = new SqlConnection(str1);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_Country_SelectionForDropDown";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            conn1.Close();

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel cntrlst = new LOC_CountryDropDownModel();
                cntrlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                cntrlst.CountryName = dr["CountryName"].ToString();
                list.Add(cntrlst);
            }
            ViewBag.CountryList = list;
            #endregion

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list2;


            if (CityID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_SelectByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                LOC_CityModel modelLOC_City = new LOC_CityModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.CityCode = dr["CityCode"].ToString();
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_City.Modification = Convert.ToDateTime(dr["Modification"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                }
                return View("LOC_CityAddEdit", modelLOC_City);
            }
            return View("LOC_CityAddEdit");
        }
        #endregion

        #region Save region
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_City.CityID == null)
            {
                cmd.CommandText = "PR_LOC_City_Insert";
                //cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelLOC_City.CreationDate;
                cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                cmd.CommandText = "PR_LOC_City_UpdateByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLOC_City.CityID;
            }
            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = modelLOC_City.CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.NVarChar).Value = modelLOC_City.CityCode;
            cmd.Parameters.Add("@StateID", SqlDbType.NVarChar).Value = modelLOC_City.StateID;
            //cmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = modelLOC_City.ModificationDate;
            cmd.Parameters.Add("@Modification", SqlDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_City.CountryID;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityID == null)
                {
                    TempData["CityInsertMsg"] = "Record Inserted Successfully ! ";
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion


        [HttpPost]
        public IActionResult DropdownByCountry(int CountryID)
        {
            #region DropDown State
            string str2 = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn2 = new SqlConnection(str2);
            conn2.Open();
            SqlCommand cmd2 = conn2.CreateCommand();
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "PR_LOC_State_SelectionForDropDownByCountryID";
            cmd2.Parameters.AddWithValue("@CountryID", CountryID);
            DataTable dt3 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt3.Load(sdr2);
            conn2.Close();
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr3 in dt3.Rows)
            {
                LOC_StateDropDownModel sdmlst = new LOC_StateDropDownModel();
                sdmlst.StateID = Convert.ToInt32(dr3["StateID"]);
                sdmlst.StateName = dr3["StateName"].ToString();
                list2.Add(sdmlst);
            }
            var vModel = list2;
            return Json(vModel);
            #endregion
        }

        #region Filter Records
        public IActionResult Filter(string? CityName = null, string? CityCode = null)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_FilterByCityNameAndCode";

            if (CityName == null)
            {
                CityName = "";
            }
            if (CityCode == null)
            {
                CityCode = "";
            }
            //cmd.Parameters.AddWithValue("@CountryName", CountryName);
            //cmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.NVarChar).Value = CityCode;


            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();//ExecuteReader()-Read All Data       EecuteScalar-Single Row and Single Column      ExecuteNonQuery()-Insert Update Delete
            dt.Load(sdr);
            conn.Close();
            return View("LOC_CityList", dt);
        }
        #endregion

    }
}
#endregion