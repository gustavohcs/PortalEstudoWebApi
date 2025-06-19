public static class HashingHelper
{
    public static (string PasswordHash, string Salt) HashPassword(string password)
    {
        // Gerando um salt aleatório
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            byte[] saltBytes = new byte[16]; // 16 bytes de salt
            rng.GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            // Gerando o hash da senha com o salt usando HMACSHA512
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(salt)))
            {
                byte[] passwordHashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                string passwordHash = Convert.ToBase64String(passwordHashBytes);
                return (passwordHash, salt);
            }
        }
    }

    // Verificando a senha
    public static bool VerifyPassword(string password, string storedHash, string salt)
    {
        // Converter o salt de Base64 novamente antes de usar no HMACSHA512
        using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(salt)))
        {
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            string computedHashBase64 = Convert.ToBase64String(computedHash);
            return computedHashBase64 == storedHash;  // Verificar se o hash gerado é igual ao armazenado
        }
    }

}
