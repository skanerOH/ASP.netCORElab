using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISTP_LABA_3.Models
{
    public class Offer
    {
        public int OfferID { get; set; }

        public int BankID { get; set; }

        public int ConditionID { get; set; }

        public int OfferTypeID { get; set; }

        [Required]
        [Display(Name ="Offer Name")]
        public string Name { get; set; }
        [Required]
        [Range(1,1000)]
        [Display(Name ="Percentage")]
        public int Percentage { get; set; }

        public Condition Condition { get; set; }

        public OfferType OfferType { get; set; }

        public Bank Bank { get; set; }

        public ICollection<Contract> Contracts { get; set; }


    }
}
