using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISTP_LABA_3.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Client Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Contact phone number")]
        [RegularExpression(@"\d{7,10}")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Client type")]
        [RegularExpression(@"Juridical|Physical")]
        public string ClientType { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
