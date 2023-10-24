namespace Ryder.Infrastructure.Utility
{
    public static class PaystackUtility
    {
        public static string GenerateUniqueReference(string prefix = "RYDER", int length = 15)
        {
            if (prefix.Length > length)
            {
                throw new ArgumentException("Prefix length should not exceed the total length.");
            }

            string randomPart = Guid.NewGuid().ToString("N").Substring(0, length - prefix.Length);

            string uniqueReference = $"{prefix}{randomPart}";

            return uniqueReference;
        }
    }
}
