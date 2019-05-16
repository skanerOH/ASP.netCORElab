using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISTP_LABA_3.Models
{
    public class Contract
    {
        public int ContractID { get; set; }

        public int OfferID { get; set; }

        public int ClientID { get; set; }

        [Required]
        [Display(Name = "Sum")]
        //[Range(0, 9999999999999.99999)]
        public float Sum { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Signing date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SigningDate {get; set;}

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Finish date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FinishDate { get; set; }

        public Offer Offer { get; set; }

        public Client Client { get; set; }
    }
}
