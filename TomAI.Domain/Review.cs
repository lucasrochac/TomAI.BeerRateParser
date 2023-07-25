using System.ComponentModel.DataAnnotations;

namespace TomAI.Domain
{
    public class Review
    {
        [Display(Name = "review/appearance")]
        public double Appearance { get; set; }

        [Display(Name = "review/aroma")]
        public double Aroma { get; set; }

        [Display(Name = "review/palate")]
        public double Palate { get; set; }

        [Display(Name = "review/taste")]
        public double Taste { get; set; }

        [Display(Name = "review/overall")]
        public double Overall { get; set; }

        [Display(Name = "review/time")]
        public long Time { get; set; }

        [Display(Name = "review/profileName")]
        public string ProfileName { get; set; }

        [Display(Name = "review/text")]
        public string Text { get; set; }
    }
}
