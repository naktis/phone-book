using Api.RequestProcessors.Validators.Interfaces;
using Business.Dto.Requests;
using System;

namespace Api.RequestProcessors.Validators
{
    public class EntryParamsValidator : IEntryParamsValidator
    {
        public bool Validate(EntryParameters entryParams)
        {
            // Value 0 means unspecified, therefore is allowed and changed
            // in default setter
            if (entryParams.PageSize < 0 || entryParams.PageNumber < 0)
                return false;

            return true;
        }
    }
}
