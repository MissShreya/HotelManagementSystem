using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Practice1301.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult AddData(FormCollection frm)
        {
            string _erroe = "";
            string _msg = "";
            string _name= Convert.ToString(frm[""]);
            try
            {
                string[] pName = { };
                string[] pValue = { };
                DataTable tmpDT = new DALBase().ExecuteProcedure("", pName, pValue).Tables[0];
                List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();
                if (tmpDT.Rows.Count > 0)
                {
                    lstRows = BLS_Utility.GetJsonFromTable(tmpDT);
                    _erroe = "OK";
                    _msg = "Success";
                    return Json(new { ERROR = _erroe, MESSAGE = _msg, COUNT = tmpDT.Rows.Count, RECORD = lstRows }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    _erroe = "ERROR";
                    _msg = "Failed";
                    return Json(new { ERROR = _erroe, MESSAGE = _msg, COUNT = "0", RECORD = "" }, JsonRequestBehavior.DenyGet);

                }
            }
            catch(Exception ex) 
            {
                return Json(new { ERROR = "ERROR", MESSAGE = ex.Message, COUNT = "0", RECORD = "" }, JsonRequestBehavior.DenyGet);
            }
           


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<JsonResult> GetRegList(string Mode)
        {
            string _error = "";
            string _msg = "";
            using (HttpClient httpClient=new HttpClient())
            {
                try
                {
                    string url = $"https://localhost:7022/api/Registration/GetRegDataUsingDTO?Mode={Mode}";
                    HttpResponseMessage response= await httpClient.GetAsync(url);
                    if(response.IsSuccessStatusCode)
                    {
                        _error = "OK";
                        _msg = await response.Content.ReadAsStringAsync();
                        DataTable tmpDt= JsonConvert.DeserializeObject<DataTable>(_msg);
                        List<Dictionary<string,object>> _lstRows= new List<Dictionary<string,object>>();
                        _lstRows=BLS_Utility.GetJsonFromTable(tmpDt);
                        return Json(new {ERROR= _error, MESSAGE = _msg, COUNT = _lstRows.Count,RECORD = _lstRows});
                    }
                    else
                    {
                        _error = "ERROR";
                        _msg = "Data not retrieved from API.";
                        return Json(new { ERROR = _error, MESSAGE = _msg, COUNT = "0", RECORD = "" }, JsonRequestBehavior.DenyGet);

                    }

                }
                catch (Exception ex)
                {
                    _error = "ERROR";
                    _msg= ex.Message;
                    return Json(new { ERROR=_error,MESSAGE=_msg,COUNT="0",RECORD=""},JsonRequestBehavior.DenyGet);
                }
            }
        }

        public async Task<JsonResult> EditRegRecord(FormCollection frm)
        {
            string _error = "";
            string _msg = "";


            string id = Convert.ToString(frm["hdPKID"]);
            string name = Convert.ToString(frm["strNameE"]);
            string course = Convert.ToString(frm["strCourseE"]);
            string rollno = Convert.ToString(frm["strRollNoE"]);
            string email = Convert.ToString(frm["strEmailE"]);
            string status = Convert.ToString(frm["strStatusE"]);

            try
            {
                var data = new
                {
                    mode = "UPDATE",
                    id = id,
                    strRollNo = rollno,
                    strName = name,
                    strEmail = email,
                    strCourse = course,
                    ynStatus = status
                };
                string url = $"https://localhost:7022/api/Registration/UpdateRedDataNormally";
                using(HttpClient client = new HttpClient())
                {
                 
                    string jsonData = JsonConvert.SerializeObject(data);

                    HttpResponseMessage response = await client.PutAsync(url,new StringContent(jsonData,Encoding.UTF8,"application/json"));
                    if(response.IsSuccessStatusCode)
                    {

                        string result= await response.Content.ReadAsStringAsync();
                        try
                        {
                            _error = "OK";
                            _msg = "Successfully update.";
                            return Json(new { ERROR = _error, MESSAGE = _msg }, JsonRequestBehavior.DenyGet);
                        }
                        catch(JsonReaderException ex)
                        {
                            throw ex;
                        }
                      
                    }
                    else
                    {

                    }
                }
            }
            catch(Exception ex)
            {

            }

        }
        
        
        public  async Task<JsonResult> AddAPI(FormCollection frm)
        {
            string _error = "";
            string _msg = "";
            string name = Convert.ToString(frm["strName"]);
            string course = Convert.ToString(frm["strCourse"]);
            string rollno = Convert.ToString(frm["strRollNo"]);
            string email = Convert.ToString(frm["strEmail"]);
            string pwd = Convert.ToString(frm["strPwd"]);
            string status = Convert.ToString(frm["strStatus"]);
            string url = $"https://localhost:7022/api/Registration/AddRegDataUsingStoredProceudre";
            var data = new
            {
                mode = "",
                strRollNo = rollno,
                strEmail = email,
                strPassword = pwd,
                strStatus = status,
                strCntPassword = pwd,
                strCourse = course,
                ynStatus = status

            };
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {

                    string jsonData = JsonConvert.SerializeObject(data);
                    HttpResponseMessage response= await httpClient.PostAsync(url,new StringContent(jsonData, Encoding.UTF8, "application/json"));

                    string result= await response.Content.ReadAsStringAsync();
                    try
                    {
                        _error = "OK";
                        _msg = result;
                    }
                    catch (JsonReaderException ex)
                    {
                        _error = "Invalid JSON format";
                        _msg = "The response content is not in valid JSON format. Response: " + result;
                    }

                }
                catch(Exception ex)
                {
                    _error = "Exception occurred";
                    _msg = ex.Message;
                }

            }
            return Json(new { ERROR = _error, MESSAGE = _msg }, JsonRequestBehavior.AllowGet);
        }


        
        
        public async Task<JsonResult> EditAPI(FormCollection frm)
        {
            string _error = "";
            string _msg = "";
            string id = Convert.ToString(frm["hdPKID"]);
            string name = Convert.ToString(frm["strNameE"]);
            string course = Convert.ToString(frm["strCourseE"]);
            string rollno = Convert.ToString(frm["strRollNoE"]);
            string email = Convert.ToString(frm["strEmailE"]);
            string status = Convert.ToString(frm["strStatusE"]);

            var data = new
            {
                mode = "UPDATE",
                id = id,
                strRollNo = rollno,
                strName = name,
                strEmail = email,
                strCourse = course,
                ynStatus = status
            };

            string url = "https://localhost:7022/api/Registration/UpdateRedDataNormally";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonData = JsonConvert.SerializeObject(data);

                    HttpResponseMessage response = await client.PutAsync(url, new StringContent(jsonData, Encoding.UTF8, "application/json"));



                    string result = await response.Content.ReadAsStringAsync();
                    try
                    {
                        _error = "OK";
                        _msg = result;
                    }
                    catch (JsonReaderException ex)
                    {
                        _error = "Invalid JSON format";
                        _msg = "The response content is not in valid JSON format. Response: " + result;
                    }
                }
                catch (Exception ex)
                {
                    _error = "Exception occurred";
                    _msg = ex.Message;
                }
            }
            return Json(new { ERROR = _error, MESSAGE = _msg }, JsonRequestBehavior.AllowGet);
        }



        //public async Task<JsonResult> DeleteAPI(FormCollection frm)
        //{
        //    string _error = "";
        //    string _msg = "";
        //    string hdID = Convert.ToString(frm["hdPKIDD"]);
        //    string url = $"https://localhost:7022/api/TblApiRegistrationMasters/{hdID}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            HttpResponseMessage response = await client.DeleteAsync(url);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                _error = "OK";
        //                _msg = "Record deleted successfully.";
        //            }
        //            else
        //            {
        //                string errorDetails = await response.Content.ReadAsStringAsync();
        //                _error = $"Failed to delete the record. Status code: {response.StatusCode}, Details: {errorDetails}";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _error = "Exception occurred";
        //            _msg = ex.Message;
        //        }
        //    }
        //    return Json(new { ERROR = _error, MESSAGE = _msg }, JsonRequestBehavior.AllowGet);
        //}





    }
}