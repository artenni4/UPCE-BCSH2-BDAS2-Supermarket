namespace Supermarket.Core.Domain.Common
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
