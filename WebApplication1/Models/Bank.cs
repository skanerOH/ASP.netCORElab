using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISTP_LABA_3.Models
{
    public class Bank
    {
        public int BankID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name="Bank Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Address")]
        //[RegularExpression(@"^[A-Z]+.+[a-zA-Z""'\s-]*$")]
        public string Address { get; set; }

        public ICollection<Offer> Offers { get; set; }

    }
}
