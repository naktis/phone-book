using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Mappers;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EntryService : IEntryService
    {
        private readonly PhoneBookContext _context;
        private readonly IEntryMapper _mapper;

        public EntryService(PhoneBookContext context, IEntryMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CancelShare(int entryKey, int receiverKey)
        {
            var userEntry = await GetUserEntry(entryKey, receiverKey);
            _context.UserEntries.Remove(userEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<EntryResultDto> Create(EntryRequestDto request, int userId)
        {
            var entry = _mapper.DtoToEntity(request);
            entry.OwnerId = userId;
            var createdEntry = await _context.Entries.AddAsync(entry);

            var user = await _context.Users.FindAsync(userId);
            await _context.UserEntries.AddAsync(new UserEntry { 
                User = user, 
                Entry = createdEntry.Entity 
            });

            await _context.SaveChangesAsync();

            return _mapper.EntityToDto(createdEntry.Entity);
        }

        public async Task Delete(int key)
        {
            var entry = await _context.Entries.FindAsync(key);
            var userEntries = await _context.UserEntries.Where(ue => ue.EntryId == key).ToListAsync();

            _context.UserEntries.RemoveRange(userEntries);
            _context.Entries.Remove(entry);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(EntryRequestDto request, int userId)
        {
            return await _context.Entries.FirstOrDefaultAsync(e => 
                e.OwnerId == userId && e.Phone == request.Phone) != null;
        }

        public IEnumerable<EntryDetailedResultDto> Get(EntryParameters entryParams, int userId)
        {
            var userEntries = _context.UserEntries
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Entry)
                .ThenInclude(e => e.Owner)
                .Skip((entryParams.PageNumber - 1) * entryParams.PageSize)
                .Take(entryParams.PageSize)
                .ToList();

            var entriesDto = new List<EntryDetailedResultDto>();

            foreach (var ue in userEntries)
                entriesDto.Add(_mapper.EntityToDetailedDto(ue.Entry));

            return entriesDto;
        }

        public EntryDetailedResultDto Get(int key)
        {
            var entry = _context.Entries.Where(e => e.EntryId == key).Include(e => e.Owner).First();
            return _mapper.EntityToDetailedDto(entry);
        }

        public async Task<bool> KeyExists(int key)
        {
            return await _context.Entries.FindAsync(key) != null;
        }

        public async Task Share(int entryKey, int receiverKey)
        {
            var receiver = await _context.Users.FindAsync(receiverKey);
            var entry = await _context.Entries.FindAsync(entryKey);

            await _context.UserEntries.AddAsync(new UserEntry
            {
                User = receiver,
                Entry = entry
            });

            await _context.SaveChangesAsync();
        }

        public async Task Update(EntryRequestDto newEntry, int entryId, int userId)
        {
            var entry = await _context.Entries.FindAsync(entryId);
            entry = _mapper.CopyFromDto(entry, newEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserHasAccess(int entryId, int userId)
        {
            return await GetUserEntry(entryId, userId) != null;
        }

        public async Task<bool> UserIsOwner(int entryKey, int userKey)
        {
            var entry = await _context.Entries.FindAsync(entryKey);
            return entry.OwnerId == userKey;
        }

        private async Task<UserEntry> GetUserEntry(int entryId, int userId)
        {
            return await _context.UserEntries.FirstOrDefaultAsync(ue =>
                ue.UserId == userId && ue.EntryId == entryId);
        }
    }
}
