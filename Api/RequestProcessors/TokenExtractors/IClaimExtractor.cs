using System.Security.Claims;

namespace Api.RequestProcessors.TokenExtractors
{
    public interface IClaimExtractor
    {
        public int GetUserId(ClaimsIdentity identity);
    }
}
