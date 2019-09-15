using MincePieRateV2.DAL.Data;
using MincePieRateV2.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MincePieRateV2.DAL.Repositories
{
    public class MincePieRepository: BaseRepository<MincePie>
    {
        public MincePieRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
