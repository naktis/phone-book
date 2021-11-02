using Api.RequestProcessors.Validators.Interfaces;
using Business.Dto.Requests;
using System;
using System.Linq;

namespace Api.RequestProcessors.Validators
{
    public class EntryValidator : IEntryValidator
    {
        public readonly int MinNameLength = 1;
        public readonly int MaxNameLength = 30;
        public readonly int MinPhoneLength = 9;
        public readonly int MaxPhoneLength = 12;
        public readonly ISharedValidator _sharedValidator;

        public EntryValidator(ISharedValidator sharedValidator)
        {
            _sharedValidator = sharedValidator;
        }

        public bool Validate(EntryRequestDto entry)
        {
            return _sharedValidator.TextLengthValid(entry.Name, MaxNameLength, MinNameLength) &&
                   _sharedValidator.TextLengthValid(entry.Phone, MaxPhoneLength, MinPhoneLength) &&
                   TextLettersSpacesOnly(entry.Name) &&
                   PhoneFormatValid(entry.Phone);
        }

        private bool TextLettersSpacesOnly(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private bool PhoneFormatValid(string phone)
        {
            return phone.All(c => char.IsDigit(c) || c == '+');
        }
    }
}
