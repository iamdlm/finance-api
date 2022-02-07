using Fin.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fin.Api.Filters
{
    public class ContextUserIdFilter : IAsyncActionFilter
    {
        private readonly FinDbContext dbContext;

        public ContextUserIdFilter(FinDbContext dbContext) => this.dbContext = dbContext;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid userId = Guid.Empty;

            string claimsIdentity = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(claimsIdentity))
            {
                userId = new Guid(claimsIdentity);
            }

            dbContext.UserId = userId;

            await next();
        }
    }
}
