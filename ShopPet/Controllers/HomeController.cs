using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopPet.Models;
using PagedList.Mvc;
using PagedList;

namespace ShopPet.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? page) // trang 3
        {
            var dbContext = new ShopPetEntities();
    
            if (page == null) page = 1;
            int pageSize = 8; //so luong sp 1 trang
            int pageNum = page ?? 1; //tra ve trang 3, neu null thi ve trang 1

            var sp = dbContext.SanPhams.OrderByDescending(x => x.masp) //linq cú pháp phương thức methodd 
                      .ToList();

            List<SanPham> sanpham = dbContext.SanPhams.ToList();
            var spnoibat  = from sp1 in sanpham
                                      select new ViewModel
                                      {
                                          sanpham = sp1
                                      };

            ViewBag.PhanTrangSP = spnoibat;


            var danhmucsp = dbContext.DanhMucs.ToList();

            ViewBag.DanhMuc = danhmucsp.ToList();


            return View(sp.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ProductDanhmuc(int id, int? page)
        {
            var dbContext = new ShopPetEntities();

            if (page == null) page = 1;
            int pageSize = 8; //so luong sp 1 trang
            int pageNum = page ?? 1; //tra ve trang 3, neu null thi ve trang 1

            var products = dbContext.SanPhams.Where(sp => sp.idDanhmuc == id).ToList();
            return View(products.ToPagedList(pageNum, pageSize));
        }
    }
}