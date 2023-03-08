using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;

namespace MVCBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // Use DbContext to manage database
        bookstoreDBEntities database = new bookstoreDBEntities();
        private List<SACH> LaySachMoi(int soluong)
        {
            // Sắp xếp sách theo ngày cập nhật giảm dần, lấy đúng số lượng sách cần
            // Chuyển qua dạng danh sách kết quả đạt được
            return database.SACHes.OrderByDescending(sach =>
           sach.Ngaycapnhat).Take(soluong).ToList();
        }
        // GET: BookStore
        public ActionResult Index()
        {
            // Giả sử cần lấy 5 quyển sách mới cập nhật
            var dsSachMoi = LaySachMoi(5);
            return View(dsSachMoi);
        }

        public ActionResult LayChuDe()
        {
            var dsChuDe = database.CHUDEs.ToList();
            return PartialView(dsChuDe);
        }

        public ActionResult LayNhaXuatBan()
        {
            var dsNhaXB = database.NHAXUATBANs.ToList();
            return PartialView(dsNhaXB);
        }

        public ActionResult SPTheoChuDe(int id)
        {
            //Lấy các sách theo mã chủ đề được chọn
            var dsSachTheoChuDe = database.SACHes.Where(sach => sach.MaCD == id).ToList();

            //Trả về View để render các sách trên
            //(tái sử dụng View Index ở trên , truyền vào danh sách)
            return View("Index", dsSachTheoChuDe);
        }
        public ActionResult SPTheoNXB (int id)
        {
            //Lấy các sách theo mã chủ đề được chọn
            var dsSachNXB = database.SACHes.Where(sach => sach.MaNXB == id).ToList();

            //Trả về View để render các sách trên
            //(tái sử dụng View Index ở trên, truyền vào danh sách)
            return View("Index", dsSachNXB);
        }

        public ActionResult Details(int id)
        {
            //Lấy mã sách có mã tương ứng
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);
            return View(sach);
        }
    }
}