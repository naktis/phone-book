using Business.Dto.Requests;
using Business.Dto.Results;
using Data.Models;
using System;

namespace Business.Mappers
{
    public class EntryMapper : IEntryMapper
    {
        public Entry DtoToEntity(EntryRequestDto dto)
        {
            return new Entry
            {
                Name = dto.Name,
                Phone = dto.Phone
            };
        }

        public EntryResultDto EntityToDto(Entry entry)
        {
            return new EntryResultDto
            {
                EntryId = entry.EntryId
            };
        }

        public EntryDetailedResultDto EntityToDetailedDto(Entry entry)
        {
            return new EntryDetailedResultDto
            {
                EntryId = entry.EntryId,
                Name = entry.Name,
                Phone = entry.Phone,
                OwnerId = entry.OwnerId,
                OwnerUsername = entry.Owner.Username
            };
        }

        public Entry CopyFromDto(Entry entry, EntryRequestDto newEntry)
        {
            entry.Name = newEntry.Name;
            entry.Phone = newEntry.Phone;

            return entry;
        }
    }
}
