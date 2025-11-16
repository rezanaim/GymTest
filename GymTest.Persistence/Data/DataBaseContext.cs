using GymTest.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTest.Application.Interfaces.Contexts;
using System.Threading;

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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }



        // در فایل DataBaseContext.cs

        // در فایل DataBaseContext.cs


        //DbSet<User> IDataBaseContext.Users { get; set; }

        //DbSet<User> IDataBaseContext.Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    }

}
