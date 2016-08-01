using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS
{
    public interface IEntity
    {
        /// <summary>
        /// The only member an entity should have is an ID, to diffrentiate it from other entities
        /// </summary>
        Guid Id { get; set; }
    }
}
