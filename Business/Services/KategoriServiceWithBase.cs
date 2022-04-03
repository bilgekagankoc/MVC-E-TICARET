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
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
