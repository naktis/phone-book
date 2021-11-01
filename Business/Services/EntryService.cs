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
    }
}
