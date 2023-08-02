using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TomAI.Domain
{
    public class BeerReview
    {
        public BeerReview() 
        {
            Beer = new Beer();
            Review = new Review();
        }

        [Display(Name = "review")]
        public Review Review { get; set; }
        
        [Display(Name = "beer")]
        public Beer Beer { get; set; }
    }
}