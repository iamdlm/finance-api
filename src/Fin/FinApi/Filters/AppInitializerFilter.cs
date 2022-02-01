using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinApi.Filters
{
    public class AppInitializerFilter : IAsyncActionFilter
    {
        private readonly FinDbContext dbContext;

        public AppInitializerFilter(FinDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid userId = Guid.Empty;

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.HttpContext.User.Identity;

            if(claimsIdentity != null)
            {
                Claim userIdClaim = claimsIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userId = new Guid(userIdClaim.Value);
                }
            }

            dbContext.UserId = userId;

            await next();
        }
    }
}
