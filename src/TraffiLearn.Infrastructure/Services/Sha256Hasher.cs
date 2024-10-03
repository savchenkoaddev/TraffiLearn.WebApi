using System.Security.Cryptography;
using System.Text;
using TraffiLearn.Application.Abstractions.Services;

namespace TraffiLearn.Infrastructure.Services
{
    public sealed class Sha256Hasher : IHasher
    {
        public string Hash(string value)
        {
            return Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(value)));
        }
    }
}
