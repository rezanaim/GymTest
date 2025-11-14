using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;



namespace GymTest.Application.Interfaces.Contexts
{
    interface IDataBaseContext
    {
        DbSet<User> Users { get; set; }


    }
}
