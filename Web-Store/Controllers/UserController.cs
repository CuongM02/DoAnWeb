using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Web_Store.Model;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Web_Store.Controllers

{

    public class UserController : Controller
    {
        dbQLGiayDataContext db = new dbQLGiayDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ Tên Khách Hàng Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên Đăng Nhập Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật Khẩu Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Mật Khẩu Nhập lại Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "SĐT Không Được Để Trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TK = tendn;
                kh.MK = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            else
            {
                var tk = db.KhachHangs.SingleOrDefault(n => n.TK == tendn && n.MK == matkhau);
                if (tk != null)
                {
                    ViewBag.Thongbao = "Đăng Nhập Thành Công";
                    Session["Taikhoan"] = tk;
                    return RedirectToAction("Index", "User");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            return View();
        }
        private List<Giay> layGiay(int count)
        {
            return db.Giays.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult IfCustomer()
        {
            KhachHang kh = (KhachHang)Session["TenDN"];
            return View();
        }
        public ActionResult SuaTTKH()
        {
            KhachHang kh = (KhachHang)Session["TenDN"];
            KhachHang kh1 = new KhachHang();
            kh1.MaKH = kh.MaKH;
            kh1.HoTen = kh.HoTen;
            kh1.SDT = kh.SDT;
            kh1.DiaChi = kh.DiaChi;
            kh1.NgaySinh = kh.NgaySinh;
            return View(kh1);
        }

        [HttpPost]
        public ActionResult SuaTTKH(KhachHang kh1)
        {
            if (ModelState.IsValid)
            {
                UpdateModel(kh1);
                db.SubmitChanges();
            }
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("index", "User");
            else
            {
                var thongtin = from tt in db.KhachHangs where tt.MaKH == id select tt;
                return View(thongtin.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateInput(false)]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("index", "User");
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
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
                    Session["Taikhoan"] = kh;
                    UpdateModel(kh);
                    db.SubmitChanges();
                    return RedirectToAction("thongtincanhan", "User");
                }
            }
        }


        public ActionResult Sanpham(int? page)
        {
            var newclock = layGiay(1000);
            int pagesize = 9;
            int pageNum = (page ?? 1);
            return View(newclock.ToPagedList(pageNum, pagesize));
        }
        public ActionResult thongtincanhan()
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("index", "User");

            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("index", "User");
        }
        public ActionResult ThuongHieu()
        {
            var ths = from th in db.ThuongHieus select th;
            return PartialView(ths);
        }
        public ActionResult GiayTheoThuongHieu(int id)
        {
            var giays = from giay in db.Giays where giay.MaTH == id select giay;
            return View(giays);
        }
        public ActionResult Chitietgiay(int id)
        {
            var giays = from Giay in db.Giays
                        where Giay.MaGiay == id
                        select Giay;
            return View(giays.Single());
        }
        
    }
}  