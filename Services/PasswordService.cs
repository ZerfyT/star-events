using System.Security.Cryptography;

namespace star_events.Services;

public class PasswordService : IPasswordService
{
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DigitChars = "0123456789";
    private const string SpecialChars = "!@#$%&";

    public string GenerateRandomPassword(int length = 8)
    {
        if (length < 8)
            throw new ArgumentException("Password length must be at least 8 characters", nameof(length));

        var allChars = LowercaseChars + UppercaseChars + DigitChars + SpecialChars;
        var password = new char[length];
        
        // Ensure at least one character from each category
        password[0] = LowercaseChars[RandomNumberGenerator.GetInt32(LowercaseChars.Length)];
        password[1] = UppercaseChars[RandomNumberGenerator.GetInt32(UppercaseChars.Length)];
        password[2] = DigitChars[RandomNumberGenerator.GetInt32(DigitChars.Length)];
        password[3] = SpecialChars[RandomNumberGenerator.GetInt32(SpecialChars.Length)];

        // Fill the rest with random characters
        for (int i = 4; i < length; i++)
        {
            password[i] = allChars[RandomNumberGenerator.GetInt32(allChars.Length)];
        }

        // Shuffle the password array
        for (int i = 0; i < length; i++)
        {
            int randomIndex = RandomNumberGenerator.GetInt32(length);
            (password[i], password[randomIndex]) = (password[randomIndex], password[i]);
        }

        return new string(password);
    }
}
