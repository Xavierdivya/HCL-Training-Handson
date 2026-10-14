
using System;
using System.Text.RegularExpressions;

namespace TrainingManagementSystem.Utilities;

public static class InputValidator
{
    public static int ReadPositiveInteger(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a positive integer.");
        }
    }

    public static string ReadRequiredText(string message)
    {
        while (true)
        {
            Console.Write(message);

            string? value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("This field cannot be empty.");
        }
    }

    public static bool IsValidContactNumber(string contactNumber)
    {
        if (string.IsNullOrWhiteSpace(contactNumber))
        {
            return false;
        }

        string digits = Regex.Replace(contactNumber, @"[\s()+-]", "");

        return Regex.IsMatch(digits, @"^\d{10,15}$");
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return Regex.IsMatch(
            email.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
