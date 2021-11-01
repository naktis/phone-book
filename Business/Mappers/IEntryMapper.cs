using Business.Dto.Requests;
using Business.Dto.Results;
using Data.Models;

namespace Business.Mappers
{
    public interface IEntryMapper : IMapper<Entry, EntryRequestDto, EntryDetailedResultDto, EntryResultDto> { }
}
