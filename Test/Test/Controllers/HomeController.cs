using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using Test.ADO;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string search)
        {
            var data = Database.Dataprovider.Instance.ExecuteQuery("select * from chitiet_nhapxuat where ma_vt like %@search% or ten_vt like %@search% @search",
            new object[] { new object[] { search} });
            List<Vattu> datList = new();
            foreach (DataRow item in data.Rows)
            {
                datList.Add(new Vattu()
                {
                    so_phieu= item["so_phieu"].ToString(),
                    ngay_lap_phieu = item["ngay_lap_phieu"].ToString(),
                    ten_vt = item["ten_vt"].ToString(),
                });
            }
            
            return View(datList);
        }

        [HttpPost]
        public JsonResult Add(Vattu vattu)
        {
            int result = 0;
            DataResult res = new DataResult();
            try
            {
                result = Database.Dataprovider.Instance.ExecuteNonQuery("add_ct_nx @pso_phieu,@pngay_lap_phieu,@pma_vt,@pten_vt,@pdvt,@psl_nhap",
                new object[] { new object[] { vattu.so_phieu,vattu.ngay_lap_phieu,vattu.ma_vt,vattu.ten_vt,vattu.dvt,vattu.sl_nhap} });

                if(result > 0)
                {
                    res.msg = "thanh cong";
                }
                else
                {
                    res.msg = "that bai";
                }
            }
            catch
            {
                res.msg = "that bai";
            }
            res.status = result > 0;
            return new JsonResult(res); 
        }

        [HttpPost]
        public JsonResult Delete(string mavt, string sophieu)
        {
            int result = 0;
            DataResult res = new DataResult();
            try
            {
                result = Database.Dataprovider.Instance.ExecuteNonQuery("DELETE FROM chitiet_nhapxuat WHERE ma_vt = @mavt and so_phieu = @sophieu @mavt, @sophieu",
                new object[] { new object[] { mavt, sophieu } });

                if (result > 0)
                {
                    res.msg = "thanh cong";
                }
                else
                {
                    res.msg = "that bai";
                }
            }
            catch
            {
                res.msg = "that bai";
            }
            res.status = result > 0;
            return new JsonResult(res);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}