using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class AdminMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path.ToString();
            var role = context.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role);

            List<string> methods = new()
            {
                "PATCH",
                "PUT",
                "POST",
                "DELETE"
            };
            List<string> paths = new List<string>();
            paths.Add("/api/activities");
            paths.Add("/api/categories");
            paths.Add("/api/comments");
            paths.Add("/api/contacts");
            paths.Add("/api/members");
            paths.Add("/api/news");
            paths.Add("/api/organizations");
            paths.Add("/api/slides");
            paths.Add("/api/testimonials");
            paths.Add("/api/user");
            var s = methods.Contains(method.ToLower());
            var j = paths.Contains(path.ToLower());
            if (methods.Contains(method) && paths.Contains(path.ToLower()))
            {
                if (!context.User.IsInRole("Admin"))
                {
                    context.Response.StatusCode = 403;
                    return;
                }
            }
            await _next.Invoke(context);

        }
    }

}