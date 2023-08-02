using TomAI.Domain;
using TomAI.RabbitConnector.RabbitConnector;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using Newtonsoft.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "C:\\Users\\Integer\\Desktop\\ratebeer_small.txt";

        var lines = File.ReadAllLines(filePath);
        var reviewlines = new List<string>();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                var beerReview = ParseBeerReview(reviewlines);
                Console.WriteLine(string.Format("BeerName: {0} | BeerOverall: {1}", beerReview.Beer.Name, beerReview.Review.Overall));

                SendRabbitMessage(beerReview);
                reviewlines.Clear();
                continue;
            }
            else
            {
                reviewlines.Add(line);
            }
        }

    }

    private static async void SendRabbitMessage(BeerReview review)
    {   
        var json = JsonConvert.SerializeObject(review);
        IRabbitMQService rabbitMqService = new RabbitMQService();
        rabbitMqService.SendMessage(json);
    }


    private static double ConvertValueResult(string value)
    {
        var splitted = value.Split('/');
        return Convert.ToDouble(splitted[0]);
    }

    private static BeerReview ParseBeerReview(List<string> reviewlines)
    {
        var beerReview = new BeerReview();
        foreach (var line in reviewlines)
        {
            var parts = line.Split(':');

            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();

            switch (key)
            {
                case "beer/name":
                    beerReview.Beer.Name = value;
                    break;
                case "beer/beerId":
                    beerReview.Beer.BeerId = int.Parse(value);
                    break;
                case "beer/brewerId":
                    beerReview.Beer.BrewerId = int.Parse(value);
                    break;
                case "beer/ABV":
                    beerReview.Beer.ABV = double.Parse(value);
                    break;
                case "beer/style":
                    beerReview.Beer.Style = value;
                    break;
                case "review/appearance":
                    beerReview.Review.Appearance = ConvertValueResult(value);
                    break;
                case "review/aroma":
                    beerReview.Review.Aroma = ConvertValueResult(value);
                    break;
                case "review/palate":
                    beerReview.Review.Palate = ConvertValueResult(value);
                    break;
                case "review/taste":
                    beerReview.Review.Taste = ConvertValueResult(value);
                    break;
                case "review/overall":
                    beerReview.Review.Overall = ConvertValueResult(value);
                    break;
                case "review/time":
                    beerReview.Review.Time = long.Parse(value);
                    break;
                case "review/profileName":
                    beerReview.Review.ProfileName = value;
                    break;
                case "review/text":
                    beerReview.Review.Text = value;
                    break;
            }
        }
        return beerReview;
    }
}