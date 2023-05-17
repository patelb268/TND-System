using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TND.Models;
using System.Data.SqlClient;
using System.Web.Security;

using System.IO;
using System.Data;
using System.Configuration;

using System.Web.Script.Serialization; //  nampespace required to create the Json String

//using System.Web.Mvc;
//using System.Web.Mvc.Html;



namespace TND.Controllers
{
    public class EmpController : Controller
    {
        // GET: Emp
        TNDEntities s = new TNDEntities();

        [HttpPost]
        public ActionResult Login(string UID, string UPwd)
        {
            var slist = s.Employees.SqlQuery("select * from Employee where Emp_ID=@id", new SqlParameter("@id", UID)).FirstOrDefault<Employee>();
            
            if (slist.Emp_ID == Convert.ToDecimal(UID) && slist.Password == UPwd && slist.Status == true)
            {
                if (slist.Type == true)
                {
                    Session["ID"] = slist.Emp_ID;
                    Session["NAME"] = slist.Emp_Name;
                    Session["LID"] = slist.Location;
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {                
                    Session["ID"] = slist.Emp_ID;
                    Session["NAME"] = slist.Emp_Name;
                    Session["LID"] = slist.Location;
                    foreach (var l in s.Locations)
                    {
                        if (l.LTLocation_Code == Convert.ToInt32(@Session["LID"]))
                        {
                            foreach (var m in s.Month_End)
                            {
                                if (l.Location_ID == m.Location_ID && m.Status == true)
                                {
                                    Session["Month"] = m.Month;
                                    Session["Year"] = m.Year;
                                    Session["LocID"] = l.Location_ID;
                                }
                            }
                        }
                    }
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {

                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();

        }

        public ActionResult Dashboard()
        {
            if (Session["ID"] != null)
            {
                //feeder_wise_loss_graph_start
                var data = s.TND_Cal.SqlQuery("select *  from TND_Cal ").ToList<TND_Cal>();
                var X = data.Select(x => x.Feeder_Nm).ToList();
                var Y = data.Select(y => y.FLoss).ToList();

                JavaScriptSerializer si = new JavaScriptSerializer();
                string xval = si.Serialize(X); 
                string yval = si.Serialize(Y);

                ViewBag.X_VAL = xval;
                ViewBag.Y_VAL = yval;

                //feeder_wise_loss_gaph_end 

                //change_over_table_start
                int lid = Convert.ToInt32(@Session["LOCID"]);
                int cm = Convert.ToInt32(@Session["Month"]);
                int cy = Convert.ToInt32(@Session["Year"]);

                var changeover = s.Change_Over.SqlQuery("select * from Change_Over where Location_ID=@lid and Month=@cm and Year=@cy",
                    new SqlParameter("@lid", lid),
                    new SqlParameter("@cm", cm),
                    new SqlParameter("@cy", cy)).ToList<Change_Over>();


                var feederlist = s.Feeder_Master.ToList();
                ViewBag.FeederList = feederlist;

                return View(changeover);
                //change_over_table_end
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }

        }

        public ActionResult Feeder_Master()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Feeder_Master(FormCollection form)
        {
            var data = new Feeder_Master
            {
                Feeder_Code = Convert.ToInt32(form["F_Code"]),
                Feeder_Nm = form["F_Name"],
                Sdivn_Cd = Convert.ToInt32(form["SDn_Code"]),
                Feeder_Cat = form["F_Cat"],
                Feeder_Tp = form["F_Type"],
                Capacity = Convert.ToInt32(form["Cap"]),
                Feeder_Len = Convert.ToDouble(form["F_Len"]),
                Feeder_Ld = Convert.ToInt32(form["F_Load"]),
                Conn_Load = Convert.ToInt32(form["Conn_Load"]),
                Theo_Loss = Convert.ToDouble(form["T_Loss"]),
                Pyear_Loss = Convert.ToDouble(form["PY_Loss"]),
                Lt_Loc = Convert.ToInt32(form["LTL_Code"]),
                Lt_Feed = Convert.ToInt32(form["LTF_Code"]),
                Tr_ss = Convert.ToInt32(form["TRSS_Code"]),
                Tr_Fd= Convert.ToInt32(form["TRF_Code"]),
                Tr_Sdivn= Convert.ToInt32(form["TR_Sdivn"])
            };
            s.Feeder_Master.Add(data);
            s.SaveChanges();
            return RedirectToAction("Feeder_Master");

        }

        [HttpPost]
        public ActionResult Feeder_Master_Update(FormCollection form)
        {
            s.Database.ExecuteSqlCommand("update Feeder_Master set Feeder_Nm= @Feeder_Nm, Sdivn_Cd= @Sdivn_Cd, Feeder_Cat= @Feeder_Cat, Feeder_Tp= @Feeder_Tp, Capacity=@Capacity,Feeder_Len= @Feeder_Len, Feeder_Ld= @Feeder_Ld, Conn_Load= @Conn_Load, Theo_Loss= @Theo_Loss, Pyear_Loss= @Pyear_Loss,Lt_Loc= @Lt_Loc, Lt_Feed= @Lt_Feed, Tr_ss= @Tr_ss, Tr_Fd= @Tr_Fd, Tr_Sdivn= @Tr_Sdivn where Feeder_Cd = @Feeder_Cd",
                                new SqlParameter("@Feeder_Cd", Convert.ToInt32(form["F_Code"])),
                                new SqlParameter("@Feeder_Nm",form["F_Name"]),
                                new SqlParameter("@Sdivn_Cd", Convert.ToInt32(form["SDn_Code"])),
                                new SqlParameter("@Feeder_Cat", form["F_Cat"]),
                                new SqlParameter("@Feeder_Tp", form["F_Type"]),
                                new SqlParameter("@Capacity", Convert.ToInt32(form["Cap"])),
                                new SqlParameter("@Feeder_Len", Convert.ToDouble(form["F_Len"])),
                                new SqlParameter("@Feeder_Ld", Convert.ToInt32(form["F_Load"])),
                                new SqlParameter("@Conn_Load", Convert.ToInt32(form["Conn_Load"])),
                                new SqlParameter("@Theo_Loss", Convert.ToDouble(form["T_Loss"])),
                                new SqlParameter("@Pyear_Loss", Convert.ToDouble(form["PY_Loss"])),
                                new SqlParameter("@Lt_Loc", Convert.ToInt32(form["LTL_Code"])),
                                new SqlParameter("@Lt_Feed", Convert.ToInt32(form["LTF_Code"])),
                                new SqlParameter("@Tr_ss", Convert.ToInt32(form["TRSS_Code"])),
                                new SqlParameter("@Tr_Fd", Convert.ToInt32(form["TRF_Code"])),
                                new SqlParameter("@Tr_Sdivn", Convert.ToInt32(form["TR_Sdivn"])));
            s.SaveChanges();
            return RedirectToAction("Feeder_Master");

        }

        [HttpGet]
        public ActionResult SS_Interface()
        {
            List<Ss_Master> sscodes = s.Ss_Master.ToList();
            ViewBag.sscodes = new SelectList(sscodes, "Ss_Code","Ss_Name");

            return View();

        }

        public JsonResult GetFeederList(int Ss_Code)
        {
            s.Configuration.ProxyCreationEnabled = false;
            List<Feeder_Master> feeder = s.Feeder_Master.Where(x => x.Tr_ss == Ss_Code).ToList();
            return Json(feeder, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SS_Interface(FormCollection form)
        {
            var data = new Ss_Interface
            {
                Ss_Code = Convert.ToInt32(form["Sscode"]),
                Month = Convert.ToInt32(form["Month"]),
                interface_units = Convert.ToInt32(form["Inunits"]),
                            };
            s.Ss_Interface.Add(data);
            s.SaveChanges();

            return RedirectToAction("SS_Interface");

           

        }

        [HttpPost]
        public ActionResult SS_Update(FormCollection form)
        {
            s.Database.ExecuteSqlCommand("update Ss_interface set Month=@Month,interface_units=@interface_units where Ss_Code=@Ss_Code",
               new SqlParameter("@Month",form["Month"]),
                new SqlParameter("@Ss_Code",Convert.ToInt32(form["Sscode"])),
                 new SqlParameter("@interface_units", Convert.ToInt32(form["Inunits"])));
            s.SaveChanges();
            return RedirectToAction("SS_Interface");

        }

        public ActionResult Change_Over_Units()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Change_Over_Units(FormCollection form)
        {
            var data = new Change_Over
            {
                Year = Convert.ToInt32(form["Year"]),
                Month = Convert.ToInt32(form["Month"]),
                F_Feeder = Convert.ToInt32(form["F_Feeder"]),
                T_Feeder = Convert.ToInt32(form["T_Feeder"]),
                F_Units = Convert.ToInt32(form["F_Units"]),
                T_Units = Convert.ToInt32(form["T_Units"])
            };

            s.Change_Over.Add(data);
            s.SaveChanges();

            return RedirectToAction("Change_Over_Units");

            

        }

        public ActionResult Temp_Cpp_Detail()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Temp_Cpp_Detail(FormCollection form)
        {
            var data = new Temp_CPP
            {
                Month = Convert.ToInt32(form["Month"]),
                S_Div_CD= Convert.ToInt32(form["SDn_CD"]),
                Feeder_CD = Convert.ToInt32(form["F_CD"]),
                Temp_Units= Convert.ToInt32(form["T_Un"]),
                Temp_Assessment = Convert.ToDouble(form["T_Asse"]),
                Temp_Arrears = Convert.ToDouble(form["T_Arr"]),
                Temp_Collection = Convert.ToDouble(form["T_Col"]),
                Cpp_Units= Convert.ToInt32(form["Cpp_Un"]),
                Cpp_Assessment= Convert.ToDouble(form["Cpp_Asse"])


            };

            s.Temp_CPP.Add(data);
            s.SaveChanges();

            return RedirectToAction("Temp_Cpp_Detail");

        }

        [HttpPost]
        public ActionResult Temp_Cpp_Detail_Update(FormCollection form)
        {
            s.Database.ExecuteSqlCommand("update Temp_CPP set Month=@Month, Feeder_CD=@Feeder_CD, Temp_Units=@Temp_Units, Temp_Assessment=@Temp_Assessment, Temp_Collection=@Temp_Collection, Temp_Arrears=@Temp_Arrears, Cpp_Units=@Cpp_Units, Cpp_Assessment=@Cpp_Assessment where S_Div_CD=@S_Div_CD",
               new SqlParameter("@Month", form["Month"]),
                new SqlParameter("@S_Div_CD", Convert.ToInt32(form["SDn_CD"])),
                 new SqlParameter("@Feeder_CD", Convert.ToInt32(form["F_CD"])),
                 new SqlParameter("@Temp_Units", Convert.ToInt32(form["T_Un"])),
                 new SqlParameter("@Temp_Assessment", Convert.ToDouble(form["T_Asse"])),
                 new SqlParameter("@Temp_Collection", Convert.ToDouble(form["T_Col"])),
                 new SqlParameter("@Temp_Arrears", Convert.ToDouble(form["T_Arr"])),
                 new SqlParameter("@Cpp_Units", Convert.ToInt32(form["Cpp_Un"])),
                  new SqlParameter("@Cpp_Assessment", Convert.ToDouble(form["Cpp_Asse"])));

            s.SaveChanges();
            return RedirectToAction("Temp_Cpp_Detail");

        }

        [HttpGet]
        public ActionResult Month_End()
        {
            if (Session["ID"] != null)
            {
                foreach (var l in s.Locations)
                {
                    if (l.LTLocation_Code == Convert.ToInt32(@Session["LID"]))
                    {
                        foreach (var m in s.Month_End)
                        {
                            if (l.Location_ID == m.Location_ID && m.Status == true)
                            {
                                ViewBag.lid = m.Location_ID;
                                ViewBag.month = m.Month;
                                ViewBag.year = m.Year;
                            }
                        }
                    }
                }                             
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpPost]
        public ActionResult Month_End(string close) //name of submit button and parameter name same i.e.close here
        {
            string[] data = close.Split('/');
            int lid = Convert.ToInt32(data[0]);
            bool status = false;

            s.Database.ExecuteSqlCommand("update Month_End set Status=@status where Location_ID=@id",
               new SqlParameter("@status", status),
                new SqlParameter("@id",lid));
            
            s.SaveChanges();

            int month = Convert.ToInt32(data[1]);
            int year = Convert.ToInt32(data[2]);

            if (month == 12)
            {
                month = 1;
                year++;
            }
            else
                month++;

            var entry = new Month_End
            {
                Location_ID = lid,
                Month = month,
                Year=year,
                Status=true
            };
            s.Month_End.Add(entry);
            s.SaveChanges();
            Session["Month"] = month;
            Session["Year"] = year;
            return RedirectToAction("Month_End","Emp");
        }

        public ActionResult Complain()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        public ActionResult Upload_Data()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpPost]
        public ActionResult Upload_Data(HttpPostedFileBase postedFile)
        {
           
            
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Create a DataTable.
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[75] {
     new DataColumn("Location_ID",typeof(string)),
     new DataColumn("MONTH", typeof(string)),
     new DataColumn("YEAR", typeof(string)),
     new DataColumn("CIRCLE_CODE", typeof(string)),
     new DataColumn("CIRCLE_NAME", typeof(string)),
     new DataColumn("DIVISION_CODE", typeof(string)),
     new DataColumn("DIVISION_NAME", typeof(string)),
     new DataColumn("SUBDIV_CODE", typeof(string)),
     new DataColumn("SUBDIV_NAME", typeof(string)),
     new DataColumn("LOCATION_CODE", typeof(string)),
     new DataColumn("CONSUMER_NO", typeof(string)),
     new DataColumn("CONSUMER_NAME", typeof(string)),
     new DataColumn("ADDRESS1", typeof(string)),
     new DataColumn("ADDRESS2", typeof(string)),
     new DataColumn("CENCUS_CODE", typeof(string)),
     new DataColumn("TRANS_LOCATION", typeof(string)),
     new DataColumn("SD_AMOUNT", typeof(string)),
     new DataColumn("CONNECTION_DATE", typeof(string)), //
     new DataColumn("POLE_CODE", typeof(string)),
     new DataColumn("PDC_DATE", typeof(string)), //
     new DataColumn("IND_TYPE", typeof(string)),
     new DataColumn("SEASONAL_IND", typeof(string)),
     new DataColumn("SD_AMOUNT_RECEIPT_DATE", typeof(string)), //
     new DataColumn("SD_AMOUNT_RECEIPT_NO", typeof(string)),
     new DataColumn("CONTRACT_LOAD", typeof(string)),
     new DataColumn("TARRIF", typeof(string)),
     new DataColumn("CYCLE_NO", typeof(string)),
     new DataColumn("METER_READER_NO", typeof(string)),
     new DataColumn("BOOK_NO", typeof(string)),
     new DataColumn("ROUTE_CODE", typeof(string)),
     new DataColumn("METER_NO", typeof(string)),
     new DataColumn("METER_STATUS", typeof(string)),
     new DataColumn("EDUTY_CODE", typeof(string)),
     new DataColumn("EDUTY_AMOUNT", typeof(string)),
     new DataColumn("CONTRACT_DEMAND", typeof(string)),
     new DataColumn("FEEDER_NO", typeof(string)),
     new DataColumn("PHASE", typeof(string)),
     new DataColumn("METER_RENT_TYPE", typeof(string)),
     new DataColumn("THEFT_ARREARS", typeof(string)),
     new DataColumn("LITIGATION_ARREARS", typeof(string)),
     new DataColumn("FIXED_CHG", typeof(string)),
     new DataColumn("FUEL_CHG", typeof(string)),
     new DataColumn("FUSE_CHG", typeof(string)),
     new DataColumn("CREDIT_ADJUSTMENT", typeof(string)),
     new DataColumn("DEBIT_ADJUSTMENT", typeof(string)),
     new DataColumn("CLOSING_ARREARS", typeof(string)),
     new DataColumn("CONSUMPTION_UNIT", typeof(string)),
     new DataColumn("ASSESSMENT_UNIT", typeof(string)),
     new DataColumn("DPC_AMOUNT", typeof(string)),
     new DataColumn("RELIEF_AMOUNT", typeof(string)),
     new DataColumn("ENERGY_CHARGE", typeof(string)),
     new DataColumn("BOARD_CHARGE", typeof(string)),
     new DataColumn("PROV_BILL_AMOUNT", typeof(string)),
     new DataColumn("PAYMENT_AMOUNT", typeof(string)),
     new DataColumn("BILL_AMOUNT", typeof(string)),
     new DataColumn("START_METER", typeof(string)),
     new DataColumn("END_METER", typeof(string)),
     new DataColumn("AVG_UNIT", typeof(string)),
     new DataColumn("RE_START_READING", typeof(string)),
     new DataColumn("RE_END_READING", typeof(string)),
     new DataColumn("RE_CONSUMPTION", typeof(string)),
     new DataColumn("BILL_DATE", typeof(string)), //
     new DataColumn("DUE_DATE", typeof(string)), //
     new DataColumn("PAYMENT_DATE", typeof(string)), //
     new DataColumn("FEEDER_CODE", typeof(string)),
     new DataColumn("FEEDER_NAME", typeof(string)),
     new DataColumn("VILLAGE_NAME", typeof(string)),
     new DataColumn("TARRIF_SHORT", typeof(string)),
     new DataColumn("METER_CHANGE_DATE", typeof(string)), //
     new DataColumn("ADJUSTMENT_UNIT", typeof(string)),
     new DataColumn("OLD_TARIFF_IND", typeof(string)),
     new DataColumn("BILL_DEMAND", typeof(string)),
     new DataColumn("BILL_TYPE", typeof(string)),
     new DataColumn("STATUS", typeof(string)),
     new DataColumn("LAST_PAYDATE", typeof(string)) //
    });
                string lid = "0";
                foreach (var l in s.Locations)
                {
                    if (l.LTLocation_Code == Convert.ToInt32(@Session["LID"]))
                    {

                        lid = Convert.ToString(l.Location_ID);

                    }
                }


                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);
                int firstflag = 1;
                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (firstflag == 2)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] =lid;
                            int i = 1;

                            //Execute a loop over the columns.
                            foreach (string cell in row.Split('|'))
                            {
                                if (i == 75)
                                    break;
                                dt.Rows[dt.Rows.Count - 1][i] = cell;

                                i++;
                            }
                        }

                        if (firstflag == 1)
                            firstflag = 2;
                    }

                }
                string conString = ConfigurationManager.ConnectionStrings["TNDEntities2"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.Upload_Data";

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult TND_Cal()
        {
            if (Session["ID"] != null)
            {
                    return View(); 
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }
     
        [HttpPost]
        public ActionResult TND_Cal(string close)
        {
            
                int lid = Convert.ToInt32(@Session["LOCID"]);
                int cm = Convert.ToInt32(@Session["Month"]);
                int cy = Convert.ToInt32(@Session["Year"]);

                int sum = 0; int sum1 = 0; int s1 = 0; int s2 = 0; int tpp = 0; int cpp = 0; int fl = 0; int si = 0;
                double tnd = 0;

                string Data = "";//for perticular one feeder
                List<string> list = new List<string>();//list of feeders

                foreach (var fm in s.Feeder_Master)
                {

                    foreach (var ssi in s.Ss_Interface)
                    {
                        if (fm.Feeder_Code == ssi.Feeder_Cd && fm.Tr_ss == ssi.Ss_Code && lid == ssi.Location_ID && cm == ssi.Month && cy == ssi.Year)
                        {
                            si = ssi.interface_units;//sentout
                            Data = "" + ssi.Feeder_Cd + "|" + fm.Feeder_Nm + "|" + ssi.interface_units;

                            foreach (var up in s.Upload_Data)
                            {
                                if (fm.Feeder_Code == Convert.ToInt32(up.FEEDER_CODE) && lid == ssi.Location_ID && cm == ssi.Month && cy == ssi.Year)
                                {
                                    sum = sum + Convert.ToInt32(up.CONSUMPTION_UNIT);
                                }
                            }

                            foreach (var tc in s.Temp_CPP)
                            {
                                if (fm.Feeder_Code == tc.Feeder_CD && lid == tc.Location_ID && cm == tc.Month && cy == tc.Year)
                                {
                                    tpp = Convert.ToInt32(tc.Temp_Units);
                                    cpp = Convert.ToInt32(tc.Cpp_Units);
                                }
                            }

                            var sql = s.Change_Over.SqlQuery("select * from Change_Over where (F_Feeder = @fe or T_Feeder =@fe) and Location_ID=@lid and Month=@m and Year=@y",
                                new SqlParameter("@fe", fm.Feeder_Code),
                                new SqlParameter("@lid", lid),
                                new SqlParameter("@m", cm),
                                new SqlParameter("@y", cy)).ToList<Change_Over>();

                            foreach (var li in sql)
                            {
                                if (fm.Feeder_Code == li.F_Feeder)
                                {
                                    s1 = s1 + Convert.ToInt32(li.T_Units);
                                }
                                else
                                {
                                    s2 = s2 + Convert.ToInt32(li.T_Units);
                                }
                            }
                            sum1 = -(s1) + s2;//change over
                            fl = si + sum1 - (sum + tpp + cpp);//floss
                            tnd = (fl * 100) / (si + sum1);//tnd
                            Data = Data + "|" + sum + "|" + sum1 + "|" + tpp + "|" + cpp + "|" + fl + "|" + tnd;//data feeder wise
                            sum = 0; sum1 = 0; s1 = 0; s2 = 0;//for next feeder//no need to tnd=0 coz every time overwrite by formula
                            list.Add(Data);
                        }
                    }
                }

                foreach (var li in list)
                {
                    string[] dl = li.Split('|');

                    var data = new TND_Cal
                    {
                        Location_ID = Convert.ToInt32(lid),
                        Year = cy,
                        Month = cm,
                        Feeder_Cd = Convert.ToInt32(dl[0]),
                        Feeder_Nm = dl[1],
                        Sent_Out = Convert.ToInt32(dl[2]),
                        Consuption = Convert.ToInt32(dl[3]),
                        Change_Over = Convert.ToInt32(dl[4]),
                        Temp = Convert.ToInt32(dl[5]),
                        Cpp = Convert.ToInt32(dl[6]),
                        FLoss = Convert.ToInt32(dl[7]),
                        Tnd = dl[8]

                    };
                    dl = null;//making empty for upcoming next feeder 
                    s.TND_Cal.Add(data);
                    s.SaveChanges();
                }

            return RedirectToAction("TND_Cal", "Emp");
        }

       
        [HttpPost]
        public ActionResult ATandC(string close)
        {
            int lid = Convert.ToInt32(@Session["LOCID"]);
            int cm = Convert.ToInt32(@Session["Month"]);
            int cy = Convert.ToInt32(@Session["Year"]);

            int sold = 0; int sum1 = 0; int s1 = 0; int s2 = 0;int si = 0;
            double collection = 0; double assement = 0; double atnc = 0;double orignalCollection = 0;double unitrec = 0;


            string Data = "";//for perticular one feeder
            List<string> list = new List<string>();//list of feeders

            foreach (var fm in s.Feeder_Master)
            {

                foreach (var ssi in s.Ss_Interface)
                {
                    if (fm.Feeder_Code == ssi.Feeder_Cd && fm.Tr_ss == ssi.Ss_Code && lid == ssi.Location_ID && cm == ssi.Month && cy == ssi.Year)
                    {
                        si = ssi.interface_units;//sentout
                        Data = "" + ssi.Feeder_Cd + "|" + fm.Feeder_Nm + "|" + ssi.interface_units;

                        var sql = s.Change_Over.SqlQuery("select * from Change_Over where (F_Feeder = @fe or T_Feeder =@fe) and Location_ID=@lid and Month=@m and Year=@y",
                            new SqlParameter("@fe", fm.Feeder_Code),
                            new SqlParameter("@lid", lid),
                            new SqlParameter("@m", cm),
                            new SqlParameter("@y", cy)).ToList<Change_Over>();

                        foreach (var up in s.Upload_Data)
                        {
                            if (fm.Feeder_Code == Convert.ToInt32(up.FEEDER_CODE) && lid == ssi.Location_ID && cm == ssi.Month && cy == ssi.Year)
                            {
                                collection = collection + Convert.ToDouble(up.PAYMENT_AMOUNT);
                                assement = assement + Convert.ToDouble(up.ASSESSMENT_UNIT);
                                sold = sold + Convert.ToInt32(up.CONSUMPTION_UNIT);
                            }
                        }
                        orignalCollection = collection / assement;
                        unitrec = sold * orignalCollection;

                        foreach (var li in sql)
                        {
                            if (fm.Feeder_Code == li.F_Feeder)
                            {
                                s1 = s1 + Convert.ToInt32(li.T_Units);
                            }
                            else
                            {
                                s2 = s2 + Convert.ToInt32(li.T_Units);
                            }
                        }

                        sum1 = -(s1) + s2;//change over
                        atnc=((si+sum1)-unitrec)/ (si + sum1)*100;//AT&C 
                        Data = Data + "|" + collection + "|" + assement + "|" + sold + "|" + orignalCollection + "|" + unitrec + "|"+ sum1+"|" + atnc;//data feeder wise
                        sold = 0; collection = 0; assement = 0; sum1 = 0; s1 = 0; s2 = 0;//for next feeder
                        list.Add(Data);
                    }
                }
            }

           

            foreach (var li in list)
            {
                string[] dl = li.Split('|');

                var data = new ATandC_Cal
                {
                    Location_ID = Convert.ToInt32(lid),
                    Year = cy,
                    Month = cm,
                    Feeder_Cd = Convert.ToInt32(dl[0]),
                    Feeder_Nm = dl[1],
                    Sent_Out = Convert.ToInt32(dl[2]),
                    Payment = dl[3],
                    Assement = dl[4],
                    Sold = dl[5],
                    Collection = dl[6],
                    UnitRecord = dl[7],
                    Change_Over = Convert.ToInt32(dl[8]),
                    ATandC=dl[9]
                };
                dl = null;//making empty for upcoming next feeder 
                s.ATandC_Cal.Add(data);
                s.SaveChanges();
            }

            return RedirectToAction("TND_Cal", "Emp");
        }

        public ActionResult Tables_Tnd()
        {
            if (Session["ID"] != null)
            {
                var loclist = s.Locations.ToList();
                ViewBag.Locationlist = loclist;

                int lid = Convert.ToInt32(@Session["LOCID"]);
                int cm = Convert.ToInt32(@Session["Month"]);
                int cy = Convert.ToInt32(@Session["Year"]);

                var tnd = s.TND_Cal.SqlQuery("select * from TND_Cal where Location_ID=@lid and Month=@cm and Year=@cy",                                    
                            new SqlParameter("@lid", lid),
                            new SqlParameter("@cm", cm),
                            new SqlParameter("@cy", cy)).ToList<TND_Cal>();

                return View(tnd);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }

        }

        public ActionResult Tables_ATandC()
        {
            if (Session["ID"] != null)
            {
                var loclist = s.Locations.ToList();
                ViewBag.Locationlist = loclist;

                int lid = Convert.ToInt32(@Session["LOCID"]);
                int cm = Convert.ToInt32(@Session["Month"]);
                int cy = Convert.ToInt32(@Session["Year"]);

                var ATnC = s.ATandC_Cal.SqlQuery("select * from ATandC_Cal where Location_ID=@lid and Month=@cm and Year=@cy",
                            new SqlParameter("@lid", lid),
                            new SqlParameter("@cm", cm),
                            new SqlParameter("@cy", cy)).ToList<ATandC_Cal>();

                return View(ATnC);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }

        }

    }
}




