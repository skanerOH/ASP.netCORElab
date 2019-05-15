using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISTP_LABA_3.Models
{
    public class OfferType
    {
        public int OfferTypeID { get; set; }

        [Required]
        [Display(Name = "Offer type")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public ICollection<Offer> Offers { get; set; }
    }
}
