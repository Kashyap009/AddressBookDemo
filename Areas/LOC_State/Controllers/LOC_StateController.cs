using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing.Printing;
using AddressBookDemo.Areas.CON_Contact.Models;
using AddressBookDemo.Areas.LOC_State.Models;

namespace AddressBookDemo.Areas.LOC_State.Controllers
{
    public class LOC_StateController : Controller
    {
        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll86
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("LOC_StateList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_DeleteByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
        public IActionResult Add(int StateID)
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
            #endregion

            #region Record Select by PK
            if (StateID != null)
            {
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_State_SelectByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                LOC_StateModel modelLOC_State = new LOC_StateModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_State.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_State.StateName = dr["StateName"].ToString();
                    modelLOC_State.StateCode = dr["StateCode"].ToString();
                    modelLOC_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_State.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_State.Modification = Convert.ToDateTime(dr["Modification"]);
                }
                return View("LOC_StateAddEdit", modelLOC_State);
            }
            #endregion
            return View("LOC_StateAddEdit");
        }

        #region Insert(Save)
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_State.StateID == null)
            {
                cmd.CommandText = "PR_LOC_State_Insert";
                //cmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = modelLOC_State.CreationDate;
                cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = DBNull.Value;


            }
            else
            {
                cmd.CommandText = "PR_LOC_State_UpdateByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;
            }
            cmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;
            // cmd.Parameters.Add("@Modification", SqlDbType.Date).Value = modelLOC_State.Modification;
            cmd.Parameters.Add("@Modification", SqlDbType.DateTime).Value = DBNull.Value;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.StateID == null)
                    TempData["StateInsertMsg"] = "State inserted successfully.";
                else
                    return RedirectToAction("Index");
            }

            conn.Close();
            //return View("LOC_StateAddEdit");
            return RedirectToAction("Index");

        }
        #endregion

        #region Filter Records
        public IActionResult Filter(string? StateName = null, string? StateCode = null)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_FilterByStateNameAndCode";

            if (StateName == null)
            {
                StateName = "";
            }
            if (StateCode == null)
            {
                StateCode = "";
            }
            //cmd.Parameters.AddWithValue("@CountryName", CountryName);
            //cmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            cmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = StateCode;


            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();//ExecuteReader()-Read All Data       EecuteScalar-Single Row and Single Column      ExecuteNonQuery()-Insert Update Delete
            dt.Load(sdr);
            conn.Close();
            return View("LOC_StateList", dt);
        }
        #endregion

    }
}
