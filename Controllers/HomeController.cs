using Aspose.Words;
using Aspose.Words.Replacing;
using bloodbank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace bloodbank.Controllers
{
    public class HomeController : Controller
    {
        string myConnectionString = "server=localhost;uid=root;" + "database=bloodbank";
        public ActionResult Login()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.loginDetail obj)
        {
            DataSet ds = new DataSet("center");
            Models.CenterDetails C = new Models.CenterDetails();
            string donorID = ""; 
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select Count(*) from donors o where o.username=@username and password=@password", conn);
                    cmd.Parameters.AddWithValue("@username", obj.username);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        donorID = Convert.ToString(cmd.ExecuteScalar());
                        if (Convert.ToInt32(donorID) == 0)
                        {
                            return View("Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //--------------
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT *,(select id from donors o where o.username='"+ obj.username + "' and password='" + obj.password + "') as DonerId,(select Role from donors o where o.username='" + obj.username + "' and password='" + obj.password + "') as Role FROM centerdetails", conn);
                    da.Fill(ds, "center");
                    C.dt = ds.Tables[0];
                    Session["Doner"] = ds.Tables[0].Rows[0]["DonerId"].ToString();
                    Session["Role"] = ds.Tables[0].Rows[0]["Role"].ToString();      
                    //--------------
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                donorID = "";
            }
            Session["uid"] = obj.username;
            Session["pwd"] = obj.password;
            return View("Index",C);
        }
         
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(Models.RegistrationDetails obj)
        {
            int count = 0;
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select Count(*) from donors o where o.username=@username and password=@password", conn);
                    cmd.Parameters.AddWithValue("@username", obj.username);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            //ViewBag["MSG"]= "User Allready Exist.";
                            ViewBag.Message = "User Allready Exist.";
                            return View("UserRegistration");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    MySql.Data.MySqlClient.MySqlCommand cmd1 = new MySql.Data.MySqlClient.MySqlCommand("insert into donors(username,password,email,ROLE,create_at) values(@username,@password,@email,@Roles,@create_at)", conn);
                    cmd1.Parameters.AddWithValue("@username", obj.username);
                    cmd1.Parameters.AddWithValue("@password", obj.password);
                    cmd1.Parameters.AddWithValue("@email", obj.email);
                    cmd1.Parameters.AddWithValue("@Roles", (obj.Roles).ToString());
                    cmd1.Parameters.AddWithValue("@create_at", DateTime.Now);
                    try
                    {
                        //conn.Open();
                        count = cmd1.ExecuteNonQuery();
                        if (count == 0)
                        {
                            return View("UserRegistration");
                        }
                    }
                    catch (Exception ex)
                    {
                        return View("UserRegistration");
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                count = 0;
            }
            return View("Login");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["uid"] = null;
            Session["pwd"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult UserReport(Models.CenterDetails obj)
        {
            if (Convert.ToString(Session["Role"]) == "Manager")
            {
                return RedirectToAction("LoginData");
            }
            Session["CenterId"] = obj.id;
            return View();
        }

        [HttpPost]
        public ActionResult UserReport(Models.DonerDetails obj)
        {
            int count = 0;          
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("insert into DonerDetails(CenterId,DonateDate,Unit,Reason,Supervisor,BloodGrop,DonerId) values(@CenterId,@DonateDate,@Unit,@Reason,@Supervisor,@BloodGrop,@DonerId)", conn);
                    cmd.Parameters.AddWithValue("@CenterId", (Session["CenterId"]).ToString());
                    cmd.Parameters.AddWithValue("@DonateDate", (obj.DonateDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Unit", obj.Unit);
                    cmd.Parameters.AddWithValue("@Reason", obj.Reason);
                    cmd.Parameters.AddWithValue("@Supervisor", obj.Supervisor);
                    cmd.Parameters.AddWithValue("@BloodGrop", obj.BloodGrop);
                    cmd.Parameters.AddWithValue("@DonerId", (Session["Doner"]).ToString());
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        Session["Status"]="Failed";
                        return RedirectToAction("Status");
                    }
                    Session["Status"] = "Success";
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                count = 0;
            }
            return RedirectToAction("Status");
        }

        public ActionResult Status()
        {
            return View();
        }

        public ActionResult DonerDetails()
        {
            DataSet ds = new DataSet("Doner");
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    string Query = "";
                    //MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT o.*,s.email FROM donerdetails o,donors s Where o.DonerId = s.id AND o.DonerId like ('" + (Session["Doner"]).ToString() + "') order by o.DonateDate", conn);
                    if (Convert.ToString(Session["Role"]) == "Manager")
                    {
                        Query = "SELECT o.*,s.email,CASE WHEN o.Status = 'CANCELED' THEN 'CANCELED' WHEN o.DonateDate > CURRENT_DATE THEN 'Y' ELSE 'N' END CanStatus FROM donerdetails o, donors s Where o.DonerId = s.id order by o.DonateDate";
                    }
                    else
                    {
                        Query = "SELECT o.*,s.email,CASE WHEN o.Status = 'CANCELED' THEN 'CANCELED' WHEN o.DonateDate > CURRENT_DATE THEN 'Y' ELSE 'N' END CanStatus FROM donerdetails o, donors s Where o.DonerId = s.id AND o.DonerId in ('" + (Session["Doner"]).ToString() + "') order by o.DonateDate";
                    }
                    //MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT o.*,s.email,CASE WHEN o.Status = 'CANCELED' THEN 'CANCELED' WHEN o.DonateDate > CURRENT_DATE THEN 'Y' ELSE 'N' END CanStatus FROM donerdetails o, donors s Where o.DonerId = s.id AND o.DonerId in ('" + (Session["Doner"]).ToString() + "') order by o.DonateDate", conn);
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(Query, conn);
                    da.Fill(ds, "Doner");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
            }
            Models.DonerDetails D = new Models.DonerDetails();
            D.dt = ds.Tables[0];
            return View(D);
        }

        public ActionResult DeatailReport(Models.DonerDetails obj)
        {
            DataSet ds = new DataSet("Doner");
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    //string Query = "";
                    //if (Convert.ToString(Session["Role"]) == "Manager")
                    //{
                    //    Query = "SELECT D.CREATE_AT,D.EMAIL,DD.BLOODGROP,DD.DONATEDATE,DD.REASON,DD.SUPERVISOR,DD.UNIT,C.CENTER_NAME,C.ADDRESS,D.username FROM DONERDETAILS DD, DONORS D,CENTERDETAILS C WHERE D.ID = DD.DONERID AND C.ID = DD.CENTERID";
                    //}
                    //else
                    //{
                    //    Query = "SELECT D.CREATE_AT,D.EMAIL,DD.BLOODGROP,DD.DONATEDATE,DD.REASON,DD.SUPERVISOR,DD.UNIT,C.CENTER_NAME,C.ADDRESS,D.username FROM DONERDETAILS DD, DONORS D,CENTERDETAILS C WHERE D.ID = DD.DONERID AND C.ID = DD.CENTERID AND DD.ID = '" + obj.id + "'";
                    //}
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT D.CREATE_AT,D.EMAIL,DD.BLOODGROP,DD.DONATEDATE,DD.REASON,DD.SUPERVISOR,DD.UNIT,C.CENTER_NAME,C.ADDRESS,D.username,DD.ID FROM DONERDETAILS DD, DONORS D,CENTERDETAILS C WHERE D.ID = DD.DONERID AND C.ID = DD.CENTERID AND DD.ID = '" + obj.id + "'", conn);                    
                    //MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(Query, conn);
                    da.Fill(ds, "Doner");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
            }
            Models.DonerDetails D = new Models.DonerDetails();
            D.dt = ds.Tables[0];
            return View(D);
        }

        public ActionResult Back()
        {
            Session["uid"] = null;
            Session["pwd"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Cancel(string id)
        {
            int count = 0;
            Session["MSG"] = "";
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    //MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT o.*,s.email FROM donerdetails o,donors s Where o.DonerId = s.id AND o.DonerId like ('" + (Session["Doner"]).ToString() + "') order by o.DonateDate", conn);
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("Update DONERDETAILS SET Status='CANCELED' WHERE ID = '" + id + "'", conn);
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        Session["MSG"]= "Appointment Canceled Successfully...";
                        return RedirectToAction("DonerDetails");
                    } 
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Session["MSG"] = "Error While Canceling Appointment...";
            }
            return RedirectToAction("DonerDetails");
        }

        [HttpPost]
        public JsonResult Download(DownloadDetails DownloadDetails)
        {
            int count = 0;
            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT Count(*) FROM DONERDETAILS WHERE ID=@Id AND DonateDate >= @DonateDate AND Status Is NULL", conn);
                cmd.Parameters.AddWithValue("@Id", DownloadDetails.Id);
                cmd.Parameters.AddWithValue("@DonateDate", (DownloadDetails.Date).ToString("yyyy-MM-dd"));
                conn.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar()); 
            
            if (count > 0)
            {
                String FilePath;
                FilePath = Server.MapPath("/Files/Certificate.docx"); 
                Document doc = new Document(FilePath);
                FindReplaceOptions Options = new FindReplaceOptions();
                Options.MatchCase = true;  
                doc.Range.Replace("[Date]", (DownloadDetails.Date).ToString("yyyy-MM-dd"), Options);
                doc.Range.Replace("[Name]", DownloadDetails.User, Options);
                //string FileName = DownloadDetails.User + "_" + DownloadDetails.Date + "_" + (System.DateTime.Now).ToString("ddMMyyyyhhmmss");
                string FileName = DownloadDetails.User + "_" + (System.DateTime.Now).ToString("ddMMyyyyhhmmss");
                string path = Server.MapPath("/Files/" + FileName + ".docx");
                doc.Save(path);
                return Json("/Files/" + FileName + ".docx", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Certificate is not Exist.", JsonRequestBehavior.AllowGet);
            }
            }
        }

        public ActionResult LoginData()
        { 
            Models.loginDetail obj = new Models.loginDetail();
            obj.username = (Session["uid"]).ToString();
            obj.password = (Session["pwd"]).ToString(); 
            DataSet ds = new DataSet("center");
            Models.CenterDetails C = new Models.CenterDetails();
            string donorID = "";
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select Count(*) from donors o where o.username=@username and password=@password", conn);
                    cmd.Parameters.AddWithValue("@username", obj.username);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        donorID = Convert.ToString(cmd.ExecuteScalar());
                        if (Convert.ToInt32(donorID) == 0)
                        {
                            return View("Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //--------------
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT *,(select id from donors o where o.username='" + obj.username + "' and password='" + obj.password + "') as DonerId,(select Role from donors o where o.username='" + obj.username + "' and password='" + obj.password + "') as Role FROM centerdetails", conn);
                    da.Fill(ds, "center");
                    C.dt = ds.Tables[0];
                    Session["Doner"] = ds.Tables[0].Rows[0]["DonerId"].ToString();
                    Session["Role"] = ds.Tables[0].Rows[0]["Role"].ToString();
                    //--------------
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                donorID = "";
            }
            Session["uid"] = obj.username;
            Session["pwd"] = obj.password;
            return View("Index", C);
        }
    }
}
