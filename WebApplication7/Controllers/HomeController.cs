using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        FileUploadingEntities2 db = new FileUploadingEntities2();
        FileUp tbl = new FileUp();

        public ActionResult list()
        {

            return View(db.FileUps.ToList());
        }

        public ActionResult update(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileUp fileUp = db.FileUps.Find(id);
            if (fileUp == null)
            {
                return HttpNotFound();
            }
            return View(fileUp);
        }

        [HttpPost]

        public ActionResult update(int id, HttpPostedFileBase img1,FileUp model)
        {
           

            try
            {
                if (img1 != null) // Start Of First Outer If and this If Will Execute First
                {
                    var ex = Path.GetExtension(img1.FileName);
                    var data = (from t in db.FileUps where t.id == id select t.img).FirstOrDefault();


                    if (ex.Equals(".jpg") || ex.Equals(".png")) // Start Of First Inner If to check weather file contains .jpg or .png extension or not
                    {
                        int size = img1.ContentLength;

                        if (size > 2000) // Start Of Seond Inner If to check weather file Size equal to 2 mb or not
                        {
                           
                            string _path = Path.Combine(Server.MapPath("~/Content/Images"), data);
                            img1.SaveAs(_path);

                           
                            tbl = (from t in db.FileUps where t.id == id select t).FirstOrDefault();

                            model.img = data;

                            tbl.name = model.name;
                            tbl.roll = model.roll;
                            tbl.city = model.city;
                            tbl.img = model.img;
                            int k = db.SaveChanges();

                                ViewBag.data = "Data Is Updated";
                                return RedirectToAction("list");
                        }
                        else // End Of Seond Inner If
                        {
                            ViewBag.data = "Sorry File Is Greater Than 2 Mb";
                        }
                    }
                    else // End Of First inner If
                    {
                        ViewBag.data = "Please Select Image Only";
                    }
                }
                else // End Of First Outer If
                {
                    ViewBag.data = "Please Select A File";
                }
            }
            catch (Exception ex)
            {
                ViewBag.data = ex.Message;
            }

            return View();
        }
       

        public ActionResult dashboard()
        {
            if (Session["Name"] == null)
            {
                return View("Index");

            }
            else
            {
                return View("dashboard");
            }

        }

        public ActionResult logout()
        {
            if (Session["Name"] != null)
            {
                Session.Abandon();
                Session.Clear();

                return View("Index");
            }
            else
            {

                return View("Index");
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FileUp model, HttpPostedFileBase img1)
        {
            try
            {
                if (img1 != null) // Start Of First Outer If and this If Will Execute First
                {
                    var ex = Path.GetExtension(img1.FileName);
                    var FileName = Guid.NewGuid().ToString() + ex;


                    if (ex.Equals(".jpg") || ex.Equals(".png")) // Start Of First Inner If to check weather file contains .jpg or .png extension or not
                    {
                        int size = img1.ContentLength;

                        if (size >= 2000) // Start Of Seond Inner If to check weather file Size equal to 2 mb or not
                        {

                            string _path = Path.Combine(Server.MapPath("~/Content/Images"), FileName);
                            img1.SaveAs(_path);


                            model.img = FileName;

                            tbl.name = model.name;
                            tbl.roll = model.roll;
                            tbl.city = model.city;
                            tbl.img = model.img;

                            db.FileUps.Add(model);
                            int k = db.SaveChanges();

                            if (k > 0)
                            {
                                if (ModelState.IsValid)
                                {
                                    var data = db.FileUps.Where(x => x.name.Equals(model.name)).FirstOrDefault();

                                    if (data != null)
                                    {
                                        Session["Name"] = data.name.ToString();
                                        Session["img"] = data.img.ToString();

                                        return RedirectToAction("dashboard");

                                    }

                                }
                               
                            }
                        }
                        else // End Of Seond Inner If
                        {
                            ViewBag.data = "Sorry File Is Smaller Than 2 Mb";
                        }
                    }
                    else // End Of First inner If
                    {
                        ViewBag.data = "Please Select Image Only";
                    }
                }
                else // End Of First Outer If
                {
                    ViewBag.data = "Please Select A File";
                }
            }
            catch (Exception ex)
            {
                ViewBag.data = ex.Message;
            }

            return View();
        }
    }
}