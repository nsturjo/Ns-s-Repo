using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sample_Voucher_Entry.EDMX;
using Sample_Voucher_Entry.Models;
using Newtonsoft.Json;

namespace Sample_Voucher_Entry.Controllers
{
    public class AddVoucherController : Controller
    {
        private Voucher_EntryEntities db = new Voucher_EntryEntities();

        // GET: AddVoucher
        public ActionResult Index()
        {
            var tblVoucherMaster = db.tblVoucherMaster.Include(t => t.tblSupplier);
            return View(tblVoucherMaster.ToList());
        }

        // GET: AddVoucher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherMaster tblVoucherMaster = db.tblVoucherMaster.Find(id);
            if (tblVoucherMaster == null)
            {
                return HttpNotFound();
            }
            return View(tblVoucherMaster);
        }

        // GET: AddVoucher/Create
        public ActionResult Create()
        {
            ViewBag.Supplier_Id = new SelectList(db.tblSupplier, "Supplier_Id", "Supplier_Name");


            return View();
        }

        // POST: AddVoucher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Voucher_Id,Voucher_No,Voucher_Date,Supplier_Id")] tblVoucherMaster tblVoucherMaster, string[] DynamicTextBox)
        {
            if (ModelState.IsValid)
            {
                db.tblVoucherMaster.Add(tblVoucherMaster);

                //List<tblVoucherDetail> voucherDetailsList = new List<tblVoucherDetail>();


                var rowCount = 0;

                var itemCount = DynamicTextBox.Count();

                if (itemCount > 0)
                {
                    rowCount = itemCount / 3;

                    int j = 0;

                    for (int i = 1; i <= rowCount; i++)
                    {
                        tblVoucherDetail voucherDetails = new tblVoucherDetail();

                        string account_name = DynamicTextBox[j];
                        int account_id = db.tblAccountsHead.Where(a => a.Accounts_Name == account_name).Select(a => a.Accounts_Id).FirstOrDefault();

                        voucherDetails.Accounts_Id = account_id;
                        voucherDetails.Dr_Amount = Convert.ToDouble(DynamicTextBox[j + 1]);
                        voucherDetails.Cr_Amount = Convert.ToDouble(DynamicTextBox[j + 2]);
                        //voucherDetailsList.Add(voucherDetails);
                        db.tblVoucherDetail.Add(voucherDetails);

                        j = j + 3;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Supplier_Id = new SelectList(db.tblSupplier, "Supplier_Id", "Supplier_Name", tblVoucherMaster.Supplier_Id);
            return View(tblVoucherMaster);
        }

        // GET: AddVoucher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherMaster tblVoucherMaster = db.tblVoucherMaster.Find(id);

            //db.tblVoucherDetail.Where(a => a.Voucher_Id == tblVoucherMaster.Voucher_Id).SelectMany(a=>a.Accounts_Id,a=>a.); 
            var voucherDetailList = db.tblVoucherDetail.Where(a => a.Voucher_Id == tblVoucherMaster.Voucher_Id).Select(i => new { i.Detail_Id, i.Accounts_Id, i.Dr_Amount, i.Cr_Amount }).Distinct().ToArray();

            int voucherNumber = voucherDetailList.Count();


            string[] textBoxValues = new string[voucherNumber * 4];

            int x = 0;

            foreach (var item in voucherDetailList)
            {
                int account_id = item.Accounts_Id;
                string account_name = db.tblAccountsHead.Where(a => a.Accounts_Id == account_id).Select(a => a.Accounts_Name).FirstOrDefault();

                textBoxValues[x] = item.Detail_Id.ToString();
                textBoxValues[x + 1] = account_name;
                textBoxValues[x + 2] = item.Dr_Amount.ToString();
                textBoxValues[x + 3] = item.Cr_Amount.ToString();
                x = x + 4;
            };

            ViewBag.VoucherItemNo = voucherNumber;
            ViewBag.TextBoxData = JsonConvert.SerializeObject(textBoxValues);

            if (tblVoucherMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.Supplier_Id = new SelectList(db.tblSupplier, "Supplier_Id", "Supplier_Name", tblVoucherMaster.Supplier_Id);
            return View(tblVoucherMaster);
        }

        // POST: AddVoucher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Voucher_Id,Voucher_No,Voucher_Date,Supplier_Id")] tblVoucherMaster tblVoucherMaster, string[] DynamicTextBox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblVoucherMaster).State = EntityState.Modified;

                #region voucher detail table update

                var rowCount = 0;

                var itemCount = DynamicTextBox.Count();

                if (itemCount > 0)
                {
                    rowCount = itemCount / 4;

                    int j = 0;

                    List<int> totalItemFromUI = new List<int>();

                    for (int i = 0; i < itemCount; i = i + 4)
                    {
                        if (DynamicTextBox[i] != "0")
                        {
                            totalItemFromUI.Add(Convert.ToInt16(DynamicTextBox[i]));
                        }
                    }
                    var totalItemInDetailForAVoucherId = db.tblVoucherDetail.Where(a => a.Voucher_Id == tblVoucherMaster.Voucher_Id).Select(a => a.Detail_Id).ToList();

                    var deletedIDs = totalItemInDetailForAVoucherId.Except(totalItemFromUI);

                    if (deletedIDs != null)
                    {
                        foreach (var item in deletedIDs)
                        {
                            var details = db.tblVoucherDetail.Where(a => a.Detail_Id == item).SingleOrDefault();
                            db.tblVoucherDetail.Remove(details);
                        }
                        db.SaveChanges();
                    }


                    for (int i = 1; i <= rowCount; i++)
                    {
                        tblVoucherDetail voucherDetails = new tblVoucherDetail();

                        int detailID = Convert.ToInt16(DynamicTextBox[j]);
                        string account_name = DynamicTextBox[j + 1];
                        int account_id = db.tblAccountsHead.Where(a => a.Accounts_Name == account_name).Select(a => a.Accounts_Id).FirstOrDefault();
                        double dr_amount = Convert.ToDouble(DynamicTextBox[j + 2]);
                        double cr_amount = Convert.ToDouble(DynamicTextBox[j + 3]);

                        if (detailID != 0)
                        {
                            var details = db.tblVoucherDetail.Where(a => a.Detail_Id == detailID).SingleOrDefault();

                            details.Accounts_Id = account_id;
                            details.Dr_Amount = dr_amount;
                            details.Cr_Amount = cr_amount;

                        }
                        else
                        {
                            voucherDetails.Accounts_Id = account_id;
                            voucherDetails.Dr_Amount = dr_amount;
                            voucherDetails.Cr_Amount = cr_amount;
                            voucherDetails.Voucher_Id = tblVoucherMaster.Voucher_Id;

                            db.tblVoucherDetail.Add(voucherDetails);
                        }


                        j = j + 4;
                    }
                }

                #endregion

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Supplier_Id = new SelectList(db.tblSupplier, "Supplier_Id", "Supplier_Name", tblVoucherMaster.Supplier_Id);
            return View(tblVoucherMaster);
        }

        // GET: AddVoucher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVoucherMaster tblVoucherMaster = db.tblVoucherMaster.Find(id);
            if (tblVoucherMaster == null)
            {
                return HttpNotFound();
            }
            return View(tblVoucherMaster);
        }

        // POST: AddVoucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblVoucherMaster tblVoucherMaster = db.tblVoucherMaster.Find(id);
            db.tblVoucherMaster.Remove(tblVoucherMaster);
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

        public JsonResult GetVisitCustomer(string keyword)
        {
            //var objCustomerlist = db.tblAccountsHead
            //                .Select(c => new { Name = c.Accounts_Name, ID = c.Accounts_Id })
            //                .Where(c => c.Name.Contains)
            //                .Distinct().ToList();
            var objCustomerlist = from head in db.tblAccountsHead where head.Accounts_Name.Contains(keyword) select new { Name = head.Accounts_Name, ID = head.Accounts_Id };

            return Json(objCustomerlist, JsonRequestBehavior.AllowGet);
        }
    }
}
