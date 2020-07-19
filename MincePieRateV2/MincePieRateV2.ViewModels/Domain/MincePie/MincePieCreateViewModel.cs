using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MincePieRateV2.ViewModels.Domain
{
    public class MincePieCreateViewModel : MincePieBaseViewModel
    {
        public IFormFile Image { get; set; }
    }
}
