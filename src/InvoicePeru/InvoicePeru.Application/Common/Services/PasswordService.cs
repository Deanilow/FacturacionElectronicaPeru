namespace InvoicePeru.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string Base64Decode(string password)
        {
            password = password.Replace('-', '+');
            password = password.Replace('_', '/');
            password = password.PadRight(password.Length + (4 - password.Length % 4) % 4, '=');

            var data = Convert.FromBase64String(password);
            var ss = Encoding.UTF8.GetString(data);
            return ss;
        }

        public bool IsBase64String(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            IList<FluentValidation.Results.ValidationFailure> messages = new List<FluentValidation.Results.ValidationFailure>();

            if (password == null) messages.Add(new FluentValidation.Results.ValidationFailure("Password", "Password is null"));
            if (string.IsNullOrWhiteSpace(password)) messages.Add(new FluentValidation.Results.ValidationFailure("password", "Value cannot be empty or whitespace only string."));

            if (messages.Count > 0) throw new FluentValidation.ValidationException(messages);

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            IList<FluentValidation.Results.ValidationFailure> messages = new List<FluentValidation.Results.ValidationFailure>();

            if (password == null) messages.Add(new FluentValidation.Results.ValidationFailure("password", "Password is null"));
            if (string.IsNullOrWhiteSpace(password)) messages.Add(new FluentValidation.Results.ValidationFailure("password", "Value cannot be empty or whitespace only string."));
            if (storedHash.Length != 64) messages.Add(new FluentValidation.Results.ValidationFailure("passwordHash", "Invalid length of password hash (64 bytes expected)."));
            if (storedSalt.Length != 128) throw new ArgumentException("passwordHash", "Invalid length of password salt (128 bytes expected).");

            if (messages.Count > 0) throw new FluentValidation.ValidationException(messages);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        public string GeneratePassword(int passwordlength)
        {
            var pwd = new Password(passwordlength).IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("[]{}^_=");
            return pwd.Next();
        }
    }
}