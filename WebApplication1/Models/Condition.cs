using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISTP_LABA_3.Models
{
    public class Condition
    {
        public int ConditionID {get; set;}

        [Required]
        [Display(Name = "Condition name")]
        public string Mark { get; set; }

        [Required]
        [Display(Name ="Max sum")]
        public int MaxSum { get; set; }

        [Required]
        [Display(Name = "Min sum")]
        public int MinSum { get; set; }

        [Display(Name = "Special conditions")]
        public string Special { get; set; }

        ICollection<Offer> Offers { get; set; }
    }
}
