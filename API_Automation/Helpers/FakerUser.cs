using System;
using System.Text.RegularExpressions;
using API_Automation.Models.Request;
using Bogus;

namespace API_Automation.Helpers
{
    public static class FakeUserFactory
    {
        private static readonly Random _random = new();

        public static string GenerateValidPassword()
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            bool isValidPassword = false;
            string passwordString = String.Empty;

            while (!isValidPassword)
            {
                char GetRandomChar(string chars) => chars[_random.Next(chars.Length)];

            var password = new char[8];
                password[0] = GetRandomChar(lower);
                password[1] = GetRandomChar(upper);
                password[2] = GetRandomChar(digits);
                password[3] = GetRandomChar(special);

                // Fill the rest randomly from all character sets
                string allChars = lower + upper + digits + special;
                for (int i = 4; i < password.Length; i++)
                {
                    password[i] = GetRandomChar(allChars);
                }

                passwordString = (new string(password.OrderBy(_ => _random.Next()).ToArray()));
                isValidPassword = IsValidPassword(passwordString); // Validate the password
                if (isValidPassword)
                {
                    Logger.Log($"Generated valid password: {passwordString}");
                    break; // Exit the loop if a valid password is generated
                }
            }
            // Shuffle the array so it's not predictable
            return passwordString;
        }

        public static Faker<UserRequest> GetUserFaker()
        {
            return new Faker<UserRequest>()
                .RuleFor(u => u.userName, f => f.Internet.UserName())
                .RuleFor(u => u.password, _ => GenerateValidPassword());
        }

        public static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z0-9])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }

}
