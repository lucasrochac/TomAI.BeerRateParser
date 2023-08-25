using TomAI.Domain;
using TomAI.RabbitConnector.RabbitConnector;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using Newtonsoft.Json;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "C:\\Users\\Integer\\Desktop\\ratebeer" + ".txt";

        var lines = File.ReadAllLines(filePath);
        var reviewlines = new List<string>();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                var beerReview = ParseBeerReview(reviewlines);                
                
                if(beerReview.Beer.BeerId == 0) { 
                    continue; 
                } else {
                    Console.WriteLine(string.Format("BeerName: {0} - {1} | BeerOverall: {2}", beerReview.Beer.Name, beerReview.Beer.Style, beerReview.Review.Overall));
                    SendRabbitMessage(beerReview);
                    reviewlines.Clear();
                    continue;
                }
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
                    beerReview.Beer.Name = WebUtility.UrlDecode(value);
                    break;
                case "beer/beerId":
                    beerReview.Beer.BeerId = ValidateIds(value);
                    break;
                case "beer/brewerId":
                    beerReview.Beer.BrewerId = ValidateIds(value);
                    break;
                case "beer/ABV":
                    beerReview.Beer.ABV = ValidateIds(value);
                    break;
                case "beer/style":
                    beerReview.Beer.Style = WebUtility.UrlDecode(value);
                    break;
                case "review/appearance":
                    beerReview.Review.Appearence = ConvertValueResult(value);
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
                    beerReview.Review.Time = value != "-" ? long.Parse(value) : 0;
                    break;
                case "review/profileName":
                    beerReview.Review.ProfileName = WebUtility.UrlDecode(value);
                    break;
                case "review/text":
                    beerReview.Review.Text = WebUtility.UrlDecode(value);
                    break;
            }
        }
        return beerReview;
    }

    private static int ValidateIds(string value)
    {
        int id = 0;
        int.TryParse(value, out id);
        return id;
    }
}