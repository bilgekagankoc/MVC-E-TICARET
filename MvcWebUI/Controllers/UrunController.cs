using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.Controllers
{
    public class UrunController : Controller
    {
        private readonly IUrunService _urunService;
        private readonly IKategoriService _kategoriService;

        public UrunController(IUrunService urunService, IKategoriService kategoriService)
        {
            _urunService = urunService;
            _kategoriService = kategoriService;
        }

        public IActionResult Index()
        {
            var result = _urunService.Query().ToList();
            return View(result);
        }

        public IActionResult AddView()
        {
            List<KategoriModel> kategoriList = _kategoriService.Query().ToList();
            ViewBag.KategoriId = new SelectList(kategoriList, "Id", "Adi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UrunModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _urunService.Add(model);
                if (result.IsSuccessful)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.KategoriId = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", model.KategoriId);
            return View("AddView", model);
        }
        public IActionResult EditView(int Id)
        {
            var urun = _urunService.Query().SingleOrDefault(x => x.Id == Id);
            if (urun == null)
            {
                TempData["Success"] = "Ürün Bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.KategoriId = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                return View(urun);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UrunModel urun)
        {
            var result = _urunService.Update(urun);
            if (result.IsSuccessful)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.KategoriId = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                ModelState.AddModelError("", result.Message);
                return View(urun);
            }
        }

        public IActionResult Detail(int id)
        {
            var result = _urunService.Query().SingleOrDefault(x => x.Id == id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                TempData["Success"] = "Ürün Bulunamadı";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
