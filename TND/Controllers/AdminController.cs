using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TND.Models;
using System.Data.SqlClient;

namespace TND.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        TNDEntities s = new TNDEntities();

        public ActionResult Dashboard()
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

        [HttpGet]
        public ActionResult Registration(string empid)
        {
            if (Session["ID"] != null)
            {
                if (empid != null)
                {
                    var slist = s.Employees.SqlQuery("select * from Employee where Emp_ID=@id", new SqlParameter("@id", empid)).FirstOrDefault<Employee>();

                    ViewBag.data_empid = slist.Emp_ID;
                    ViewBag.data_name = slist.Emp_Name;
                    ViewBag.data_lo = slist.Location;
                    ViewBag.data_email = slist.Email_ID;
                    ViewBag.data_mobile = slist.Mobile_No;
                    ViewBag.data_sta = slist.Status;
                    ViewBag.data_typ = slist.Type;
                    ViewBag.data_pass = slist.Password;
                    ViewBag.data_de = slist.Designation;

                }

                var des = s.Designations.ToList();
                SelectList list_des = new SelectList(des, "Designation_ID", "Designation_Name");
                ViewBag.data_des = list_des;

                var loc = s.Locations.ToList();

                SelectList list_loc = new SelectList(loc, "LTLocation_Code", "Location_Name");
                ViewBag.data_loc = list_loc;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }

        }

        [HttpPost]
        public ActionResult Registration(FormCollection form)
        {
            Boolean t, st;
            if (form["type"] == "Admin")
            {
                t = true;
            }
            else
            {
                t = false;
            }

            if (form["status"] == "Active")
            {
                st = true;
            }
            else
            {
                st = false;
            }

            var data = new Employee
            {
                Emp_ID = Convert.ToDecimal(form["EID"]),
                Emp_Name = form["EName"],
                Location = Convert.ToInt32(form["Eloc"]),
                Email_ID = form["EMail"],
                Mobile_No = Convert.ToDecimal(form["EMob"]),
                Status = st,
                Type = t,
                Password = form["EPwd"],
                Designation = Convert.ToInt32(form["Edes"])
            };
            s.Employees.Add(data);
            s.SaveChanges();

            return RedirectToAction("Registration");

        }

        [HttpPost]
        public ActionResult EmpUpdate(FormCollection form)
        {
            Boolean t, st;
            if (form["type"] == "Admin")
            {
                t = true;
            }
            else
            {
                t = false;
            }

            if (form["status"] == "Active")
            {
                st = true;
            }
            else
            {
                st = false;
            }


            s.Database.ExecuteSqlCommand("update Employee set Emp_Name=@name,Location=@loc,Email_ID=@email,Mobile_No=@mob,Status=@sta,Type=@type,Password=@pwd,Designation=@des where Emp_ID=@id",
               new SqlParameter("@id", Convert.ToDecimal(form["EID"])),
                new SqlParameter("@name", form["EName"]),
                 new SqlParameter("@loc", form["ELoc"]),
                  new SqlParameter("@email", form["Email"]),
                   new SqlParameter("@mob", Convert.ToDecimal(form["EMob"])),
                    new SqlParameter("@sta", st),
                    new SqlParameter("@type", t),
                     new SqlParameter("@pwd", form["EPwd"]),
                      new SqlParameter("@des", form["EDes"]));
            s.SaveChanges();
            return RedirectToAction("Registration");
        }

        [HttpGet]
        public ActionResult Designation()
        {
            if (Session["ID"] != null)
            {
                /* if (empid != null)
                 {
                     var slist = s.Employees.SqlQuery("select * from Employee where Emp_ID=@id", new SqlParameter("@id", empid)).FirstOrDefault<Employee>();

                     ViewBag.data_empid = slist.Emp_ID;
                     ViewBag.data_name = slist.Emp_Name;
                     ViewBag.data_lo = slist.Location;
                     ViewBag.data_email = slist.Email_ID;
                     ViewBag.data_mobile = slist.Mobile_No;
                     ViewBag.data_sta = slist.Status;
                     ViewBag.data_typ = slist.Type;
                     ViewBag.data_pass = slist.Password;
                     ViewBag.data_de = slist.Designation;

                 }*/


                var des = s.Designations.ToList();
                SelectList list_des = new SelectList(des, "Designation_Name", "Designation_Name");
                ViewBag.data_des = list_des;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpPost]
        public ActionResult Designation(FormCollection form)
        {
            var data = new Designation
            {
                Designation_ID = Convert.ToDecimal(form["DID"]),
                Designation_Name = form["Dna"]
            };
            s.Designations.Add(data);
            s.SaveChanges();

            return RedirectToAction("Designation");

        }
       
        [HttpGet]
        public ActionResult Location()
        {
            if (Session["ID"] != null)
            {
                /* if (empid != null)
                 {
                     var slist = s.Employees.SqlQuery("select * from Employee where Emp_ID=@id", new SqlParameter("@id", empid)).FirstOrDefault<Employee>();

                     ViewBag.data_empid = slist.Emp_ID;
                     ViewBag.data_name = slist.Emp_Name;
                     ViewBag.data_lo = slist.Location;
                     ViewBag.data_email = slist.Email_ID;
                     ViewBag.data_mobile = slist.Mobile_No;
                     ViewBag.data_sta = slist.Status;
                     ViewBag.data_typ = slist.Type;
                     ViewBag.data_pass = slist.Password;
                     ViewBag.data_de = slist.Designation;

                 }*/


                var loc = s.Locations.ToList();
                SelectList list_loc = new SelectList(loc, "Location_Name", "Location_Name","LTLocation_Code");
                ViewBag.data_loc = list_loc;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpPost]
        public ActionResult Location(FormCollection form)
        {
            var data = new Location
            {
                Location_ID = Convert.ToDecimal(form["LID"]),
                Location_Name = form["Lna"],
                LTLocation_Code= Convert.ToDecimal(form["LTCD"])
            };
            s.Locations.Add(data);
            s.SaveChanges();

            return RedirectToAction("Location");

        }

        public ActionResult Tables_Emp()
        {
            if (Session["ID"] != null)
            {
                var emps = s.Employees.SqlQuery("select * from Employee").ToList<Employee>();

                var loclist = s.Locations.ToList();
                ViewBag.Locationlist = loclist;

                var deslist = s.Designations.ToList();
                ViewBag.Desiglist = deslist;

                return View(emps);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }

        }

        public ActionResult Tables_Comp()
        {
            if (Session["ID"] != null)
            {
                var comps = s.Complains.SqlQuery("select * from Complain").ToList<Complain>();
                return View(comps);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }
        
        public ActionResult Tabels_Desig()
        {
            if (Session["ID"] != null)
            {
                var slist = s.Designations.SqlQuery(@"select * from  Designation").ToList<Designation>();
                return View(slist);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        public ActionResult Tables_Loc()
        {
            if (Session["ID"] != null)
            {
                var locs = s.Locations.SqlQuery("select * from Location").ToList<Location>();
                return View(locs);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpGet]
        public ActionResult Tables_Month_End()
        {
            if (Session["ID"] != null)
            {
                var MonthEnd = s.Month_End.SqlQuery("select * from Month_End").ToList<Month_End>();

                var loclist = s.Locations.ToList();
                ViewBag.Locationlist = loclist;

                return View(MonthEnd);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        [HttpPost]
        public ActionResult Tables_Month_End(string revert) //name of submit button and parameter name same i.e.close here
        {
            string[] data = revert.Split('/');
            int lid = Convert.ToInt32(data[0]);
            int month = Convert.ToInt32(data[1]);
            int year = Convert.ToInt32(data[2]);
           
            var req_entry = new Req_Revert_MonthEnd
            {
                Location_ID = lid,
                Month = month,
                Year=year,
                Status=true
            };
            s.Req_Revert_MonthEnd.Add(req_entry);
            s.SaveChanges();

            return RedirectToAction("Month_End", "Emp");
        }

        [HttpGet]
        public ActionResult Tables_Req_Revert()
        {
            if (Session["ID"] != null)
            {
                var RevertMonthEnd = s.Req_Revert_MonthEnd.SqlQuery("select * from Req_Revert_MonthEnd").ToList<Req_Revert_MonthEnd>();

                var loclist = s.Locations.ToList();
                ViewBag.Locationlist = loclist;

                return View(RevertMonthEnd);
            }
            else
            {
                return RedirectToAction("Login", "Emp");
            }
        }

        
        public ActionResult Req_Revert(int lid,int month,int year)
        {
            s.Database.ExecuteSqlCommand("delete from Req_Revert_MonthEnd where Location_ID=@id", new SqlParameter("@id", lid));
            s.SaveChanges();

            s.Database.ExecuteSqlCommand("delete from Month_End where Location_ID=@id and Month=@month and Year=@year", 
                new SqlParameter("@id", lid),
                new SqlParameter("@month", month),
                new SqlParameter("@year", year));
            s.SaveChanges();

            if (month == 1)
            {
                month = 12;
                year--;
            }
            else
                month--;

            s.Database.ExecuteSqlCommand("update Month_End set Status=@status where Location_ID=@id and Month=@month and Year=@year",
                new SqlParameter("@status",Convert.ToBoolean(1)),
                new SqlParameter("@id", lid),
                new SqlParameter("@month", month),
                new SqlParameter("@year", year));
            s.SaveChanges();
            
            return RedirectToAction("Tables_Req_Revert","Admin");
        }

        public ActionResult EmpDelete(string empid)
        {
            s.Database.ExecuteSqlCommand("delete from Employee where Emp_ID=@id", new SqlParameter("@id", Convert.ToDecimal(empid)));
            s.SaveChanges();
            return RedirectToAction("Tables_Emp");
        }

        public ActionResult DesigDelete(string did)
        {
            s.Database.ExecuteSqlCommand("delete from Designation where Designation_ID=@id", new SqlParameter("@id", Convert.ToDecimal(did)));
            s.SaveChanges();
            return RedirectToAction("Tabels_Desig");
        }

        public ActionResult LocDelete(string lid)
        {
            s.Database.ExecuteSqlCommand("delete from Location where Location_ID=@id", new SqlParameter("@id", Convert.ToDecimal(lid)));
            s.SaveChanges();
            return RedirectToAction("Tables_Loc");
        }
    }
}