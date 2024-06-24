using System;

namespace sas_interview_task.models;

public class Invoice
{
    // Constructors

    public Invoice () {}

    public Invoice(string name, string address, int invoiceAmount, DateTime dateOfSale)
    {
        Name = name;
        Address = address;
        InvoiceAmount = invoiceAmount;
        DateOfSale = dateOfSale;
    }

    // Properties

    public string Name { get; set; } = "";

    public string Address { get; set; } = "";

    public int InvoiceAmount { get; set; } = 0;

    public DateTime DateOfSale { get; set; } = DateTime.Now;

    // Methods

    // Property specific comparison of a given Invoice instance 
    public int CompareTo(Invoice other, string propertyName)
    {
        switch (propertyName.ToLower())
        {
            default:
            case "name":
                return Name.CompareTo(other.Name);
            case "address":
                return Address.CompareTo(other.Address);
            case "invoiceamount":
                return InvoiceAmount - other.InvoiceAmount;
            case "dateofsale":
                return DateOfSale.CompareTo(other.DateOfSale);
        }
    }
    
    // Output convenience method
    public override string ToString()
    {
        return $"{Name}, {Address}, {InvoiceAmount}, {DateOfSale.ToShortDateString()}";
    }

    // Parsing and validation method for converting 5 column input data to the 4 Invoice properties,
    // combining the last and first name columns of the given string in the Name property
    public static bool TryParse(string s, out Invoice result)
    {
        if (s != null && s.Length > 0)
        {
            string[] fields = s.Split(',', StringSplitOptions.TrimEntries);
            // Validate correct number of fields
            if (fields.Length == 5)
            {
                // Process Name
                string name = $"{fields[0]}, {fields[1]}".Trim(' '); // Combine last, first and trim leading, trailing spaces
                if (name.Length == 0 || name == ",")
                {
                    Console.WriteLine($"ERROR: Unable to parse, invalid Name: '{s}'");
                } else {
                    string address = fields[2];
                    if (address.Length == 0)
                    {
                        Console.WriteLine($"ERROR: Unable to parse, invalid Address: '{s}'");
                    } else {
                        // Process InvoiceAmount
                        int amount;
                        if (!int.TryParse(fields[3], out amount))
                        {
                            Console.WriteLine($"ERROR: Unable to parse, invalid Amount: '{s}'");
                        } else {
                            // Process DateOfSale
                            DateTime dateOfSale;
                            if (!DateTime.TryParse(fields[4], out dateOfSale))
                            {
                                Console.WriteLine($"ERROR: Unable to parse, invalid Date Of Sale: '{s}'");
                            } else {
                                result = new Invoice(name, address, amount, dateOfSale);
                                return true;
                            }
                        }
                    }
                }
            } else {
                Console.WriteLine($"ERROR: Unable to parse, missing or extra columns: '{s}'");
            }
        } else {
            Console.WriteLine($"ERROR: Unable to parse, empty or null record string.");
        }
        result = new Invoice();
        return false;
    }
}
