using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MvcWebUI.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult Seed()
        {
            using (ETicaretContext db = new ETicaretContext())
            {
                db.Kategoriler.RemoveRange();
                db.Urunler.RemoveRange();
                db.Kategoriler.Add(new Kategori()
                {
                    Adi = "Bilgisayar",
                    Aciklamasi = "Bilgisayar parçalarının olduğu kategori",
                    Guid = Guid.NewGuid().ToString(),
                    Urunler = new List<Urun>()
                    {
                        new Urun()
                        {
                            Adi="ASUS LAPTOP",
                            Aciklamasi="Laptop",
                            BirimFiyati = 20,
                            SonKullanmaTarihi = new DateTime(2025,1,22),
                            StokMiktari=120,
                            Guid = Guid.NewGuid().ToString(),
                        },
                        new Urun()
                        {
                            Adi="MONSTER LAPTOP",
                            Aciklamasi="Laptop",
                            BirimFiyati = 215,
                            SonKullanmaTarihi = new DateTime(2026,1,22),
                            StokMiktari=12,
                            Guid = Guid.NewGuid().ToString(),
                        },
                        new Urun()
                        {
                            Adi="GTX3080",
                            Aciklamasi="Ekran Kartı",
                            BirimFiyati = 2500,
                            SonKullanmaTarihi = new DateTime(2029,1,22),
                            StokMiktari=112,
                            Guid = Guid.NewGuid().ToString(),
                        }
                    }
                });
                db.SaveChanges();
            }
            return Content("<label style=\"color:red;\"><b>İlk veriler oluşturuldu.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
