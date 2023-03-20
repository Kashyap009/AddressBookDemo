using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using AddressBookDemo.Areas.MST_ContactCategory.Models;
using System.Reflection;
using System.Drawing.Printing;

namespace AddressBookDemo.Areas.MST_ContactCategory.Controllers
{
    public class MST_ContactCategoryController : Controller
    {
        private IConfiguration Configuration;
        public MST_ContactCategoryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_ContactCategory_SelectAll";
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("MST_ContactCategoryList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactCategoryID)
        {
            DataTable dt = new DataTable();
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_ContactCategory_DeleteByPK";
            cmd.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion
        public IActionResult Add(int ContactCategoryID)
        {
            if (ContactCategoryID != null)
            {
                string str = Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_MST_ContactCategory_SelectByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = ContactCategoryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                MST_ContactCategoryModel modelMST_ContactCategory = new MST_ContactCategoryModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelMST_ContactCategory.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelMST_ContactCategory.ContactCategoryName = dr["ContactCategoryName"].ToString();
                    modelMST_ContactCategory.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelMST_ContactCategory.Modification = Convert.ToDateTime(dr["Modification"]);
                }
                return View("MST_ContactCategoryAddEdit", modelMST_ContactCategory);
            }

            return View("MST_ContactCategoryAddEdit");
        }

        #region Insert(Save)
        [HttpPost]
        public IActionResult Save(MST_ContactCategoryModel modelMST_ContactCategory)
        {
            string str = Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelMST_ContactCategory.ContactCategoryID == null)
            {
                cmd.CommandText = "PR_MST_ContactCategory_Insert";
                //cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelMST_ContactCategory.CreationDate;
                cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = DBNull.Value;

            }
            else
            {
                cmd.CommandText = "PR_MST_ContactCategory_UpdateByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelMST_ContactCategory.ContactCategoryID;
            }
            cmd.Parameters.Add("@ContactCategoryName", SqlDbType.NVarChar).Value = modelMST_ContactCategory.ContactCategoryName;
            //cmd.Parameters.Add("@Modification", SqlDbType.DateTime).Value = modelMST_ContactCategory.Modification;
            cmd.Parameters.Add("@Modification", SqlDbType.DateTime).Value = DBNull.Value;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_ContactCategory.ContactCategoryID == null)
                    TempData["ContactCategoryInsertMsg"] = "ContactCategory inserted successfully.";
                else
                    return RedirectToAction("Index");

            }

            conn.Close();
            return RedirectToAction("Index");

        }
        #endregion

    }
}
