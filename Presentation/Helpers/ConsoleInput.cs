namespace Presentation.Helpers
{
    public static class ConsoleInput
    {
        public static string ReadRequiredString(string label)
        {
            while (true)
            {
                Console.Write($"{label}:");
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("input cannot blank. try again..");
            }
        }

        public static int ReadRequiredInt(string label)
        {
            while (true)
            {
                Console.Write($"{label}:");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Please enter a valid number");
            }
        }

        public static DateTime ReadRequiredDate(string label)
        {
            while (true)
            {
                Console.Write($"{label} (yyyy-MM-dd):");
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out var date))
                    return date.Date;

                Console.WriteLine("Invalid date format");
            }
        }
    }
}
