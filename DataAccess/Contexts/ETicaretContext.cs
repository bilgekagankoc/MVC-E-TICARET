using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class ETicaretContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(connectionString: @"server=.;database=MVCCALISMA;user id=sa;password=122333;multipleactiveresultsets=true;"); //EV
            optionsBuilder.UseSqlServer(connectionString: @"Server=(localdb)\mssqllocaldb; initial catalog  =ETICARET; integrated security = true");
        }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Magaza> Magazalar { get; set; }
        public DbSet<UrunMagaza> UrunMagazalar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<KullaniciDetayi> KullaniciDetaylari { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Ulke> Ulkeler { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>()
                .ToTable("ETicaretUrunler") // genelde tablo isimlerini değiştirmeye gerek yoktur, DbSet ismini kullanır (Urunler)
                .HasOne(urun => urun.Kategori)
                .WithMany(kategori => kategori.Urunler)
                .HasForeignKey(urun => urun.KategoriId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Urun>()
                .HasMany(urun => urun.UrunMagazalar)
                .WithOne(urunMagaza => urunMagaza.Urun)
                .HasForeignKey(urunMagaza => urunMagaza.UrunId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Magaza>()
                .ToTable("ETicaretMagazalar")
                .HasMany(magaza => magaza.UrunMagazalar)
                .WithOne(urunMagaza => urunMagaza.Magaza)
                .HasForeignKey(urunMagaza => urunMagaza.MagazaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kategori>()
                .ToTable("ETicaretKategoriler");

            modelBuilder.Entity<UrunMagaza>()
                .ToTable("ETicaretUrunMagazalar")
                .HasKey(urunMagaza => new { urunMagaza.UrunId, urunMagaza.MagazaId });

            modelBuilder.Entity<Kullanici>()
                .ToTable("ETicaretKullanicilar")
                .HasOne(kullanici => kullanici.Rol)
                .WithMany(rol => rol.Kullanicilar)
                .HasForeignKey(kullanici => kullanici.RolId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
                .ToTable("ETicaretKullaniciDetaylari")
                .HasOne(kullaniciDetayi => kullaniciDetayi.Kullanici)
                .WithOne(kullanici => kullanici.KullaniciDetayi)
                .HasForeignKey<KullaniciDetayi>(kullaniciDetayi => kullaniciDetayi.KullaniciId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
                .HasOne(kullaniciDetayi => kullaniciDetayi.Ulke)
                .WithMany(ulke => ulke.KullaniciDetaylari)
                .HasForeignKey(kullaniciDetayi => kullaniciDetayi.UlkeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
                .HasOne(kullaniciDetayi => kullaniciDetayi.Sehir)
                .WithMany(sehir => sehir.KullaniciDetaylari)
                .HasForeignKey(kullaniciDetayi => kullaniciDetayi.SehirId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Sehir>()
                .ToTable("ETicaretSehirler")
                .HasOne(sehir => sehir.Ulke)
                .WithMany(ulke => ulke.Sehirler)
                .HasForeignKey(sehir => sehir.UlkeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ulke>().ToTable("ETicaretUlkeler");

            modelBuilder.Entity<Rol>().ToTable("ETicaretRoller");

            modelBuilder.Entity<Urun>().HasIndex(urun => urun.Adi);

            modelBuilder.Entity<Kullanici>().HasIndex(kullanici => kullanici.KullaniciAdi).IsUnique();

            modelBuilder.Entity<KullaniciDetayi>().HasIndex(kullaniciDetay => kullaniciDetay.Eposta).IsUnique();
        }
    }

}
