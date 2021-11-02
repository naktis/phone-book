using Api.RequestProcessors.Validators;
using Business.Dto.Requests;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class UserValidator_ValidateLogin
    {
        private readonly UserValidator _validator;

        public UserValidator_ValidateLogin()
        {
            _validator = new UserValidator(new SharedValidator());
        }

        [Theory]
        [MemberData(nameof(InvalidLoginRequests))]
        public void EmailInvalid_ReturnFalse(LoginRequestDto request)
        {
            Assert.False(_validator.ValidateLogin(request));
        }

        [Theory]
        [MemberData(nameof(ValidLoginRequests))]
        public void EmailValid_ReturnTrue(LoginRequestDto request)
        {
            Assert.True(_validator.ValidateLogin(request));
        }

        public static IEnumerable<object[]> InvalidLoginRequests()
        {
            return new List<object[]>
            {
                new object[] { new LoginRequestDto { Email = "", Password="validpassword"} },
                new object[] { new LoginRequestDto { Email = "goodlengthemail", Password="validpassword"} },
                new object[] { new LoginRequestDto { Email = "emailwithout@dot", Password="validpassword"} },
                new object[] { new LoginRequestDto { Email = "emailwithout.at", Password="validpassword"} },
                new object[] { new LoginRequestDto { Email = "According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyway because bees don't care what humans think is impossible. Yellow, black. Yellow, black. Yellow, black. Yellow, black. Ooh, black and yellow!@gmail.com", Password="validpassword"} }
            };
        }
        public static IEnumerable<object[]> ValidLoginRequests()
        {
            return new List<object[]>
            {
                new object[] { new LoginRequestDto { Email = "valid@email.com", Password="validpassword"} }
            };
        }
    }
}
