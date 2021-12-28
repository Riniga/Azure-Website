// See https://aka.ms/new-console-template for more information
using AzureWebsite.Library.Inkasso;
Console.WriteLine("Nisse Inkasso");

PersonManager.SeedPersons();

Person last=new Person();
Console.WriteLine("List all Persons");
var personer = PersonManager.GetPersons();
foreach (var person in personer)
{
    Console.WriteLine(person.Id+ ":" + person.Name);
    last = person;
}

Console.WriteLine("Update person");
last.Name = "Rickard Nisses-Gagnér";
PersonManager.UpdatePerson(last);


Console.WriteLine("Get person by Id");
var individual = PersonManager.GetPerson(last.Id);
Console.WriteLine(individual.Id + ":" + individual.Name);



