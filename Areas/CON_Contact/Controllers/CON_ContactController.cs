using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing.Printing;
using AddressBookDemo.Areas.CON_Contact.Models;
using AddressBook.DAL;

namespace AddressBookDemo.Areas.CON_Contact.Controllers
{
    public class CON_ContactController : Controller
    {
        private IConfiguration Configuration;
        public CON_ContactController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {

            string str = Configuration.GetConnectionString("myConnectionString");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_CON_Contact_SelectAll(str);
            return View("CON_ContactList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactID)
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_Contact_DeleteByPK";
            cmd.Parameters.AddWithValue("@ContactID", ContactID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
        public IActionResult Add(int ContactID)
        {
            if (ContactID != null)
            {
                #region DropDown
                string str = Configuration.GetConnectionString("myConnectionString");
                DataTable dt1 = new DataTable();
                SqlConnection conn1 = new SqlConnection(str);
                conn1.Open();
                SqlCommand cmd1 = conn1.CreateCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "PR_LOC_Country_SelectionForDropDown";
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                dt1.Load(sdr1);
                conn1.Close();

                List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
                foreach (DataRow dr in dt1.Rows)
                {
                    LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                    vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                    vlst.CountryName = dr["CountryName"].ToString();
                    list.Add(vlst);
                }
                ViewBag.CountryList = list;

                // STATE DROPDOWN

                DataTable dt2 = new DataTable();
                SqlConnection conn2 = new SqlConnection(str);
                conn2.Open();
                SqlCommand cmd2 = conn2.CreateCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "PR_LOC_State_SelectionForDropDown";
                SqlDataReader sdr2 = cmd2.ExecuteReader();
                dt2.Load(sdr2);
                conn2.Close();

                List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
                foreach (DataRow dr2 in dt2.Rows)
                {
                    LOC_StateDropDownModel vlst3 = new LOC_StateDropDownModel();
                    vlst3.StateID = Convert.ToInt32(dr2["StateID"]);
                    vlst3.StateName = dr2["StateName"].ToString();
                    list2.Add(vlst3);
                }
                ViewBag.StateList = list2;

                // CITY DROPDOWN
                DataTable dt3 = new DataTable();
                SqlConnection conn3 = new SqlConnection(str);
                conn3.Open();
                SqlCommand cmd3 = conn3.CreateCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "PR_LOC_City_SelectionForDropDown";
                SqlDataReader sdr3 = cmd3.ExecuteReader();
                dt3.Load(sdr3);
                conn3.Close();

                List<LOC_CityDropDownModel> list3 = new List<LOC_CityDropDownModel>();
                foreach (DataRow dr3 in dt3.Rows)
                {
                    LOC_CityDropDownModel vlst3 = new LOC_CityDropDownModel();
                    vlst3.CityID = Convert.ToInt32(dr3["CityID"]);
                    vlst3.CityName = dr3["CityName"].ToString();
                    list3.Add(vlst3);
                }
                ViewBag.CityList = list3;

                // CATEGORY DROPDOWN
                DataTable dt4 = new DataTable();
                SqlConnection conn4 = new SqlConnection(str);
                conn4.Open();
                SqlCommand cmd4 = conn4.CreateCommand();
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.CommandText = "PR_MST_ContactCategory_SelectionForDropDown";
                SqlDataReader sdr4 = cmd4.ExecuteReader();
                dt4.Load(sdr4);
                conn4.Close();

                List<MST_ContactCategoryDropDownModel> list4 = new List<MST_ContactCategoryDropDownModel>();
                foreach (DataRow dr4 in dt4.Rows)
                {
                    MST_ContactCategoryDropDownModel vlst4 = new MST_ContactCategoryDropDownModel();
                    vlst4.ContactCategoryID = Convert.ToInt32(dr4["ContactCategoryID"]);
                    vlst4.ContactCategoryName = dr4["ContactCategoryName"].ToString();
                    list4.Add(vlst4);
                }
                ViewBag.ContactCategoryList = list4;
                #endregion
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CON_Contact_SelectByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                CON_ContactModel modelCON_Contact = new CON_ContactModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelCON_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCON_Contact.ContactName = dr["ContactName"].ToString();
                    modelCON_Contact.ContactMobile = dr["ContactMobile"].ToString();
                    modelCON_Contact.ContactAddress = dr["ContactAddress"].ToString();
                    modelCON_Contact.ContactEmail = dr["ContactEmail"].ToString();
                    modelCON_Contact.PhotoPath = dr["PhotoPath"].ToString();
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.StateID = Convert.ToInt32(dr["StateID"]);
                    modelCON_Contact.ContactCreationDate = Convert.ToDateTime(dr["ContactCreationDate"]);
                    modelCON_Contact.ContactModification = Convert.ToDateTime(dr["ContactModification"]);
                    modelCON_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);

                }
                return View("CON_ContactAddEdit", modelCON_Contact);
            }

            return View("CON_ContactAddEdit");
        }

        #region Insert(Save)
        [HttpPost]
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            //Upload starts here
            if (modelCON_Contact.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, modelCON_Contact.File.FileName);
                modelCON_Contact.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + modelCON_Contact.File.FileName;
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    modelCON_Contact.File.CopyTo(stream);
                }
            }

            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelCON_Contact.ContactID == null)
            {
                cmd.CommandText = "PR_CON_Contact_Insert";
                //cmd.Parameters.Add("@ContactCreationDate", SqlDbType.Date).Value = modelCON_Contact.ContactCreationDate;
                cmd.Parameters.Add("@ContactCreationDate", SqlDbType.DateTime).Value = DBNull.Value;


            }
            else
            {
                cmd.CommandText = "PR_CON_Contact_UpdateByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = modelCON_Contact.ContactID;
            }



            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = modelCON_Contact.ContactName;
            cmd.Parameters.Add("@ContactMobile", SqlDbType.NVarChar).Value = modelCON_Contact.ContactMobile;
            cmd.Parameters.Add("@ContactAddress", SqlDbType.NVarChar).Value = modelCON_Contact.ContactAddress;
            cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar).Value = modelCON_Contact.ContactEmail;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCON_Contact.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCON_Contact.StateID;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelCON_Contact.CityID;
            cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelCON_Contact.ContactCategoryID;
            // cmd.Parameters.Add("@ContactModification", SqlDbType.Date).Value = modelCON_Contact.ContactModification;
            cmd.Parameters.Add("@ContactModification", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PhotoPath", SqlDbType.NVarChar).Value = modelCON_Contact.PhotoPath;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelCON_Contact.ContactID == null)
                    TempData["ContactInsertMsg"] = "Contact inserted successfully.";
                else
                    return RedirectToAction("Index");
            }

            conn.Close();



            return RedirectToAction("Index");

        }
        #endregion

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
        public IActionResult DropdownByState(int StateID)
        {
            string str3 = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn3 = new SqlConnection(str3);
            conn3.Open();
            SqlCommand cmd3 = conn3.CreateCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "PR_LOC_City_SelectionForDropDownByStateID";
            cmd3.Parameters.AddWithValue("StateID", StateID);
            DataTable dt4 = new DataTable();
            SqlDataReader sdr3 = cmd3.ExecuteReader();
            dt4.Load(sdr3);
            conn3.Close();

            List<LOC_CityDropDownModel> list4 = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr4 in dt4.Rows)
            {
                LOC_CityDropDownModel ctylst = new LOC_CityDropDownModel();
                ctylst.CityID = Convert.ToInt32(dr4["CityID"]);
                ctylst.CityName = dr4["CityName"].ToString();
                list4.Add(ctylst);
            }
            var vmodel = list4;
            return Json(vmodel);

        }

        #region Filter Records
        public IActionResult Filter(string? ContactName = null, string? ContactMobile = null)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_Contact_FilterByContactNameAndMobile";

            if (ContactName == null)
            {
                ContactName = "";
            }
            if (ContactMobile == null)
            {
                ContactMobile = "";
            }
            //cmd.Parameters.AddWithValue("@CountryName", CountryName);
            //cmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
            cmd.Parameters.Add("@ContactMobile", SqlDbType.NVarChar).Value = ContactMobile;


            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();//ExecuteReader()-Read All Data       EecuteScalar-Single Row and Single Column      ExecuteNonQuery()-Insert Update Delete
            dt.Load(sdr);
            conn.Close();
            return View("CON_ContactList", dt);
        }
        #endregion

    }
}
