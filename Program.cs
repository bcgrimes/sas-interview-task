using System;
using System.IO;
using System.Collections.Generic;
using sas_interview_task.models;
using sas_interview_task.tools;

namespace sas_interview_task;

class Program
{
    static void Main(string[] args)
    {
        string csvDataFile = args.Length > 0 && args[0].Length > 0 ? args[0] : @".\sample-data.csv";
        string[] headerFields;
        Invoice[] invoices;
        Console.WriteLine("Hello, SAS!\n");
        GetInvoiceList(csvDataFile, out headerFields, out invoices);
        DisplayInvoiceList(invoices);
        InteractiveSession(headerFields, invoices);
    }

    static void GetInvoiceList(string path, out string[] headerFields, out Invoice[] results)
    {
        headerFields = [];
        results = [];
        List<Invoice> invoices = new List<Invoice>();
        if (File.Exists(path))
        {
            Console.WriteLine("Loading invoices data file: '{0}'\n", path);
            try {
                using (StreamReader reader = new StreamReader(path))
                {
                    // Process header line
                    string header = reader.ReadLine() ?? "";
                    string[] headers = header.Replace(" ", "").Split(',', StringSplitOptions.TrimEntries);
                    if (headers.Length != 4)
                    {
                        Console.WriteLine("ERROR: invalid data file, incorrect header.");
                        return;
                    } else { // TODO: add specific header field validation?
                        headerFields = headers;
                        // Display the headers
                        Console.WriteLine(header);
                    }
                    while (reader.Peek() >= 0)
                    {
                        Invoice invoice;
                        if (Invoice.TryParse(reader.ReadLine() ?? "", out invoice))
                        {
                            invoices.Add(invoice);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The data load failed: {0}", e.ToString());
            }
        } else {
            Console.WriteLine("The data file '{0}' does not exist.", path);
        }
        results = invoices.ToArray();
    }

    static void DisplayInvoiceList(Invoice[] invoices)
    {
        foreach (Invoice invoice in invoices)
        {
            Console.WriteLine(invoice.ToString());
        }
    }

    static void InteractiveSession(string[] headerFields, Invoice[] invoices)
    {
        // Validate arguments
        if (headerFields.Length == 4 && invoices.Length > 0)
        {
            bool sessionComplete = false;
            string userResponse = "";
            int primarySortIndex = -1;
            int secondarySortIndex = -1;
            bool primarySortASC = true;
            SortedListBuilder sortedListBuilder = new SortedListBuilder(headerFields[0]);
            while (!sessionComplete)
            {
                // Ask user to continue sorting or quit program
                Console.WriteLine("\nDo you wish to sort the data? [Y]|N");
                userResponse = Console.ReadLine() ?? "Y";
                if (userResponse.Equals("") || userResponse.ToUpper()[0] == 'Y')
                {
                    // Request sorting parameters, primary first
                    primarySortIndex = -1;
                    primarySortASC = true;
                    Console.WriteLine($"> Enter primary sort column index: 0 - {headerFields[0]}, 1 - {headerFields[1]}, 2 - {headerFields[2]}, 3 - {headerFields[3]}");
                    userResponse = Console.ReadLine() ?? "";
                    if (int.TryParse(userResponse, out primarySortIndex) && primarySortIndex >= 0 && primarySortIndex < 4) {
                        Console.WriteLine("> Enter sort order, ascending or descending: [A]|D");
                        userResponse = Console.ReadLine() ?? "A";
                        primarySortASC = (userResponse.Equals("") || userResponse.ToUpper()[0] == 'A');
                        // Request optional secondary sorting parameters
                        Console.WriteLine("\nDo you wish to add a secondary sort column? [Y]|N");
                        userResponse = Console.ReadLine() ?? "Y";
                        if (userResponse.Equals("") || userResponse.ToUpper()[0] == 'Y')
                        {
                            secondarySortIndex = -1;
                            Console.WriteLine($"> Enter secondary sort column index: 0 - {headerFields[0]}, 1 - {headerFields[1]}, 2 - {headerFields[2]}, 3 - {headerFields[3]}");
                            userResponse = Console.ReadLine() ?? "";
                            if (int.TryParse(userResponse, out secondarySortIndex) && (secondarySortIndex < 0 || secondarySortIndex > 4)) {
                                secondarySortIndex = -1;
                                Console.WriteLine("WARNING: invalid secondary column index, skipping secondary sort.");
                            }
                        }
                        // Perform sort with provided parameters
                        Console.WriteLine($"Primary Sort Column: {headerFields[primarySortIndex]}");
                        Console.WriteLine("Primary Sort Order:  " + (primarySortASC ? "ASC" : "DESC"));
                        if (secondarySortIndex >= 0 && secondarySortIndex < 4)
                        {
                            Console.WriteLine($"Secondary Sort Column: {headerFields[secondarySortIndex]}");
                            sortedListBuilder.Sort(invoices, headerFields[primarySortIndex], primarySortASC, headerFields[secondarySortIndex]);
                        } else {
                            sortedListBuilder.Sort(invoices, headerFields[primarySortIndex], primarySortASC);
                        }
                        // Display sorted results
                        DisplayInvoiceList(invoices);
                    } else {
                        Console.WriteLine($"WARNING: invalid response: {userResponse}");
                    }
                } else {
                    sessionComplete = true;
                }
            }
        }
    }
}
