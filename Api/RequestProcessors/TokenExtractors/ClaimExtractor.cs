using System;
using System.Linq;
using System.Security.Claims;

namespace Api.RequestProcessors.TokenExtractors
{
    public class ClaimExtractor : IClaimExtractor
    {
        public int GetUserId(ClaimsIdentity identity)
        {
            var claims = identity.Claims;
            return Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "id").Value);
        }
    }
}
