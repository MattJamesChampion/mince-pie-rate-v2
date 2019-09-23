using Microsoft.EntityFrameworkCore;
using MincePieRateV2.DAL.Data;
using MincePieRateV2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MincePieRateV2.DAL.Repositories
{
    public class ReviewRepository : BaseRepository<Review>
    {
        public ReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        private IQueryable<Review> GetTableWithIncludes()
        {
            return Table.Include(r => r.MincePie);
        }

        public override Review GetEntity(Func<Review, bool> predicate)
        {
            var tableWithIncludes = GetTableWithIncludes();

            return tableWithIncludes.Where(predicate).FirstOrDefault();
        }

        public override IEnumerable<Review> GetEntities(Func<Review, bool> predicate = null)
        {
            var tableWithIncludes = GetTableWithIncludes();

            if (predicate != null)
            {
                return tableWithIncludes.Where(predicate);
            }
            else
            {
                return tableWithIncludes;
            }
        }
    }
}
