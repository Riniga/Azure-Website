using AzureWebsite.Library.Inkasso;

DateTime start = DateTime.Now;
await Contracts.SeedContractsAsync(10, File.ReadAllText("companies.txt"));

await Persons.SeedPersonsAsync(10, File.ReadAllText("persons.txt"));

Console.WriteLine($"Execution time: {(int)(DateTime.Now-start).TotalMilliseconds}");

