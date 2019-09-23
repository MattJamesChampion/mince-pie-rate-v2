using MincePieRateV2.Models.Metadata;
using System;
using System.Collections.Generic;
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
        public int MincePieId { get; set; }

        //Properties
        public int PastryRating { get; set; }
        public int FillingRating { get; set; }
        public int AppearanceRating { get; set; }
        public int AromaRating { get; set; }
        public int ValueForMoney { get; set; }
        public string FreeTextReview { get; set; }
    }
}
