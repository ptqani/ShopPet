using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopPet.Models;
using PagedList;
using System.IO;

namespace ShopPet.Controllers
{
    public class AdminController : Controller
    {
        ShopPetEntities data = new ShopPetEntities();

        //trang chu admin 
        //trang nay lay layout nao layout kia la tao lay tren mang
        public ActionResult Index()
        {
            return View();
        }

        //Quan ly phan quyen
        public ActionResult ChiaQuyen()
        {
            List<KhachHang> khachhang = data.KhachHangs.ToList();
            List<KhachHangRole> khachhangrole = data.KhachHangRoles.ToList();

            var main = from kh in khachhang // link quueu query
                       join r in khachhangrole
                       on kh.RoleID equals r.RoleID
                       where (kh.RoleID == r.RoleID)
                       select new ViewModel
                       {
                           khachhang = kh,
                           khachhangrole = r
                       };
            ViewBag.Main = main;
            return View();
        }


        public ActionResult SuaQuyen(int id)
        {
            var editRole = data.KhachHangs.First(m => m.makh == id);
            return View(editRole);
        }
        [HttpPost]
        public ActionResult SuaQuyen(int id, FormCollection collection) // ado.net
        {
            var danhmuc = data.KhachHangs.First(m => m.makh == id);
            var E_tendanhmuc = collection["RoleID"];
            var status = collection["status"];
            if (string.IsNullOrEmpty(E_tendanhmuc))
            {
                ViewData["Error"] = "Du lieu khong duoc de trong!";
            }
            else
            {
                danhmuc.RoleID = Convert.ToInt32(E_tendanhmuc);
                danhmuc.status = Convert.ToInt32(status);

                if (danhmuc.status == 1 || danhmuc.status == 2)
                {
                    UpdateModel(danhmuc);
                    data.SaveChanges();
                    return RedirectToAction("ChiaQuyen");
                }
                else
                {
                    return RedirectToAction("ChiaQuyen");
                }



            }
            return this.SuaQuyen(id);
        }


        public ActionResult XoaQuyen(int id)
        {
            var D_sach = data.KhachHangs.First(m => m.makh == id);
            return View(D_sach);
        }

        [HttpPost]
        public ActionResult XoaQuyen(int id, FormCollection collection)
        {
            try
            {
                var kh = data.KhachHangs.Where(m => m.makh == id).First();
                data.KhachHangs.Remove(kh);
                data.SaveChanges();
                return RedirectToAction("ChiaQuyen");
            }
            catch (Exception e) { return View("Error" + e); }

        }
        // them san pham
        #region
        public ActionResult QLSanPham(int? page)
        {
            if (page == null) page = 1;
            var all_sach = (from s in data.SanPhams select s).OrderBy(m => m.masp); // linq 
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int id)
        {
            var D_SanPham = data.SanPhams.Where(m => m.masp == id).First();
            return View(D_SanPham);
        }

        public ActionResult Create()
        {
            var getlist = data.DanhMucs.ToList();
            SelectList list = new SelectList(getlist, "idDanhmuc", "tendanhmuc");
            ViewBag.fulllist = list;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, SanPham s)
        {
            var E_tensp = collection["tensp"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giamgia = Convert.ToInt32(collection["giamgia"]);
            var E_ngaycapnhat = DateTime.Now;
            var E_iddanhmuc = collection["idDanhmuc"];
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_mota = collection["mota"];
            if (string.IsNullOrEmpty(E_tensp))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tensp = E_tensp.ToString();
                s.hinh = E_hinh;
                s.giaban = E_giaban;
                s.giamgia = E_giamgia;

                s.idDanhmuc = Convert.ToInt32(E_iddanhmuc);
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                s.mota = E_mota;

                var x = s.giaban;
                var y = s.giamgia;

                var z = (x * y) / 100;

                var price = x - z;

                s.giakhuyenmai = price;

                data.SanPhams.Add(s); //adodotnet
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var getlist = data.DanhMucs.ToList();
            SelectList list = new SelectList(getlist, "idDanhmuc", "tendanhmuc");
            ViewBag.fulllist = list;

            var E_sach = data.SanPhams.First(m => m.masp == id);
            return View(E_sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_sach = data.SanPhams.First(m => m.masp == id);
            var E_tensach = collection["tensp"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_giamgia = collection["giamgia"];
            var E_ngaycapnhat = DateTime.Now;
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_mota = collection["mota"];
            E_sach.masp = id;
            if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_sach.tensp = E_tensach;
                E_sach.hinh = E_hinh;

                E_sach.giaban = E_giaban;
                E_sach.giamgia = Convert.ToInt32(E_giamgia);
                E_sach.ngaycapnhat = E_ngaycapnhat;
                E_sach.soluongton = E_soluongton;
                E_sach.mota = E_mota;

                var x = E_sach.giaban;
                var y = E_sach.giamgia;

                var z = (x * y) / 100;

                var price = x - z;

                E_sach.giakhuyenmai = price;

                UpdateModel(E_sach);
                data.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        //--Lay duong dan hinh anh khi sua
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/uploads/" + file.FileName));
            return "/Content/uploads/" + file.FileName;
        }

        public ActionResult Delete(int id)
        {
            var D_sach = data.SanPhams.First(m => m.masp == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = data.SanPhams.Where(m => m.masp == id).First();
            data.SanPhams.Remove(D_sach);
            data.SaveChanges();
            return RedirectToAction("Index");
        }
#endregion
    }
}