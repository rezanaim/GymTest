using GymTest.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Persistence.Data
{
    public class DataBaseContext : DbContext //, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        DbSet<User> Users ;

    }
}
