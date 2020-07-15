using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MincePieRateV2.ViewModels.Domain
{
    public class MincePieDisplayViewModel
    {
            public int Id { get; set; }
            [Required]
            [MaxLength(50)]
            public string Brand { get; set; }
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }
            public override string ToString()
            {
                return $"{Brand} - {Name}";
            }
            [NotMapped]
            public string DisplayString => ToString();

            public IFormFile Image { get; set; }
    }
}
