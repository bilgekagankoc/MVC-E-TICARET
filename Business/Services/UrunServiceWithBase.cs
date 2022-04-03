using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUrunService : IService<UrunModel,Urun,ETicaretContext>
    {

    }

    public class UrunService : IUrunService
    {
        public RepoBase<Urun, ETicaretContext> Repo { get; set; } = new Repo<Urun, ETicaretContext>();

        public IQueryable<UrunModel> Query()
        {
            var result = Repo.Query("Kategori").OrderBy(x => x.Kategori.Adi).ThenBy(x => x.Adi).Select(x => new UrunModel()
            {
                Id = x.Id,
                Adi = x.Adi,
                Aciklamasi = x.Aciklamasi,
                BirimFiyati = x.BirimFiyati,
                SonKullanmaTarihi = x.SonKullanmaTarihi,
                StokMiktari = x.StokMiktari,
                KategoriId = x.KategoriId,
                KategoriAdiDisplay = x.Kategori.Adi,
                BirimFiyatiDisplay = x.BirimFiyati.ToString("C2"),
                SonKullanmaTarihiDisplay = x.SonKullanmaTarihi.HasValue ? x.SonKullanmaTarihi.Value.ToString("yyyy-MM-dd") : ""
            });
            return result;
        }

        public Result Add(UrunModel model)
        {
            if(Repo.Query().Any(x=>x.Adi.ToUpper().Trim() == model.Adi.ToUpper().Trim()))
                return new ErrorResult("Girdiğiniz ürün adı bulunmaktadır");
            Urun entity = new Urun()
            {
                Adi = model.Adi,
                Aciklamasi = model.Aciklamasi,
                BirimFiyati = model.BirimFiyati.Value,
                StokMiktari = model.StokMiktari.Value,
                KategoriId = model.KategoriId.Value,
                SonKullanmaTarihi = model.SonKullanmaTarihi,
                Guid = Guid.NewGuid().ToString()
            };
            Repo.Add(entity);
            return new SuccessResult("Ürün başarıyla eklendi.");
        }

        public Result Update(UrunModel model)
        {
            if (Repo.Query().Any(x => x.Adi.ToUpper().Trim() == model.Adi.ToUpper().Trim() && x.Id != model.Id))
                return new ErrorResult("Girdiğiniz isimde ürün adı bulunmaktadır.");
            Urun entity = Repo.Query().SingleOrDefault(x => x.Id == model.Id);
            entity.KategoriId = model.KategoriId.Value;
            entity.Adi = model.Adi.Trim();
            entity.Aciklamasi = model.Aciklamasi?.Trim();
            entity.BirimFiyati = model.BirimFiyati.Value;
            entity.StokMiktari = model.StokMiktari.Value;
            entity.SonKullanmaTarihi = model.SonKullanmaTarihi;
            entity.KategoriId = model.KategoriId.Value;
            Repo.Update(entity);
            return new SuccessResult("Ürün başarıyla güncellendi.");
        }

        public Result Delete(int id)
        {
            Repo.Delete(x => x.Id == id);
            return new SuccessResult("Başarıyla Silindi");

        }

        public void Dispose()
        {
            Repo.Dispose();
        }
    }
}
