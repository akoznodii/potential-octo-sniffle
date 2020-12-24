using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCore.Authentication.ApiKey
{
    public class ApiKey : IApiKey
    {
        public ApiKey(string key, string owner, List<Claim> claims = null)
        {
            Key = key;
            OwnerName = owner;
            Claims = claims ?? new List<Claim>();
        }

        public string Key { get; }
        public string OwnerName { get; }
        public IReadOnlyCollection<Claim> Claims { get; }
    }
}
