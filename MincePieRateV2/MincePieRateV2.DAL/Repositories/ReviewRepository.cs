﻿using MincePieRateV2.DAL.Data;
using MincePieRateV2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MincePieRateV2.DAL.Repositories
{
    public class ReviewRepository : BaseRepository<Review>
    {
        public ReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
