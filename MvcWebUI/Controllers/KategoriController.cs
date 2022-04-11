using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    public class KategoriController : Controller
    {
        private readonly IKategoriService _kategoriService;

        public KategoriController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public IActionResult Index()
        {
            var result = _kategoriService.Query().ToList();
            return View(result);
        }

        public IActionResult AddView()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Add(KategoriModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _kategoriService.Add(model);
                if (result.IsSuccessful)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View("AddView");
                }
            }
            else
            {
                return View("AddView");
            }
        }

        public IActionResult Detail(int id)
        {
            var result = _kategoriService.Query().SingleOrDefault(x => x.Id == id);
            if (result == null)
            {
                TempData["Success"] = "Kategori Bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        public IActionResult EditView(int id)
        {
            var result = _kategoriService.Query().SingleOrDefault(x => x.Id == id);
            if (result == null)
            {
                TempData["Success"] = "Kategori Bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        public IActionResult Edit(KategoriModel kategori)
        {
            if (kategori == null)
            {
                TempData["Success"] = "Kategori Bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var result = _kategoriService.Update(kategori);
                if (result.IsSuccessful)
                {
                    TempData["Success"] = "Kategori Düzeltme İşlemi Başarıyla Yapıldı";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["Success"] = "Hata Düzeltme Yapılamadı";
                    return RedirectToAction(nameof(Index));

                }
            }
        }

        public IActionResult Delete(int id)
        {
            var result = _kategoriService.Query().SingleOrDefault(x => x.Id == id);
            if(result == null)
            {
                TempData["Success"] = "Ürün Bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            else if (result.UrunSayisiDisplay > 0)
            {
                TempData["Success"] = "Kategoriye bağlı ürünler olduğu için silme işlemi yapılmadı";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _kategoriService.Delete(id);
                TempData["Success"] = "Ürün Silindi.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
