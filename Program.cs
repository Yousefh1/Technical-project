﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the CSV file name:");

        // Use the null-forgiving operator to indicate that ReadLine will not return null
        string fileName = Console.ReadLine()!;

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (File.Exists(filePath))
            {
                Console.WriteLine($"File '{fileName}' found at: {filePath}");
                ProcessAndValidateCSV(filePath);
            }
            else
            {
                Console.WriteLine($"Error: File '{fileName}' not found in the current directory.");
            }
        }
        else
        {
            Console.WriteLine("Error: File name cannot be null or empty.");
        }
    }

    static void ProcessAndValidateCSV(string filePath)
    {
        List<string> validEmails = new List<string>();
        List<string> invalidEmails = new List<string>();

        try
        {
            // Use CsvHelper to read CSV records
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the CSV records dynamically
                var records = csv.GetRecords<dynamic>();

                foreach (var record in records)
                {
                    // Assuming the email is in the last column
                    string email = (record.Email as string)?.Trim() ?? string.Empty;

                    // Validate email and track in appropriate list
                    if (IsValidEmail(email))
                    {
                        validEmails.Add(email);
                    }
                    else
                    {
                        invalidEmails.Add(email);
                    }
                }
            }

            // Print valid and invalid email lists
            PrintEmailList("Valid Email Addresses", validEmails);
            PrintEmailList("Invalid Email Addresses", invalidEmails);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintEmailList(string title, List<string> emails)
    {
        Console.WriteLine($"\n{title}:");

        // Use string.Join for efficient concatenation
        Console.WriteLine(string.Join(Environment.NewLine, emails));
    }

  static bool IsValidEmail(string email)
{
    // Use a simple regular expression for email validation
    string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
    Regex regex = new Regex(pattern);
    
    // Check if the email matches the regular expression pattern
    bool isEmailValid = regex.IsMatch(email);

    // Optionally, you can add specific domain-based validation
    bool isGdcitEmail = email.EndsWith("@gdcit.com", StringComparison.OrdinalIgnoreCase);

    // Return true if the email is valid based on the regular expression and domain check
    return isEmailValid && isGdcitEmail;
}
}
