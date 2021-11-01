using Business.Dto.Requests;
using Business.Dto.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IEntryService
    {
        public IEnumerable<EntryDetailedResultDto> Get (EntryParameters entryParams, int userId);
        Task<EntryResultDto> Create(EntryRequestDto request, int userId);
    }
}
