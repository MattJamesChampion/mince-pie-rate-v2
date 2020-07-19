using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MincePieRateV2.ViewModels.Domain
{
    public class MincePieDetailsViewModel : MincePieBaseViewModel
    {
        public string ImagePath { get; set; }
    }
}
