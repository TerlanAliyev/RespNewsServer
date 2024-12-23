﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace respNewsV8.Models;

public partial class RespNewContext : DbContext
{
    public RespNewContext()
    {
    }

    public RespNewContext(DbContextOptions<RespNewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalLink> AdditionalLinks { get; set; }

    public virtual DbSet<Advert> Adverts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LocationDatum> LocationData { get; set; }

    public virtual DbSet<Logo> Logos { get; set; }

    public virtual DbSet<Messagess> Messagesses { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsPhoto> NewsPhotos { get; set; }

    public virtual DbSet<NewsVideo> NewsVideos { get; set; }

    public virtual DbSet<Newspaper> Newspapers { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Statisticss> Statisticsses { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Ytvideo> Ytvideos { get; set; }

    public virtual DbSet<İnfographic> İnfographics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Terlan;Database=respNew;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__Addition__2D122135DA4F3714");

            entity.Property(e => e.LinkName).HasMaxLength(2000);
            entity.Property(e => e.LinkVisibility).HasDefaultValue(true);
        });

        modelBuilder.Entity<Advert>(entity =>
        {
            entity.HasKey(e => e.AdvertId).HasName("PK__Advert__4FE88F442CDA0620");

            entity.ToTable("Advert");

            entity.Property(e => e.AdvertName).HasMaxLength(200);
            entity.Property(e => e.AdvertVisibility).HasDefaultValue(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B77A93A89");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CategoryStatus).HasDefaultValue(true);

            entity.HasOne(d => d.CategoryLang).WithMany(p => p.Categories)
                .HasForeignKey(d => d.CategoryLangId)
                .HasConstraintName("FK__Category__Catego__208CD6FA");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Language__B93855AB3EC91124");

            entity.Property(e => e.LanguageName).HasMaxLength(20);
        });

        modelBuilder.Entity<LocationDatum>(entity =>
        {
            entity.HasKey(e => e.Ipaddress).HasName("PK__Location__F0C25BE1FAD9CA67");

            entity.Property(e => e.Ipaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Logo>(entity =>
        {
            entity.HasKey(e => e.LogoId).HasName("PK__Logos__C620158D6747D59C");

            entity.Property(e => e.LogoName).HasMaxLength(200);
            entity.Property(e => e.LogoVisibility).HasDefaultValue(false);
        });

        modelBuilder.Entity<Messagess>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CA0A1C9FA");

            entity.ToTable("Messagess");

            entity.Property(e => e.MessageDate).HasColumnType("datetime");
            entity.Property(e => e.MessageIsRead).HasDefaultValue(false);
            entity.Property(e => e.MessageMail).HasMaxLength(30);
            entity.Property(e => e.MessageTitle).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF357B54349");

            entity.Property(e => e.NewsDate).HasColumnType("datetime");
            entity.Property(e => e.NewsTags).HasMaxLength(250);
            entity.Property(e => e.NewsTitle).HasMaxLength(255);
            entity.Property(e => e.NewsUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.NewsViewCount).HasDefaultValue(1);

            entity.HasOne(d => d.NewsAdmin).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsAdminId)
                .HasConstraintName("FK__News__NewsAdminI__2180FB33");

            entity.HasOne(d => d.NewsCategory).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsCategoryId)
                .HasConstraintName("FK__News__NewsCatego__22751F6C");

            entity.HasOne(d => d.NewsLang).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsLangId)
                .HasConstraintName("FK__News__NewsLangId__236943A5");

            entity.HasOne(d => d.NewsOwner).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsOwnerId)
                .HasConstraintName("FK__News__NewsOwnerI__245D67DE");
        });

        modelBuilder.Entity<NewsPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__NewsPhot__21B7B5E29C460EB2");

            entity.Property(e => e.PhotoUrl).HasColumnName("PhotoURL");

            entity.HasOne(d => d.PhotoNews).WithMany(p => p.NewsPhotos)
                .HasForeignKey(d => d.PhotoNewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NewsPhoto__Photo__5AEE82B9");
        });

        modelBuilder.Entity<NewsVideo>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__NewsVide__BAE5126ADB5810DE");

            entity.Property(e => e.VideoUrl).HasColumnName("VideoURL");

            entity.HasOne(d => d.VideoNews).WithMany(p => p.NewsVideos)
                .HasForeignKey(d => d.VideoNewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NewsVideo__Video__5CD6CB2B");
        });

        modelBuilder.Entity<Newspaper>(entity =>
        {
            entity.HasKey(e => e.NewspaperId).HasName("PK__Newspape__84EBB4804C01D002");

            entity.Property(e => e.NewspaperDate).HasColumnType("datetime");
            entity.Property(e => e.NewspaperPrice)
                .HasMaxLength(20)
                .HasDefaultValue("Xeyr");
            entity.Property(e => e.NewspaperStatus).HasDefaultValue(true);
            entity.Property(e => e.NewspaperTitle).HasMaxLength(200);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owners__819385B870979AE0");

            entity.Property(e => e.OwnerName).HasMaxLength(50);
            entity.Property(e => e.OwnerTotal).HasDefaultValue(0);
        });

        modelBuilder.Entity<Statisticss>(entity =>
        {
            entity.HasKey(e => e.StatisticId).HasName("PK__Statisti__367DEB174400F6A5");

            entity.ToTable("Statisticss");

            entity.HasIndex(e => new { e.VisitorIp, e.VisitDate }, "UQ__Statisti__24F31FC72EA88D32").IsUnique();

            entity.Property(e => e.IsAzLanguage).HasDefaultValue(false);
            entity.Property(e => e.IsDesktop).HasDefaultValue(false);
            entity.Property(e => e.IsEngLanguage).HasDefaultValue(false);
            entity.Property(e => e.IsMobile).HasDefaultValue(false);
            entity.Property(e => e.IsRuLanguage).HasDefaultValue(false);
            entity.Property(e => e.VisitDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VisitorCity)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VisitorCountry)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VisitorIp)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VisitorIP");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PK__Subscrib__4D9BB84A143B70E4");

            entity.Property(e => e.SubDate).HasColumnType("datetime");
            entity.Property(e => e.SubEmail).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE053EDA3");

            entity.Property(e => e.UserEmail).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(30);
            entity.Property(e => e.UserNickName).HasMaxLength(200);
            entity.Property(e => e.UserPassword).HasMaxLength(30);
            entity.Property(e => e.UserRole).HasMaxLength(30);
        });

        modelBuilder.Entity<Ytvideo>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__YTVideos__BAE5126A9B732CEC");

            entity.ToTable("YTVideos");

            entity.Property(e => e.VideoDate).HasColumnType("datetime");
            entity.Property(e => e.VideoStatus).HasDefaultValue(true);
            entity.Property(e => e.VideoTitle).HasMaxLength(300);
        });

        modelBuilder.Entity<İnfographic>(entity =>
        {
            entity.HasKey(e => e.InfId).HasName("PK__İnfograp__99CF2C73EAF63041");

            entity.Property(e => e.InfName).HasMaxLength(200);
            entity.Property(e => e.InfPostDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
