using System;
using ShopPet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Text.RegularExpressions;

namespace ShopPet.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: Student
        ShopPetEntities data = new ShopPetEntities();

        [HttpGet]
        public ActionResult TimKiem(int? page, string search)
        {

            if (search == null || search == "" || search == " ")
            {
                TempData["thongbaodetrrong"] = "Không được để trống khi tìm kiếm";
            }

            if (search.Length > 30)
            {
                TempData["thongbaododai"] = "Độ dài vượt quá 30 kí tự";
            }

            if (page == null) page = 1;

            var lstSP = data.SanPhams.Where(n => n.tensp.Contains(search));
            lstSP = lstSP.OrderBy(n => n.tensp);

            int pageSize = 5;
            int pageNum = page ?? 1;


            ViewBag.Search = search;

            return View(lstSP.ToPagedList(pageNum, pageSize));
        }

    }
}