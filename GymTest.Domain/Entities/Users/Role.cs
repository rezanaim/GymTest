using GymTest.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Domain.Entities.Users
{
    public class Role : BaseEntity
    {
        //Admin or
        public string Name { get; set; }
        
        //Relation Between Tables
        public ICollection<UserInRole> UserInRoles {get;set;}
    }
}


