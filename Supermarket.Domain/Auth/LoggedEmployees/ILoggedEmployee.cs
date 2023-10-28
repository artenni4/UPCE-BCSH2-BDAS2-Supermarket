namespace Supermarket.Domain.Auth.LoggedEmployees
{
    /// <summary>
    /// Marker for logged employee
    /// </summary>
    public interface ILoggedEmployee
    {
        int Id { get; }
        string Name { get; }
        string Surname { get; }
    }
}
