using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using AddressBookDemo.Areas.LOC_Country.Models;
using System.Reflection;
using System.Drawing.Printing;
using AddressBook.DAL;

namespace AddressBookDemo.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/[Controller]/[action]")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            /*DataTable dt = new DataTable();*/
            string str = Configuration.GetConnectionString("myConnectionString");
          

            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_CON_Contact_SelectAll(str);
            return View("LOC_CountryList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_DeleteByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
        public IActionResult Add(int CountryID)
        {
            if (CountryID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_Country_SelectByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                    modelLOC_Country.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_Country.Modification = Convert.ToDateTime(dr["Modification"]);
                }
                return View("LOC_CountryAddEdit", modelLOC_Country);
            }

            return View("LOC_CountryAddEdit");
        }

        #region Insert(Save)
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_Country.CountryID == null)
            {
                cmd.CommandText = "PR_LOC_Country_Insert";
                //cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelLOC_Country.CreationDate;
                cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = DBNull.Value;


            }
            else
            {
                cmd.CommandText = "PR_LOC_Country_Update";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_Country.CountryID;
            }



            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = modelLOC_Country.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = modelLOC_Country.CountryCode;
            //cmd.Parameters.Add("@Modification", SqlDbType.Date).Value = modelLOC_Country.Modification;
            cmd.Parameters.Add("@Modification", SqlDbType.DateTime).Value = DBNull.Value;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryID == null)
                    TempData["CountryInsertMsg"] = "Country inserted successfully.";
                else
                    return RedirectToAction("Index");
            }

            conn.Close();
            return RedirectToAction("Index");

        }
        #endregion

        #region Filter Records
        public IActionResult Filter(string? CountryName = null, string? CountryCode = null)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_FilterByCountryNameAndCode";

            if (CountryName == null)
            {
                CountryName = "";
            }
            if (CountryCode == null)
            {
                CountryCode = "";
            }
            //cmd.Parameters.AddWithValue("@CountryName", CountryName);
            //cmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = CountryCode;


            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();//ExecuteReader()-Read All Data       EecuteScalar-Single Row and Single Column      ExecuteNonQuery()-Insert Update Delete
            dt.Load(sdr);
            conn.Close();
            return View("LOC_CountryList", dt);
        }
        #endregion

    }
}
