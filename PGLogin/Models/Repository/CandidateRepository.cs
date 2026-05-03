using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PGLogin.Models.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly MydBContext _mydBContext;

        public CandidateRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<Candidate> AddCandidate(CandidateDTO candidate)
        {
            var newCandidate = new Candidate
            {
                FullName = candidate.FullName,
                Phone = candidate.Phone,
                Email = candidate.Email,
                Address = candidate.Address,
                My_Photo = candidate.My_Photo,
                PhotoFile = candidate.PhotoFile,
                CreatedAt = candidate.CreatedAt
            };

            await _mydBContext.AddAsync(newCandidate);
            await _mydBContext.SaveChangesAsync();

            return newCandidate;
        }

        public async Task<Candidate> DeleteCandidate(Guid candidate_Id)
        {
            var result = await _mydBContext.candidates.FirstOrDefaultAsync(x => x.CandidateId == candidate_Id);
            if (result != null)
            {
                _mydBContext.Remove(result);
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Candidate>> GetAllCandidates()
        {
            return await _mydBContext.candidates.ToListAsync();
        }
        public Task<Candidate> GetCandidateById(Guid candidate_Id)
        {
            var result = _mydBContext.candidates.FirstOrDefaultAsync(x => x.CandidateId == candidate_Id);
            return result;
        }
        public async Task<Candidate> UpdateCandidate(Candidate candidate)
        {
            var result = await _mydBContext.candidates.FirstOrDefaultAsync(x => x.CandidateId == candidate.CandidateId);
            if (result != null)
            {
                result.CandidateId = candidate.CandidateId;
                result.FullName = candidate.FullName;
                result.Phone = candidate.Phone;
                result.Email = candidate.Email;
                result.Address = candidate.Address;
                result.CreatedAt = candidate.CreatedAt;
                await _mydBContext.SaveChangesAsync();
            }
            return candidate;
        }
    }
}
