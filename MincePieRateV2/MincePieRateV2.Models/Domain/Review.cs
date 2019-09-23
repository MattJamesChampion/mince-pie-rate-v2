using MincePieRateV2.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MincePieRateV2.Models.Domain
{
    public class Review : ModelMetadata
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Keys
        public MincePie MincePie { get; set; }

        //References
        [DisplayName("Mince Pie")]
        public int MincePieId { get; set; }

        //Properties
        [DisplayName("Pastry Rating")]
        public int PastryRating { get; set; }
        [DisplayName("Filling Rating")]
        public int FillingRating { get; set; }
        [DisplayName("Appearance Rating")]
        public int AppearanceRating { get; set; }
        [DisplayName("Aroma Rating")]
        public int AromaRating { get; set; }
        [DisplayName("Value for Money")]
        public int ValueForMoney { get; set; }
        [DisplayName("Free-text Review")]
        public string FreeTextReview { get; set; }
    }
}
