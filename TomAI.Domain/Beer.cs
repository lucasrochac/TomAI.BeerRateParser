using System.ComponentModel.DataAnnotations;

namespace TomAI.Domain
{
    public class Beer
    {
        [Display(Name = "name")]
        public string Name { get; set; }

        [Display(Name = "beerId")]
        public int BeerId { get; set; }

        [Display(Name = "brewerId")]
        public int BrewerId { get; set; }

        [Display(Name = "ABV")]
        public double ABV { get; set; }

        [Display(Name = "style")]
        public string Style { get; set; }
    }
}
