using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ThinkThank.Portal.Models
{
    public class IEFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            if (userAgent.Contains("Mozilla"))
            {
                context.Result = new RedirectToActionResult("NotFound", "Home", new { statusCode = 404 });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Ничего не делаем после выполнения действия
        }
    }
}
