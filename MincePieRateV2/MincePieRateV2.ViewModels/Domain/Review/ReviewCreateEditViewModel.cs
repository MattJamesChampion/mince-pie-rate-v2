using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MincePieRateV2.ViewModels.Domain
{
    public class ReviewCreateEditViewModel : ReviewBaseViewModel
    {
        public IEnumerable<MincePieDetailsViewModel> MincePies { get; set; }
    }
}
