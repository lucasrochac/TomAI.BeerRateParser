using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TomAI.Domain;

string filePath = "C:\\Users\\Integer\\Desktop\\ratebeer_small.txt";
var reviews = new List<BeerReview>();
var currentBeerReview = new BeerReview();

string[] lines = File.ReadAllLines(filePath);


foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        if (currentBeerReview != null)
        {
            reviews.Add(currentBeerReview);
            currentBeerReview = null;
        }
    }
    else
    {
        var parts = line.Split(':');
        if (parts.Length == 2)
        {
            var key = parts[0].Trim();
            var value = parts[1].Trim();

            if (currentBeerReview == null)
            {
                currentBeerReview = new BeerReview();
            }

            if (!TrySetPropertyValue(currentBeerReview, key, value))
            {
                // Lidar com propriedade desconhecida ou erro de mapeamento, se necessário
            }
        }
    }
}

if (currentBeerReview != null)
{
    reviews.Add(currentBeerReview);
    Console.WriteLine(string.Format("Beer: {0} | Score: {1}", currentBeerReview.Beer.Name, currentBeerReview.Review.Overall));
}

bool TrySetPropertyValue(BeerReview beerReview, string key, string value)
{
    var memberName = key.Replace('/', '.'); 
    var validationResults = new List<ValidationResult>();

    var proparray = key.Split('/');

    Type type = GetClassType(ConvertToClassName(proparray[0]));
    var obj = InstantiateClass(type);
    var context = new ValidationContext(obj);
    
    foreach(var p in type.GetProperties())
    {
        var displayAttribute = p.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null && displayAttribute.Name == proparray[1])
        {
            var convertedValue = Convert.ChangeType(value, p.PropertyType);
            p.SetValue(obj, convertedValue);
        }
    }   

    return false;
}

Type GetClassType (string typeName)
{
    return Type.GetType($"TomAI.Domain.{typeName}, TomAI.Domain");
}

object InstantiateClass (Type type)
{
    return Activator.CreateInstance(type);
}

string ConvertToClassName(string className)
{
    return char.ToUpper(className[0]) + className.Substring(1);
}
