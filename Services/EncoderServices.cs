using BCrypt.Net;

namespace Backend_Prog4.Services
{
    public class EncoderServices : IEncoderServices
    {
        public string Encode(string toHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(toHash, workFactor: 13);
        }

        public bool Verify(string str, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(str, hash);
        }
    }
}
