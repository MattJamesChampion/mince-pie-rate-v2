using Microsoft.AspNetCore.Mvc.Filters;
using MincePieRateV2.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MincePieRateV2.DAL.ActionFilters
{
    public class UserInitialiserActionFilter : IAsyncActionFilter
    {
        //https://stackoverflow.com/a/52901541
        private readonly ApplicationDbContext _dbContext;

        public UserInitialiserActionFilter(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string userId = null;
            Guid userGuid = new Guid();

            var claimsIdentity = (ClaimsIdentity)context.HttpContext.User.Identity;

            var userIdClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                userId = userIdClaim.Value;
                
                if (Guid.TryParse(userId, out var parsedGuid))
                {
                    userGuid = parsedGuid;
                }
            }

            _dbContext.UserId = userGuid;

            var resultContext = await next();
        }
    }
}
