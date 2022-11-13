using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT *,(select id from donors o where o.username='"+ obj.username + "' and password='" + obj.password + "') as DonerId FROM centerdetails", conn);
                    da.Fill(ds, "center");
                    C.dt = ds.Tables[0];
                    Session["Doner"] = ds.Tables[0].Rows[0]["DonerId"].ToString();
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
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("insert into donors(username,password,email,create_at) values(@username,@password,@email,@create_at)", conn);

                    cmd.Parameters.AddWithValue("@username", obj.username);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@create_at", DateTime.Now); 
                    try
                    {
                        conn.Open();
                        count = cmd.ExecuteNonQuery();
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
                    cmd.Parameters.AddWithValue("@DonateDate", (Convert.ToDateTime(obj.DonateDate)).ToString("yyyy-MM-dd"));
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
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT o.*,s.email FROM donerdetails o,donors s Where o.DonerId = s.id AND o.DonerId like ('" + (Session["Doner"]).ToString() + "') order by o.DonateDate", conn);
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
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT D.CREATE_AT,D.EMAIL,DD.BLOODGROP,DD.DONATEDATE,DD.REASON,DD.SUPERVISOR,DD.UNIT,C.CENTER_NAME,C.ADDRESS FROM DONERDETAILS DD, DONORS D,CENTERDETAILS C WHERE D.ID = DD.DONERID AND C.ID = DD.CENTERID AND DD.ID = '" + obj.id + "'", conn);                    
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
    }
}