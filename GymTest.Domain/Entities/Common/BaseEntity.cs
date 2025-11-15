using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Domain.Entities.Common
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime RemovedTime { get; set; }
        public bool IsRemoved { get; set; } = false;

    }
    public class BaseEntity : BaseEntity<long>
    {

    }
}

