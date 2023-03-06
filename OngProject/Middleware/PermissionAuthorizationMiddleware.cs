using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class PermissionAuthorizationMiddleware
    {

        private readonly RequestDelegate _next;

        public PermissionAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var userId = context.Request.Query["id"];
                var path = context.Request.Path.ToString();
                var userPath = "/api/user";

                if (path.ToLower() == userPath)
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (string.IsNullOrEmpty(token))
                    {
                        throw new Exception("Token invalido");
                    }
                    var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                    if (!context.User.IsInRole("Admin"))
                    {
                        if (!securityToken.Claims.Any(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == userId))
                        {
                            throw new Exception("El usuario que intentas modificar no es el propio");
                        }
                    }
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var result = JsonConvert.SerializeObject(ex.Message, settings);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(result);
            }
        }
    }
}
