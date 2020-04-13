using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ImageUploadersController : Controller
    {
        private myDBEntities db = new myDBEntities();

        // GET: ImageUploaders
        public ActionResult Index()
        {
            return View(db.ImageUploaders.ToList());
        }

        // GET: ImageUploaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageUploader imageUploader = db.ImageUploaders.Find(id);
            if (imageUploader == null)
            {
                return HttpNotFound();
            }
            return View(imageUploader);
        }

        // GET: ImageUploaders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageUploaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string path = Server.MapPath("~/Images/" + ImageName);
                file.SaveAs(path);

                ImageUploader uploads = new ImageUploader();
                uploads.Avatar = ImageName;
                uploads.Email = Request.Form["Email"];
                uploads.Name = Request.Form["Name"];
                db.ImageUploaders.Add(uploads);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
           
        }

        // GET: ImageUploaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageUploader imageUploader = db.ImageUploaders.Find(id);

            if (imageUploader == null)
            {
                return HttpNotFound();
            }
            return View(imageUploader);
        }

        // POST: ImageUploaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,HttpPostedFileBase file)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.ImageUploaders.Find(id);
            if (TryUpdateModel(studentToUpdate, "", new string[] { "Name", "Email" }))
            {
                try
                {

                    if (file != null && file.ContentLength > 0)
                    {

                       
                                string fileName = Path.GetFileName(file.FileName);
                                string extension = Path.GetExtension(file.FileName);
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                studentToUpdate.Avatar = fileName;
                                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                                file.SaveAs(fileName);
                                studentToUpdate.Email = Request.Form["Email"];
                                studentToUpdate.Name = Request.Form["Name"];
                                //db.Entry(studentToUpdate).State = EntityState.Modified;
                                //db.SaveChanges();
          
                    }
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                
            }
            return View(studentToUpdate);
            //ImageUploader uploads = new ImageUploader();
            //if (ModelState.IsValid)
            //{
            //    if (file != null && file.ContentLength > 0)
            //    {
            //        string ImageName = System.IO.Path.GetFileName(file.FileName);
            //        string path = Server.MapPath("~/Images/" + ImageName);
            //        file.SaveAs(path);

            //        uploads.Avatar = ImageName;
            //        uploads.Email = Request.Form["Email"];
            //        uploads.Name = Request.Form["Name"];
            //        //db.Entry(file).State = EntityState.Modified;

            //    }
            //    db.Entry(uploads).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(uploads);

        }

        // GET: ImageUploaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageUploader imageUploader = db.ImageUploaders.Find(id);
            if (imageUploader == null)
            {
                return HttpNotFound();
            }
            return View(imageUploader);
        }

        // POST: ImageUploaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImageUploader imageUploader = db.ImageUploaders.Find(id);
            db.ImageUploaders.Remove(imageUploader);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
