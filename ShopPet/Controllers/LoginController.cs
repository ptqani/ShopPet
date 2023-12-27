using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopPet.Models;

namespace ShopPet.Controllers
{
    public class LoginController : Controller
    {
        private ShopPetEntities db = new ShopPetEntities();

        #region đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var dbContext = new ShopPetEntities();
            var tendangnhap = collection["tentaikhoan"];
            var matkhau = collection["matkhau"];


            KhachHang kiemtraDangNhap =
                dbContext.KhachHangs.FirstOrDefault(n => n.tendangnhap == tendangnhap 
                                                            && n.matkhau == matkhau);

            if (kiemtraDangNhap != null) //Dang nhap thang cong
            {
                Session["TaiKhoan"] = tendangnhap;
                Session["KTTaiKhoan"] = kiemtraDangNhap;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["DangNhapThatBai"] = "Tên tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }

            
        }

        public ActionResult DangXuat()
        {
            Session.Remove("TaiKhoan");
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region đăng ký
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var dbContext = new ShopPetEntities();

            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = "01/02/2000";
            

            var dblist = dbContext.KhachHangs.ToList();

            foreach (var item in dblist)
            {
                if (item.tendangnhap == tendangnhap)
                {
                    TempData["taikhoanTonTai"] = "Tên tài khoản đã tồn tại";
                    continue;
                }

                else
                {
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);
                    kh.RoleID = 2;
                    kh.status = 1;

                    dbContext.KhachHangs.Add(kh);
                    dbContext.SaveChanges();

                    TempData["dangKyThanhCong"] = "Bạn đẫ đăng ký thành công";
                    return RedirectToAction("Login");
                }
            }

            return this.DangKy();
        }


        #endregion
    }
}
