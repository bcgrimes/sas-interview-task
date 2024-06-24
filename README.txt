README SAS SSE Interview Exercise

REQUIREMENTS

Role-based Activity Interview/Presentation 

Instructions: Create a simple application in the language of your choosing (preference for Java, HTML + Javascript, Python, or GoLang)
The application should, at a minimum, do the following:
1 Read a csv file with the following columns: Name, Address, Invoice Amount, Date of Sale
2 Accept user input, either as a start parameter to the program or interactive in the application, to indicate which column to sort
3 Using your own algorithm, don't use the sorting functions built into the language, provide a program that sorts the data by the given column
4 Also allow sorting by two columns at once.

Prior to the interview, make the code available by emailing it to
jeff.miller@sas.com

Come to the interview prepared to discuss the program including how you performed the sort, alternate methods, and other general questions.
Sample data:
Name, Address, Invoice Amount, Date of Sale
Doe, John, 123 Main St., 1275, 3/19/2021
Smith, Jack, 75 Elm St., 750, 3/22/2021
Jones, Jane, 559 5th Ave., 2250, 5/12/2020
Brown, Brad, 123 Main St., 1890, 5/12/2021

You can complete the activity here: https://rndconfluence.sas.com/display/EDMVIYA/Interview+programming+task


SOLUTION NOTES

This solution is implemented as a console program using C# and .Net Core 8, built on a Windows 10 Intel laptop
using Visual Studio Code, with no coding assist features other than the 'dotnet' CLI and syntax and formatting extensions.
It consists of 3 class files:

Program.cs - implements the data load and user interaction
Invoice.cs - a record class that implements data properties and support methods for data loading and comparison
SortedListBuilder.cs - a simple builder class that implements the methods for sorting a given array 

The program optionally accepts a data file as a command line argument. Running the program without a file argument 
will load the default data file as long as it is in the same directory as the program execuatble. 
It is recommended to run the program directly from the data file directory. (See OPERATION)

All designation of sorting options is gathered as user input from the command line in the console. (See SAMPLE RESULTS)
The sorting options are: Primary Sort Column, Secondary Sort Column, Sort Direction (Ascending or Descending)

4 CSV data files are included:

sample-data.csv - provided sample data set
test-data-empty.csv - an empty data file
test-data-missingheader.csv - a copy of sample-data.csv with the header removed
test-data-validation.csv - a file to test the corrupted file conditions described in the Design Assumptions section.

DESIGN NOTES

This solution uses a Quicksort (https://en.wikipedia.org/wiki/Quicksort) implementation.

Design Assumptions:
1. The mismatch between the header columns and the test data columns is intentional,
   requiring the separate last and first name columns to be treated as one.
   NOTE: having 1 of the first or last name columns being empty, but not both, is considered acceptable. 
2. The program should ignore corrupted data entries, except where the corruption prevents further processing.
   Corrupted data file conditions:
  FATAL, processing halts
   a. missing header line
   b. header line that does not have 4 columns
  NON-FATAL, processing continues, skipping the corrupted line
   c. data line that does not have 5 columns
   d. data line with empty columns for both last and first name
   e. data line with one or more empty columns (excluding the case of either the first or last name being present)
   f. a blank line
3. Sorting order for primary and secondary columns is the same.
4. The program operates on a single data file at a time.


OPERATION

To run the program:

Default data file used:
C:\sas-interview-task>.\bin\Debug\net8.0\sas-interview-task.exe

Data file specified:
C:\sas-interview-task>.\bin\Debug\net8.0\sas-interview-task.exe .\sample-data.csv


SAMPLE RESULTS

C:\Users\Brett\Documents\Projects\SAS\sas-interview-task>dotnet run ".\sample-data.csv"
Hello, SAS!

Loading invoices data file: '.\sample-data.csv'

Name, Address, Invoice Amount, Date of Sale
Doe, John, 123 Main St., 1275, 3/19/2021
Smith, Jack, 75 Elm St., 750, 3/22/2021
Jones, Jane, 559 5th Ave., 2250, 5/12/2020
Brown, Brad, 123 Main St., 1890, 5/12/2021

Do you wish to sort the data? [Y]|N

> Enter primary sort column index: 0 - Name, 1 - Address, 2 - InvoiceAmount, 3 - DateofSale
0
> Enter primary sort order, ascending or descending: [A]|D
d

Do you wish to add a secondary sort column? [Y]|N

> Enter secondary sort column index: 0 - Name, 1 - Address, 2 - InvoiceAmount, 3 - DateofSale
6
WARNING: invalid secondary column index, skipping secondary sort.
Primary Sort Column: Name
Primary Sort Order:  DESC
Smith, Jack, 75 Elm St., 750, 3/22/2021
Jones, Jane, 559 5th Ave., 2250, 5/12/2020
Doe, John, 123 Main St., 1275, 3/19/2021
Brown, Brad, 123 Main St., 1890, 5/12/2021

Do you wish to sort the data? [Y]|N
N

C:\Users\Brett\Documents\Projects\SAS\sas-interview-task>dotnet run ".\test-data-validation.csv"
Hello, SAS!

Loading invoices data file: '.\test-data-validation.csv'

Name, Address, Invoice Amount, Date of Sale
ERROR: Unable to parse, missing or extra columns: 'Doe John, 123 Main St., 1275, 3/19/2021'
ERROR: Unable to parse, invalid Name: ',, 321 Spring St., 6540, 4/1/2021 '
ERROR: Unable to parse, invalid Address: 'House, Barry, , 750, 3/22/2021'
ERROR: Unable to parse, invalid Amount: 'Smith, Jack, 75 Elm St., $750, 3/22/2021'
ERROR: Unable to parse, invalid Amount: 'Green, Aaron, 2275 Willow St., , 3/22/2021'
ERROR: Unable to parse, invalid Amount: 'Brown, Jim, 57 Oak Dr., 850.00, 3/4/2021'
ERROR: Unable to parse, invalid Date Of Sale: 'Jones, Jane, 559 5th Ave., 2250, '
ERROR: Unable to parse, invalid Date Of Sale: 'James, Jerry, 955 1th Ave., 2250, 22/3/2021'
ERROR: Unable to parse, empty or null record string.
ERROR: Unable to parse, missing or extra columns: 'Brown, Brad, 123 Main St., 1890, 5/12/2021,'
ERROR: Unable to parse, missing or extra columns: 'Baker, Charlie, , 1890,,'
Dohr,, 900 Chestnut Ave., 6540, 4/1/2021
, Elsa, 101 Windy Ct., 6540, 4/1/2021

Do you wish to sort the data? [Y]|N
n

C:\Users\Brett\Documents\Projects\SAS\sas-interview-task>dotnet run ".\test-data-missingheader.csv" 
Hello, SAS!

Loading invoices data file: '.\test-data-missingheader.csv'

ERROR: invalid data file, incorrect header.

C:\Users\Brett\Documents\Projects\SAS\sas-interview-task>dotnet run ".\test-data-empty.csv"
Hello, SAS!

Loading invoices data file: '.\test-data-empty.csv'

ERROR: invalid data file, incorrect header.

