namespace PGLogin.Models.Repository.IRepository
{
    public interface IServiceRepository
    {
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
