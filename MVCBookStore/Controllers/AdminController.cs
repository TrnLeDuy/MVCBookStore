using MVCBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

namespace MVCBookStore.Controllers
{
    public class AdminController : Controller
    {
        // Use DbContext to manage database
        bookstoreDBEntities database = new bookstoreDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            return View();
        }

        //GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.UserAdmin))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
            if (string.IsNullOrEmpty(admin.PassAdmin))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                    //Kiểm tra có admin này hay chưa
                    var adminDB = database.ADMINs.FirstOrDefault(ad => ad.UserAdmin ==
                    admin.UserAdmin && ad.PassAdmin == admin.PassAdmin);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    Session["Admin"] = adminDB;
                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        // Admin Logout
        public ActionResult Logout()
        {   
            //Perform any necessary cleanup or logging out of the user
            //Remove any authentication cookies or session state information
            //Redirect the user to the login page
            Session["Admin"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Sach(int? page)
        {
            var dsSach = database.SACHes.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 7;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsSach.OrderBy(sach => sach.Masach).ToPagedList(pageNum,
            pageSize));
        }

        [HttpPost]
        public ActionResult ThemSach (SACH sach, HttpPostedFileBase Hinhminhhoa)
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");

            if(Hinhminhhoa == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);

                    //Kiểm tra hình đã tồn tại trong hệ thống chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình đã tồn tại";
                    }
                    else
                    {
                        Hinhminhhoa.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    database.SACHes.Add(sach);
                    database.SaveChanges();
                }
            }
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult ThemSach()
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            return View();
        }

        public ActionResult ChiTietSach(int id)
        {
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
    }
}