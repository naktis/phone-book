using Business.Dto.Requests;

namespace Api.RequestProcessors.DefaultSetters
{
    public class EntryParamsSetter : IEntryParamsSetter
    {
        const int MaxPageSize = 50;
        const int DefaultPageSize = 10;

        public EntryParameters SetDefaultValues(EntryParameters entryParams)
        {
            if (entryParams.PageSize == 0)
                entryParams.PageSize = DefaultPageSize;

            if (entryParams.PageSize > MaxPageSize)
                entryParams.PageSize = MaxPageSize;

            if (entryParams.PageNumber == 0)
                entryParams.PageNumber = 1;

            return entryParams;
        }
    }
}
