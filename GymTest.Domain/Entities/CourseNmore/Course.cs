using GymTest.Domain.Entities.Commons;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Domain.Entities.CourseNmore
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public string SportsName { get; set; }
        //public int CouchName { get; set; }

        public decimal Price { get; set; }
        public int Capacity { get; set; }
        //public DateTime StartDate { get; set; }
        public int DurationInMins{ get; set; }
        public int LectureCount { get; set; }

        //relations

        public virtual User Coach { get; set; }
        public long CoachId { get; set; }
        //این رابطه یک به یک با یوزر مربوط به رابطه یک به یک بین مدرس و دوره است

        public virtual ICollection<UserInCourse> Registrations { get; set; } 
        // اینم رابطه یک به چند بین دوره و یوزر ها (مشتریان) هست

        public virtual Sport Sport { get; set; }
        public long SportId { get; set; }
        //ما یعنی "دوره" با واسطه به کتگوری مرتبط هستیم 
        // یعنی از طریق اسپورت پس نیازی به رابطه مستقیم با کتگوری نیست. در واقع توصیه نمیشه


        
    }


    // ----------- sport------//

    public class Sport : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //relations
        public ICollection<Course> Courses { get; set; }

        public virtual Category Category { get; set; }
        public long CategoryId { get; set; }
    }


    //--------Category--//

    public class Category : BaseEntity
    {
        public string Name { get; set; }

        //relations
        public virtual Category ParentCategory { get; set; }
        public long? ParentId { get; set; }

        public ICollection<Category> SubCategories { get; set; }


        //public ICollection<Course> Courses { get; set; }
        
        public ICollection<Sport> Sports { get; set; }
    }

    //----UserInCourse---//
    public class UserInCourse : BaseEntity
    {
        //relation
        public virtual Course Course { get; set; }
        public long CourseId { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }

    }
}

