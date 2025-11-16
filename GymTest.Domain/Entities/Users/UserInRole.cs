using GymTest.Domain.Entities.Commons;


namespace GymTest.Domain.Entities.Users
{
    public class UserInRole : BaseEntity
    {
        public long Id { get; set; }

        // Relation with User Table
        public virtual User User { get; set; }
        public long UserId { get; set; }

        //Relation with Roles Table
        public virtual Role Role { get; set; }
        public long RoleId { get; set; }


    }
}
