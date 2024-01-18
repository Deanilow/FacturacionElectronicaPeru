namespace InvoicePeru.Application.Common.Interfaces.Persistence;
public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    void Add(User user);
}