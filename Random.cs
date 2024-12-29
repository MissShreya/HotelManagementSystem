
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SigmaIT;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Web.Security;
using System.Web.Mvc.Ajax;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;

namespace SigmaIT
{

    internal class DALBase
    {
        private string strCon = "";
        private System.Data.SqlClient.SqlConnection objConnection;
        private System.Data.DataSet dsResultSet;

        public DALBase()
        {
            this.strCon = System.Configuration.ConfigurationManager.AppSettings["DBcon"].ToString();
        }

        public void Create_Connection()
        {
            this.objConnection = null;
            try
            {
                this.objConnection = new System.Data.SqlClient.SqlConnection(this.strCon);
                if (this.objConnection.State == System.Data.ConnectionState.Closed || this.objConnection == null)
                {
                    this.objConnection.Open();
                }
            }
            catch
            {
            }
        }

        public void Close_Connection()
        {
            try
            {
                if (this.objConnection.State == System.Data.ConnectionState.Open || this.objConnection != null)
                {
                    this.objConnection.Close();
                    this.objConnection = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataSet ExecuteProcedure(string SPName, string[] pName, string[] pValue)
        {
            this.dsResultSet = null;
            try
            {
                this.dsResultSet = new System.Data.DataSet();
                this.Create_Connection();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.objConnection;
                sqlCommand.CommandText = SPName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < pName.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(pName[i], string.IsNullOrEmpty(pValue[i]) ? null : pValue[i]);
                }
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(this.dsResultSet);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                this.Close_Connection();
            }
            return this.dsResultSet;
        }

      

    }


    public class B_clsUtility
    {
        public static string DDMMYYYYtoYYYYMMDD(string _date)
        {
            string[] old = _date.Split('-');
            return (old[2] + "-" + old[1] + "-" + old[0]);

        }
        public static List<Dictionary<string, object>> GetJsonFromTable(DataTable tmpDT)
        {
            List<Dictionary<string, object>> _Json = new List<Dictionary<string, object>>();
            if (tmpDT.Rows.Count > 0)
            {
                Dictionary<string, object> dictRow = null;
                foreach (DataRow dr in tmpDT.Rows)
                {
                    dictRow = new Dictionary<string, object>();
                    foreach (DataColumn col in tmpDT.Columns)
                    {
                        dictRow.Add(col.ColumnName, dr[col]);
                    }
                    _Json.Add(dictRow);
                }
            }
            return _Json;
        }
    }

}



//function fn_GetData() { 
//        $.ajax({
//    type: "POST",
//            url: "@Url.Action("Action", "Controller")",
//            datatype: "json",
//            data: '{}',
//            success: function(result) {
//                if (result.COUNTP != "0")
//                {
//                    var data = [];
//                    data = result.RECORDP;
//                    for (var i = 0; i < data.length; i++)
//                    {
//                }

//                else{

//                }


//            }
//        })
//    }
// $(document).ready(function() {
    
//}
//function OnSuccess(result)
//{
//    if (result.ERROR == "OK")
//    {
//        result.MESSAGE;
//    }
//    else
//    {
//        return false;
//    }
//}

//@using(Ajax.BeginForm("Action", "Controller", null, new AjaxOptions
//{
//    HttpMethod = "POST",
//    OnSuccess = "OnSuccess",
//    OnFailure = "OnSuccess",
//    LoadingElementId = "loading"
//}, new { @class = "" }))
//        { 
//}


//public ActionResult Task(string ID)
//{
//    if (!(Request.IsAuthenticated) && string.IsNullOrEmpty(Convert.ToString(Session["AUTOID"])))
//    {
//        return RedirectToAction("LogOut", "Auth", null);
//    }
//    else
//    {
//        ViewBag.ID = _ID;
//        return View();
//    }

//}
//[HttpPost]
//public JsonResult Add(FormCollection frm)
//{
//    string _error = "";
//    string _mess = "";
//    try
//    {
//        if (string.IsNullOrEmpty(Convert.ToString(Session["AUTOID"])))
//        {
//            _error = "ERROR";
//            _mess = "You are Logged out Please Login Again.";
//            return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
//        }
//        else
//        {

//            string ABC = Convert.ToString(frm["ID"]);

//                string[] pName = { };

//                string[] pValue = {  };

//                DataSet tmpDS = (new DALBase().ExecuteProcedure("", pName, pValue));
//                DataTable tmpDT = tmpDS.Tables[0];
//                if (tmpDT.Rows.Count > 0)
//                {
//                    if (Convert.ToString(tmpDT.Rows[0]["code"]) == "0")
//                    {


//                        _error = "OK";
//                        _mess = Convert.ToString(tmpDT.Rows[0]["mess"]);
//                    }
//                    else
//                    {
//                        _error = "ERROR";
//                        _mess = "Some error occurred please try again later.";
//                    }
//                }
//                else
//                {
//                    _error = "ERROR";
//                    _mess = "Some error occurred please try again later.";
//                }
//            }
//            return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);

//        }
//    }
//    catch (Exception ex)
//    {
//        return Json(new { ERROR = "ERROR", MESSAGE = ex.Message }, JsonRequestBehavior.DenyGet);
//    }
//}
//[HttpPost]
//public ActionResult GetData(string _ID)
//{
//    string _error = "";
//    string _mess = "";
//    try
//    {
//        if (string.IsNullOrEmpty(Convert.ToString(Session["AUTOID"])))
//        {
//            _error = "ERROR";
//            _mess = "You are Logged out Please Login Again.";
//            return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
//        }
//        else
//        {
//            string _autoID = Convert.ToString(Session["AUTOID"]);
//            string[] pName = {  };
//            string[] pValue = {  };
//            DataTable dtTM = (new DALBase().ExecuteProcedure("", pName, pValue)).Tables[0];

//            List<Dictionary<string, object>> RowsTM = new List<Dictionary<string, object>>();
//            if (dtTM.Rows.Count > 0)
//            {
//                if (Convert.ToString(dtTM.Rows[0]["code"]) == "0")
//                {
//                    RowsTM = B_clsUtility.GetJsonFromTable(dtTM);
//                    _error = "OK";
//                    _mess = "Success";
//                    return Json(new { ERROR = _error, MESSAGE = _mess, TMCNT = dtTM.Rows.Count, TMRECORD = RowsTM }, JsonRequestBehavior.DenyGet);
//                }
//                else
//                {
//                    _error = "ERROR";
//                    _mess = Convert.ToString(dtTM.Rows[0]["mess"]);
//                    return Json(new { ERROR = _error, MESSAGE = _mess, TMCNT = dtTM.Rows.Count, TMRECORD = "" }, JsonRequestBehavior.DenyGet);
//                }
//            }
//            else
//            {
//                _error = "ERROR";
//                _mess = "Some error occured. Please try again later.";
//                return Json(new { ERROR = _error, MESSAGE = _mess, TMCNT = "0", TMRECORD = "" }, JsonRequestBehavior.DenyGet);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        return Json(new { ERROR = "ERROR", MESSAGE = ex.Message, TMCNT = "0", TMRECORD = "" }, JsonRequestBehavior.DenyGet);
//    }
//}

// < appSettings >
//    < add key = "webpages:Version" value = "3.0.0.0" />
//    < add key = "webpages:Enabled" value = "false" />
//    < add key = "ClientValidationEnabled" value = "true" />
//    < add key = "UnobtrusiveJavaScriptEnabled" value = "true" />

//    < add key = "DBcon" value = "data source=SHREYAWA;initial catalog=TESTDATA;Integrated Security=True" />
//  </ appSettings >

//public ActionResult Add()
//{
//    if (!(Request.IsAuthenticated) && string.IsNullOrEmpty(Convert.ToString(Session["AUTOID"])))
//    {
//        return RedirectToAction("LogOut", "Auth", null);
//    }
//    else
//    {
//        return View();
//    }
//}

//[AllowAnonymous]
//public ActionResult login()
//{
//    return View();
//}

//[HttpPost]
//[AllowAnonymous]
//public JsonResult login(FormCollection frm)
//{
//    string _error = "";
//    string _mess = "";
//    string _returnURL = "";
//    try
//    {
//        string sUserID = frm["sUserID"];
//        string sPassword = frm["sPassword"];
//        bool RememberMe = frm["rememberme"] == "on" ? true : false;

//        if (sUserID.Length > 2 && sPassword.Length > 2)
//        {
//            //string encpassword = Encryption64.encryptStringPWD(sPassword);
//            //string DEcpassword = Encryption64.encryptStringPWD("jBbGURziGvg=");

//            string[] pName = { "@UserId", "@Password" };
//            string[] pValue = { sUserID.Trim(), sPassword };
//            DataTable tmpDT = new DALBase().ExecuteProcedure("", pName, pValue).Tables[0];
//            if (tmpDT.Rows.Count > 0 && Convert.ToString(tmpDT.Rows[0]["Code"]) == "1")
//            {
//                Session["LOGINTIME"] = DateTime.Now.ToString("dd MMM yyyy HH:mm");
//                Session["LOGINID"] = sUserID;
//                Session["ROLENAME"] = Convert.ToString(tmpDT.Rows[0]["RoleName"]);
//                Session["DISPNAME"] = Convert.ToString(tmpDT.Rows[0]["EmpName"]);
//                Session["ROLEID"] = Convert.ToString(tmpDT.Rows[0]["RoleId"]);
//                Session["AUTOID"] = Convert.ToString(tmpDT.Rows[0]["AutoId"]);
//                //Session["ENCPWD"] = Convert.ToString(encpassword);
//                Session["PROFILEPIC"] = Convert.ToString(tmpDT.Rows[0]["ProfilePic"]);


//                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, sUserID, DateTime.Now, DateTime.Now.AddMinutes(30), RememberMe, sUserID);
//                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
//                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
//                Response.Cookies.Add(faCookie);
//                _error = "OK";
//                _mess = "Login Success !";
//                _returnURL = "/Task/dashboard";
//                return Json(new { ERROR = _error, MESSAGE = _mess, returnURL = _returnURL }, JsonRequestBehavior.DenyGet);

//            }
//            else if (tmpDT.Rows.Count > 0 && Convert.ToString(tmpDT.Rows[0]["Code"]) == "0")
//            {
//                _error = "Error";
//                _mess = "Your account is currently suspended please contact to admin.";
//                return Json(new { ERROR = _error, MESSAGE = _mess, returnURL = _returnURL }, JsonRequestBehavior.DenyGet);
//            }
//            else
//            {
//                _error = "Error";
//                _mess = "The user name or password provided is incorrect.";
//                return Json(new { ERROR = _error, MESSAGE = _mess, returnURL = _returnURL }, JsonRequestBehavior.DenyGet);
//            }
//        }
//        else
//        {
//            _error = "Error";
//            _mess = "The user name or password provided is incorrect.";
//            return Json(new { ERROR = _error, MESSAGE = _mess, returnURL = _returnURL }, JsonRequestBehavior.DenyGet);

//        }

//    }
//    catch (Exception ex)
//    {
//        return Json(new { ERROR = "Error", MESSAGE = ex.Message.Length > 25 ? ex.Message.Substring(0, 25) + "..." : ex.Message }, JsonRequestBehavior.DenyGet);
//    }
//}



//[AllowAnonymous]
//public ActionResult LogOut()
//{
//    FormsAuthentication.SignOut();
//    Session.Abandon();
//    Session.Clear();
//    Session.RemoveAll();
//    return RedirectToAction("login", "Auth", null);
//}