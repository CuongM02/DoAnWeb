using Web_Store.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Model;


namespace Web_Store.Controllers
{
    public class AdminController : Controller
    {
        dbQLGiayDataContext data = new dbQLGiayDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
                return View();
        }
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangnhap(DangnhapModel model)
        {
            if (ModelState.IsValid)
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.TK == model.tendn && n.MK == model.matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            else
            {

            }
            return View(model);
        }
        public ActionResult thongtinadmin()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("dangnhap", "Admin");

            }
            return View();
        }
        public ActionResult dangxuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult listadmin()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("dangnhap", "Admin");

            }
            else
            {
                var ad = from admin in data.Admins select admin;
                return View(ad) ;
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Admin admin, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                if(fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                        return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        data.Admins.InsertOnSubmit(admin);
                        data.SubmitChanges();
                    }

                }                
                return RedirectToAction("listadmin", "Admin");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id )
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var admin = from ad in data.Admins where ad.MaAdmin == id select ad;
                return View(admin.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateInput(false)]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.MaAdmin == id);
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                    fileUpload.SaveAs(path);
                    UpdateModel(ad);
                    data.SubmitChanges();
                    return RedirectToAction("listadmin", "Admin");
                }
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var ad = from adm in data.Admins where adm.MaAdmin == id select adm;
                return View(ad.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.MaAdmin == id);
                data.Admins.DeleteOnSubmit(ad);
                data.SubmitChanges();
                return RedirectToAction("listadmin", "Admin");
            }
        }
        [HttpGet]
        public ActionResult DoiMK(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {

                DoiMKadmin model = new DoiMKadmin();               
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMK(int id, DoiMKadmin reset)
        {
            
            var message = "";
            if (ModelState.IsValid)
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.MaAdmin == id && n.MK == reset.CheckPass); 
                if(ad != null)
                {
                    ad.MK = reset.NewPassword;
                    UpdateModel(ad);
                    Session["Taikhoanadmin"] = ad;
                    data.SubmitChanges();
                    message = "Cập nhật mật khẩu mới thành công ";
                    return RedirectToAction("thongtinadmin", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Mật khẩu cũ không đúng ";
                }
                      
            }
            else
            {
                message = "Điều gì đó không hợp lệ";

            }
            ViewBag.Message = message;
            return View(reset);
        }
    }
}