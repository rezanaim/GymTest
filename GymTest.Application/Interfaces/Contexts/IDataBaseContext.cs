using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Threading;
using GymTest.Domain.Entities.Course;

namespace GymTest.Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserInCourse> UserInCourses { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
