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
    public interface IUlkeService : IService<UlkeModel,Ulke,ETicaretContext>
    {

    }
    public class UlkeService : IUlkeService
    {
        public RepoBase<Ulke, ETicaretContext> Repo { get; set; } = new Repo<Ulke, ETicaretContext>();

        public Result Add(UlkeModel model)
        {
            if(Repo.Query().Any(u => u.Adi.ToUpper() == model.Adi.ToUpper().Trim()))
            {
                return new ErrorResult("Girdiğiniz ülke ismi bulunmaktadır");
            }
            Ulke entity = new Ulke()
            {
                Adi = model.Adi,
                Guid = Guid.NewGuid().ToString(),
            };
            Repo.Add(entity);
            return new SuccessResult("Ülke başarıyla eklendi");

        }

        public Result Delete(int id)
        {
            if (Repo.Query().Any(x => x.Sehirler.Count > 0))
            {
                return new ErrorResult("Ülkeye bağlı şehirler bulunmaktadır");
            }
            else if (Repo.Query().Any(x => x.KullaniciDetaylari.Count > 0))
            {
                return new ErrorResult("Şehire bağlı kullanıcılar bulunmaktadır.");
            }
            else
            {
                Repo.Delete(x=>x.Id == id);
                return new SuccessResult("Ülke başarıyla silindi");
            }
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<UlkeModel> Query()
        {
            var result = Repo.Query().OrderBy(x => x.Adi).Select(x => new UlkeModel()
            {
                Adi = x.Adi,
                Id = x.Id
            });
            return result;
        }

        public Result Update(UlkeModel model)
        {
            if (Repo.Query().Any(u => u.Adi.ToUpper() == model.Adi.ToUpper().Trim() && u.Id != model.Id))
            {
                return new ErrorResult("Girdiğiniz ülke adına sahip kayıt bulunmaktadır!");
            }
            else
            {
                Ulke entity = Repo.Query(x => x.Id == model.Id).SingleOrDefault();
                entity.Adi = model.Adi;
                Repo.Update(entity);
                return new SuccessResult("Ülke başarıyla güncellendi.");
            }
                
        }
    }
}
