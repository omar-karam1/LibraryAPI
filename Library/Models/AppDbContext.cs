using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }
   

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db17513.public.databaseasp.net; Database=db17513; User Id=db17513; Password=tE?3+2RdTz5#; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
    //Server=LAPTOP-R0QOES6Q\\SQLEXPRESS01;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvertisementId).HasName("PK__Advertis__C4C7F42D84C35B3B");

            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC149EB579F0");

            entity.HasIndex(e => e.Name, "IDX_Authors_Name");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.AuthorImage)
                .HasMaxLength(255)
                .HasDefaultValue("AuthorImage");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C227E0AA4D87");

            entity.HasIndex(e => e.Title, "IDX_Books_Title");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.BookDescription).HasMaxLength(500);
            entity.Property(e => e.BookImage)
                .HasMaxLength(255)
                .HasDefaultValue("BookImage");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsFree).HasDefaultValue(true);
            entity.Property(e => e.Price)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Books__CategoryI__5629CD9C");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK__BookAutho__Autho__5CD6CB2B"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("FK__BookAutho__BookI__5BE2A6F2"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PK__BookAuth__6AED6DE6819B1DB4");
                        j.ToTable("BookAuthors");
                        j.IndexerProperty<int>("BookId").HasColumnName("BookID");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("AuthorID");
                    });
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BA4962630");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32976229BF");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__06CD04F7");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF12C1D6AC");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserID__60A75C0F");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06A1E803CB7E");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__BookI__503BEA1C");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__4F47C5E3");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE7632458D");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__Reviews__BookID__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__6A30C649");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B8E5EB77D");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Order__2A164134");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC9C2E6B63");

            entity.HasIndex(e => e.UserName, "IDX_Users_UserName").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342F8D4D9F").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456DEC03A38").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(255)
                .HasDefaultValue("ProfileImage");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .HasDefaultValue("User");
        });





        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId).HasName("PK__WishList__E41F87A7EB248E30");

            entity.ToTable("WishList");

            entity.Property(e => e.WishListId).HasColumnName("WishListID");
            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__WishList__UserID__0E6E26BF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
