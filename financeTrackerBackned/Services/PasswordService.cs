// var hasher = new PasswordHasher<ApplicationUser>();
// string hashed = hasher.HashPassword(user, plainPassword);

// // Verify
// var result = hasher.VerifyHashedPassword(user, storedHashedPassword, inputPassword);
// bool isValid = result == PasswordVerificationResult.Success;
using financeTrackerBackned.Domain;
using Microsoft.AspNetCore.Identity;

namespace financeTrackerBackned.Services
{
    public class PasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(User user, string plainPassword)
        {
            return _passwordHasher.HashPassword(user, plainPassword);
        }

        public bool IsEqual(User user, string hashedPassword, string plainPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
