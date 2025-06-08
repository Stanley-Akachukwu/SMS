namespace SMS.ApiService.Helpers
{
    public static class PasswordGenerator
    {
        private static readonly Random random = new Random();

        public static string GeneratePassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%&*";

            // Generate required characters
            char upper = upperCase[random.Next(upperCase.Length)];
            string digitSet = new string(Enumerable.Range(0, 3).Select(_ => digits[random.Next(digits.Length)]).ToArray());
            char special = specialChars[random.Next(specialChars.Length)];
            string lowerSet = new string(Enumerable.Range(0, 3).Select(_ => lowerCase[random.Next(lowerCase.Length)]).ToArray());

            // Combine and shuffle
            string combined = new string((upper + digitSet + special + lowerSet)
                .ToCharArray()
                .OrderBy(_ => random.Next())
                .ToArray());

            return combined;
        }
    }
}
