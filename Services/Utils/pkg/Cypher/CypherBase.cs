using System.Security.Cryptography;
using System.Text;
using pkg.Interfaces;

namespace pkg.cypher
{
	public class CypherBase : ICypher
    {
        public string CalculateSHA512Hash(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Convert byte to hex format
                }
                return sb.ToString();
            }
        }
    }
}

