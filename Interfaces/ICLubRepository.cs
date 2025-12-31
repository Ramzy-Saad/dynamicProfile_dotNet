using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface ICLubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        Task<bool> Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
