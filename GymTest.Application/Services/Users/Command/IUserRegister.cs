using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymTest.Application.Services.Users.Command
{
    interface IUserRegister
    {
        public void Execute(User user);
    }
    public class UserRegister : IUserRegister
    {

        private readonly IDataBaseContext _context;
        public UserRegister(IDataBaseContext context)
        {
            _context = context;
        }
        public void Execute(User user)
        {
            throw new NotImplementedException();
        }
    }
}
