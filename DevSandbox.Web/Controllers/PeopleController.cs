using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevSandbox.Web.Models;

namespace DevSandbox.Web.Controllers
{
    public class PeopleController : Controller
    {
        // GET: People
        public ActionResult Index()
        {
            var list = new List<Person>()
            {
                new Person { Name = "Orianthi", CreatedDate = DateTime.Today.AddYears(-3)},
                new Person { Name = "Pat Benatar", BirthDate = new DateTime(1965,10,12), NumberOfKids =2, CreatedDate = DateTime.Today.AddMonths(-36) },
                new Person { Name = "Janis Joplin", BirthDate = new DateTime(1960, 2, 15), CreatedDate = DateTime.Today.AddMonths(-1) }
            };

            return View(list);
        }

        // GET: People/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: People/Create
        public ActionResult Create()
        {
            var model = new Person { CreatedDate = DateTime.Today };
            return View(model);
        }

        // POST: People/Create
        [HttpPost]
        public ActionResult Create(Person person)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: People/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: People/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: People/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: People/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
