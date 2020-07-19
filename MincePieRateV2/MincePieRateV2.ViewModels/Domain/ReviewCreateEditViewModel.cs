using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MincePieRateV2.ViewModels.Domain
{
    public class ReviewCreateEditViewModel
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Keys
        [DisplayName("Mince Pie")]
        public MincePieDetailsViewModel MincePie { get; set; }

        //References
        [DisplayName("Mince Pie")]
        [Required]
        public int MincePieId { get; set; }
        public IEnumerable<MincePieDetailsViewModel> MincePies { get; set; }

        //Properties
        [DisplayName("Pastry Rating")]
        [Required]
        [Range(1, 10)]
        public int PastryRating { get; set; }
        [DisplayName("Filling Rating")]
        [Required]
        [Range(1, 10)]
        public int FillingRating { get; set; }
        [DisplayName("Appearance Rating")]
        [Required]
        [Range(1, 10)]
        public int AppearanceRating { get; set; }
        [DisplayName("Aroma Rating")]
        [Required]
        [Range(1, 10)]
        public int AromaRating { get; set; }
        [DisplayName("Value for Money")]
        [Required]
        [Range(1, 10)]
        public int ValueForMoney { get; set; }
        [DisplayName("Free-text Review")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500)]
        public string FreeTextReview { get; set; }
    }
}
