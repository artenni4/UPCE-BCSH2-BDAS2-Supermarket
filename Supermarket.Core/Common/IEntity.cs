using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common
{
    /// <summary>
    /// Represents object that has some identifier
    /// </summary>
    /// <typeparam name="TId">type of identifier</typeparam>
    public interface IEntity<TId>
    {
        /// <summary>
        /// Identifier of the entity
        /// </summary>
        TId Id { get; }
    }
}
