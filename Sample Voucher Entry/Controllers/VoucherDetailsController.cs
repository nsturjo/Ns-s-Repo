using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sample_Voucher_Entry.EDMX;

namespace Sample_Voucher_Entry.Controllers
{
    public class VoucherDetailsController : Controller
    {
        private Voucher_EntryEntities db = new Voucher_EntryEntities();

        // GET: VoucherDetails
        public ActionResult Index()
        {
            var tblVoucherDetail = db.tblVoucherDetail.Include(t => t.tblAccountsHead).Include(t => t.tblVoucherMaster);
            return View(tblVoucherDetail.ToList());
        }

        // GET: VoucherDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherDetail tblVoucherDetail = db.tblVoucherDetail.Find(id);
            if (tblVoucherDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblVoucherDetail);
        }

        // GET: VoucherDetails/Create
        public ActionResult Create()
        {
            ViewBag.Accounts_Id = new SelectList(db.tblAccountsHead, "Accounts_Id", "Accounts_Name");
            ViewBag.Voucher_Id = new SelectList(db.tblVoucherMaster, "Voucher_Id", "Voucher_No");
            return View();
        }

        // POST: VoucherDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Detail_Id,Voucher_Id,Accounts_Id,Dr_Amount,Cr_Amount")] tblVoucherDetail tblVoucherDetail)
        {
            if (ModelState.IsValid)
            {
                db.tblVoucherDetail.Add(tblVoucherDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Accounts_Id = new SelectList(db.tblAccountsHead, "Accounts_Id", "Accounts_Name", tblVoucherDetail.Accounts_Id);
            ViewBag.Voucher_Id = new SelectList(db.tblVoucherMaster, "Voucher_Id", "Voucher_No", tblVoucherDetail.Voucher_Id);
            return View(tblVoucherDetail);
        }

        // GET: VoucherDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherDetail tblVoucherDetail = db.tblVoucherDetail.Find(id);
            if (tblVoucherDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Accounts_Id = new SelectList(db.tblAccountsHead, "Accounts_Id", "Accounts_Name", tblVoucherDetail.Accounts_Id);
            ViewBag.Voucher_Id = new SelectList(db.tblVoucherMaster, "Voucher_Id", "Voucher_No", tblVoucherDetail.Voucher_Id);
            return View(tblVoucherDetail);
        }

        // POST: VoucherDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Detail_Id,Voucher_Id,Accounts_Id,Dr_Amount,Cr_Amount")] tblVoucherDetail tblVoucherDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblVoucherDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Accounts_Id = new SelectList(db.tblAccountsHead, "Accounts_Id", "Accounts_Name", tblVoucherDetail.Accounts_Id);
            ViewBag.Voucher_Id = new SelectList(db.tblVoucherMaster, "Voucher_Id", "Voucher_No", tblVoucherDetail.Voucher_Id);
            return View(tblVoucherDetail);
        }

        // GET: VoucherDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherDetail tblVoucherDetail = db.tblVoucherDetail.Find(id);
            if (tblVoucherDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblVoucherDetail);
        }

        // POST: VoucherDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblVoucherDetail tblVoucherDetail = db.tblVoucherDetail.Find(id);
            db.tblVoucherDetail.Remove(tblVoucherDetail);
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
