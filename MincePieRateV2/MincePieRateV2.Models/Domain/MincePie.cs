using MincePieRateV2.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MincePieRateV2.Models.Domain
{
    public class MincePie : ModelMetadata
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Brand} - {Name}";
        }
        [NotMapped]
        public string DisplayString => ToString();
    }
}
