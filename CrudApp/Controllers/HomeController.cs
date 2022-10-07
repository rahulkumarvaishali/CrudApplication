using CrudApp.Database;
using CrudApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CrudApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        CrudEntities entobj = new CrudEntities();
        public ActionResult Index()
        {
            var res = entobj.CrudTables.ToList();
            List<Test> clsobj = new List<Test>();
            foreach (var t in res)
            {
                clsobj.Add(new Test
                {
                    Id=t.Id,
                    Name=t.Name,
                    Email=t.Email,
                    Mobile=t.Mobile,
                    Salary=t.Salary,
                    Address=t.Address
                });
            }
            return View(clsobj);
        }
        public ActionResult UserData()
        {
            var data= entobj.LoginTables.ToList();
            List<LoginClass> classes = new List<LoginClass>();
            foreach (var item in data)
            {
                classes.Add(new LoginClass
                {
                    Id=item.Id,
                    Name=item.Name,
                    Email=item.Email,
                    Password=item.Password
                });
            }
            return View(classes);
        }
        [HttpGet]
        public ActionResult Form()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Form(Test obj)
        {
            CrudTable tblobj = new CrudTable();
            tblobj.Id = obj.Id;
            tblobj.Name = obj.Name;
            tblobj.Email=obj.Email;
            tblobj.Mobile=obj.Mobile;
            tblobj.Salary = obj.Salary;
            tblobj.Address=obj.Address;
            if (obj.Id == 0)
            {
                entobj.CrudTables.Add(tblobj);
                entobj.SaveChanges();
            }
            else
            {
                entobj.Entry(tblobj).State=System.Data.Entity.EntityState.Modified;
                entobj.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var edititem=entobj.CrudTables.Where(m=>m.Id==id).First();
            Test clsobj = new Test();
            clsobj.Id = edititem.Id;
            clsobj.Name = edititem.Name;
            clsobj.Email=edititem.Email;
            clsobj.Mobile=edititem.Mobile;
            clsobj.Salary = edititem.Salary;
            clsobj.Address=edititem.Address;

            return View("Form", clsobj);
        }
        public ActionResult Delete(int id)
        {
            var delitem=entobj.CrudTables.Where(m=>m.Id==id).First();
            entobj.CrudTables.Remove(delitem);
            entobj.SaveChanges();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginClass obj)
        {
            var res= entobj.LoginTables.Where(m=>m.Email==obj.Email).FirstOrDefault();
            if (res == null)
            {
                TempData["Email"] = "Wrong Email Please Enter Valid Email";
            }
            else
            {
                if(res.Email==obj.Email && res.Password == obj.Password)
                {
                    FormsAuthentication.SetAuthCookie(res.Email, false);
                    Session["Email"]=res.Email;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Pass"] = "Wrong Password Please Enter Valid Password";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(LoginClass obj)
        {
            LoginTable tblobj = new LoginTable();
            tblobj.Id = obj.Id;
            tblobj.Name = obj.Name;
            tblobj.Email = obj.Email;
            tblobj.Password = obj.Password;
            return View();
        }
        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}