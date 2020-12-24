using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Authentication.ApiKey
{
    public class ApiKeyProvider : IApiKeyProvider
    {
        private readonly ApplicationContext _context;

        public ApiKeyProvider(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IApiKey> ProvideAsync(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Key == key);

            if (client == null) return null;

            return new ApiKey(client.Key, client.Name, new List<Claim> {new Claim(ClaimTypes.Role, client.Role)});
        }
    }
}
