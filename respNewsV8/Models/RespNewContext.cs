using System;
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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Messagess> Messagesses { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsPhoto> NewsPhotos { get; set; }

    public virtual DbSet<NewsVideo> NewsVideos { get; set; }

    public virtual DbSet<Newspaper> Newspapers { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KHAYALSADIGOV;Database=respNew;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B7B4E882E");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CategoryStatus).HasDefaultValue(true);

            entity.HasOne(d => d.CategoryLang).WithMany(p => p.Categories)
                .HasForeignKey(d => d.CategoryLangId)
                .HasConstraintName("FK__Category__Catego__70DDC3D8");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Language__B93855AB23F317AD");

            entity.Property(e => e.LanguageName).HasMaxLength(20);
        });

        modelBuilder.Entity<Messagess>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CC120D82C");

            entity.ToTable("Messagess");

            entity.Property(e => e.MessageDate).HasColumnType("datetime");
            entity.Property(e => e.MessageIsRead).HasDefaultValue(false);
            entity.Property(e => e.MessageMail).HasMaxLength(30);
            entity.Property(e => e.MessageTitle).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3A0369529");

            entity.Property(e => e.NewsDate).HasColumnType("datetime");
            entity.Property(e => e.NewsTags).HasMaxLength(500);
            entity.Property(e => e.NewsTitle)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.NewsUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.NewsViewCount).HasDefaultValue(19);

            entity.HasOne(d => d.NewsAdmin).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsAdminId)
                .HasConstraintName("FK__News__NewsAdminI__02FC7413");

            entity.HasOne(d => d.NewsCategory).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsCategoryId)
                .HasConstraintName("FK__News__NewsCatego__4BAC3F29");

            entity.HasOne(d => d.NewsLang).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsLangId)
                .HasConstraintName("FK__News__NewsLangId__4CA06362");

            entity.HasOne(d => d.NewsOwner).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsOwnerId)
                .HasConstraintName("FK__News__NewsOwnerI__4D94879B");
        });

        modelBuilder.Entity<NewsPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__NewsPhot__21B7B5E2BC849FAC");

            entity.Property(e => e.PhotoUrl).HasColumnName("PhotoURL");

            entity.HasOne(d => d.PhotoNews).WithMany(p => p.NewsPhotos)
                .HasForeignKey(d => d.PhotoNewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NewsPhoto__Photo__4E88ABD4");
        });

        modelBuilder.Entity<NewsVideo>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__NewsVide__BAE5126A3E7B883F");

            entity.Property(e => e.VideoUrl).HasColumnName("VideoURL");

            entity.HasOne(d => d.VideoNews).WithMany(p => p.NewsVideos)
                .HasForeignKey(d => d.VideoNewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NewsVideo__Video__5070F446");
        });

        modelBuilder.Entity<Newspaper>(entity =>
        {
            entity.HasKey(e => e.NewspaperId).HasName("PK__Newspape__84EBB480626BFBB6");

            entity.Property(e => e.NewspaperDate).HasColumnType("datetime");
            entity.Property(e => e.NewspaperPrice)
                .HasMaxLength(20)
                .HasDefaultValue("Xeyr");
            entity.Property(e => e.NewspaperStatus).HasDefaultValue(true);
            entity.Property(e => e.NewspaperTitle).HasMaxLength(200);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owners__819385B8884DAED6");

            entity.Property(e => e.OwnerName).HasMaxLength(50);
            entity.Property(e => e.OwnerTotal)
                .HasDefaultValue(0)
                .HasColumnName("ownerTotal");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PK__Subscrib__4D9BB84A58305141");

            entity.Property(e => e.SubDate).HasColumnType("datetime");
            entity.Property(e => e.SubEmail).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C90B54810");

            entity.Property(e => e.UserName).HasMaxLength(30);
            entity.Property(e => e.UserPassword).HasMaxLength(30);
            entity.Property(e => e.UserRole).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
