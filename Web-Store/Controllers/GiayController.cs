using PagedList;
using Web_Store.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Model;

namespace Web_Store.Controllers
{
    public class GiayController : Controller
    {
        dbQLGiayDataContext data = new dbQLGiayDataContext();
        // GET: Sach
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {

                if (page == null) page = 1;
                int pageSize = 9;
                int pageNumber = (page ?? 1);
                var giay = from g in data.Giays select g;
                return View(giay.ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var giay = from g in data.Giays where g.MaGiay == id select g;
                if (giay == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(giay.Single());
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MATH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MATH", "TENTH");
                
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Giay giay, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MATH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MATH", "TENTH");
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    giay.HinhAnh = fileName;
                    giay.SoLuongTon = 0;
                    data.Giays.InsertOnSubmit(giay);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Giay");
                }

            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var giay = from g in data.Giays where g.MaGiay == id select g;
                ViewBag.MATH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MATH", "TENTH");
                return View(giay.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                Giay giay = data.Giays.SingleOrDefault(g => g.MaGiay == id);
                ViewBag.MATH = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MATH", "TENTH");
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
                    giay.HinhAnh = fileName;
                    UpdateModel(giay);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Giay");
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
                var giay = from g in data.Giays where g.MaGiay == id select g;
                return View(giay.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                Giay giay = data.Giays.SingleOrDefault(g => g.MaGiay == id);
                var kichthuoc = from KICHTHUOC in data.KICHTHUOCs where KICHTHUOC.MAGIAY == id select KICHTHUOC;
                var dathang = from CT_DONDATHANG in data.CT_DonDatHangs where CT_DONDATHANG.MaGIay == id select CT_DONDATHANG;
                var dondathang = from DonDatHang in data.DonDatHangs select DonDatHang;
                foreach (var item in dathang)
                {
                    data.CT_DonDatHangs.DeleteOnSubmit(item);
                }             
                foreach (var item in kichthuoc)
                {
                    data.KICHTHUOCs.DeleteOnSubmit(item);
                }
                data.Giays.DeleteOnSubmit(giay);
                data.SubmitChanges();
                return RedirectToAction("Index", "Giay");
            }
        }
    }
}