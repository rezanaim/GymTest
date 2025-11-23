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
using GymTest.Domain.Entities.Course;

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


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }



            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

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
                //modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<ProductImages>().HasQueryFilter(p => !p.IsRemoved);
                //modelBuilder.Entity<ProductFeatures>().HasQueryFilter(p => !p.IsRemoved);
            }

            private void SeedData(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
                modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Coach) });
                modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });
            }

        }



    }


