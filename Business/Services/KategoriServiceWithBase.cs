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
    public interface IKategoriService : IService<KategoriModel,Kategori,ETicaretContext>
    {

    }

    public class KategoriService : IKategoriService
    {
        public RepoBase<Kategori, ETicaretContext> Repo { get; set; } = new Repo<Kategori, ETicaretContext>();

        public Result Add(KategoriModel model)
        {
            if(Repo.Query().Any(x => x.Adi.ToUpper().Trim() == model.Adi.ToUpper().Trim()))
            {
                return new ErrorResult("Bu İsimde Kategori Bulunmaktadır.");
            }
            Kategori kategori = new Kategori()
            {
                Aciklamasi = model.Aciklamasi,
                Adi = model.Adi,
                Guid = Guid.NewGuid().ToString(),
            };
            Repo.Add(kategori);
            return new SuccessResult("Ürün Başarıyla Kaydedildi");
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

        public IQueryable<KategoriModel> Query()
        {
            var result = Repo.Query("Urunler").OrderBy(kategori => kategori.Adi).Select(kategori => new KategoriModel()
            {
                Id = kategori.Id,
                Adi = kategori.Adi,
                Aciklamasi = kategori.Aciklamasi,
                UrunSayisiDisplay = kategori.Urunler.Count
            });
            return result;
        }

        public Result Update(KategoriModel model)
        {
            var result = Repo.Query().Any(x => x.Adi.ToUpper().Trim() == model.Adi.ToUpper().Trim());
            if(Repo.Query().Any(x => x.Adi.ToUpper().Trim() == model.Adi.ToUpper().Trim()))
            {
                return new ErrorResult("Girdiğiniz isimde ürün adı bulunmaktadır.");
            }
            else
            {
                Kategori kategoriEntity = Repo.Query().SingleOrDefault(x => x.Id == model.Id);
                kategoriEntity.Adi = model.Adi;
                kategoriEntity.Aciklamasi = model.Aciklamasi;
                Repo.Update(kategoriEntity);
                return new SuccessResult("Ürün başarıyla güncellendi.");
            }
        }

        
    }
}
