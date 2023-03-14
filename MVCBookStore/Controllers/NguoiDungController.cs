using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;

namespace MVCBookStore.Controllers
{
    public class NguoiDungController : Controller
    {
        //Use DBContext to manage database
        bookstoreDBEntities database = new bookstoreDBEntities();

        /*ACTION ĐĂNG KÝ*/
        /*----------------------------------------------*/
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTenKH))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra xem có người nào đã đăng ký với tên đăng nhập này hay chưa
                var khachhang = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng ký tên này");

                if (ModelState.IsValid)
                {
                    database.KHACHHANGs.Add(kh);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }
        /*----------------------------------------------*/
        /*ACTION ĐĂNG NHẬP*/
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tìm khác hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var khach = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN && k.Matkhau == kh.Matkhau);
                    if (khach != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        //Lưu vào session
                        Session["TaiKhoan"] = khach;
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}