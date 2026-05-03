using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetCandidateById(Guid candidate_Id);
        Task<IEnumerable<Candidate>> GetAllCandidates();
        Task<Candidate> AddCandidate(CandidateDTO candidate);
        Task<Candidate> UpdateCandidate(Candidate candidate);
        Task<Candidate> DeleteCandidate(Guid candidate_Id);
    }
}
