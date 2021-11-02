using Business.Dto.Requests;
using Business.Dto.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IEntryService
    {
        public IEnumerable<EntryDetailedResultDto> Get (EntryParameters entryParams, int userId);
        public Task<EntryResultDto> Create(EntryRequestDto request, int userId);
        public Task<bool> KeyExists(int entryId);
        public EntryDetailedResultDto Get(int entryId);
        public Task<bool> Exists(EntryRequestDto request, int userId);
        public Task Update(EntryRequestDto newEntry, int entryId, int userId);
        public Task Delete(int key);
        public Task<bool> UserHasAccess(int entryId, int userId);
        public Task<bool> UserIsOwner(int entryKey, int userKey);
        public Task Share(int entryKey, int receiverKey);
        public Task CancelShare(int entryKey, int receiverKey);
    }
}
