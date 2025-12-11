using GymTest.Domain.Entities.Commons;

using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymTest.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisterTime { get; set; } = DateTime.Now;
        public ICollection<UserInRole> UserInRoles { get; set; }
        public virtual ICollection<UserInCourse> UserInCourses { get; set; }

        public virtual ICollection<Course> CoachedCourses { get; set; }
       
    }
    
}
