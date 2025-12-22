using GymTest.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTest.Application.Interfaces.Contexts;
using System.Threading;
using GymTest.Common;
using GymTest.Domain.Entities.CourseNmore;
using GymTest.Domain.Entities.WalletNmore;

namespace GymTest.Persistence.Data
{

    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserInCourse> UserInCourses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }



            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

            base.OnModelCreating(modelBuilder);


            // اعمال ایندکس بر روی فیلد ایمیل
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();



            // ================== مدیریت صریح روابط و رفتار حذف ==================
            // ۱. رابطه Course <--> User (مربی)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Coach)
                .WithMany(u => u.CoachedCourses)
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // ۲. رابطه UserInCourse <--> User (شرکت‌کننده)

            // رابطه UserInCourse <--> User (استاندارد شده)
            modelBuilder.Entity<UserInCourse>()
                .HasOne(uic => uic.User) // استفاده از نام جدید
                .WithMany(u => u.UserInCourses) // استفاده از نام جدید
                .HasForeignKey(uic => uic.UserId) // کلید خارجی حالا درست است
                .OnDelete(DeleteBehavior.Restrict); // 

            // ۳. رابطه UserInCourse <--> Course
            modelBuilder.Entity<UserInCourse>()
                .HasOne(uic => uic.Course)
                .WithMany(c => c.Registrations) // استفاده از نام جدید
                .HasForeignKey(uic => uic.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            // ====================================================================

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u=> u.Wallet)
                .HasForeignKey<Wallet>(w=> w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //در یک به یک تعریف رابطه صریح علاوه بر نویگیشن پراپرتی ها الزامیه
            // چون نمیدونه اف کی کدومه و کدوم وابسته هستش که اینجا تو خط سوم مشخصش کردیم

            //Seed Data
            SeedData(modelBuilder);


                // اعمال ایندکس بر روی فیلد ایمیل
                // اعمال عدم تکراری بودن ایمیل
                modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();







            //-- عدم نمایش اطلاعات حذف شده
            ApplyQueryFilter(modelBuilder);
            }

            private void ApplyQueryFilter(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Role>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<UserInRole>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Sport>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Course>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Lecture>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Wallet>().HasQueryFilter(p => !p.IsRemoved);
                modelBuilder.Entity<Transaction>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<ProductImages>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<ProductFeatures>().HasQueryFilter(p => !p.IsRemoved);
            }

            private void SeedData(ModelBuilder modelBuilder)
            {
            /*
            modelBuilder.Entity<User>().HasData(new User()
            {
                FirstName = "Reza",
                LastName = "Naeem",
                Email = "reza.naim1380@gmail.com",
                IsActive = true,
                RegisterTime = DateTime.Now,
                Password = hashedPassword

            });
            */
                modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
                modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Coach) });
                modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });

                modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = nameof(CategorySeed.Ball) });
                modelBuilder.Entity<Category>().HasData(new Category { Id = 2, Name = nameof(CategorySeed.Mind) });
                modelBuilder.Entity<Category>().HasData(new Category { Id = 3, Name = nameof(CategorySeed.Fight) });
                modelBuilder.Entity<Category>().HasData(new Category { Id = 4, Name = nameof(CategorySeed.Winter) });

                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 1, Name = nameof(SportSeed.Football), CategoryId = 1 });
                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 2, Name = nameof(SportSeed.Pingpong), CategoryId = 1});
                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 3, Name = nameof(SportSeed.Chess), CategoryId = 2});
                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 4, Name = nameof(SportSeed.Wrestling), CategoryId = 3});
                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 5, Name = nameof(SportSeed.Boxing), CategoryId = 3});
                modelBuilder.Entity<Sport>().HasData(new Sport { Id = 6, Name = nameof(SportSeed.Ski), CategoryId = 4 });
                



                

                
            }

        }



    }


