using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Domain.Entities.Commons.BaseEntityNotId
{
    public abstract class BaseEntityNotId
    { 
        public DateTime InsertTime { get; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime RemoveTime { get; set; }

    }
}
