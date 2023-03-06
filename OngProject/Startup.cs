using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OngProject.Core.Business;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.DataAccess;
using OngProject.Entities;
using OngProject.Middleware;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AmazonS3Client = OngProject.Core.Business.AmazonS3Client;

namespace OngProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OngDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OngConnection")));
            services.AddControllers();

            services.AddScoped<IUsersBusiness, UsersBusiness>();
            services.AddScoped<IActivitiesBusiness, ActivitiesBusiness>();
            services.AddScoped<IContactsBusiness, ContactsBusiness>();
            services.AddScoped<IMembersBusiness, MembersBusiness>();
            services.AddScoped<INewsBusiness, NewsBusiness>();
            services.AddScoped<IOrganizationsBusiness, OrganizationsBusiness>();
            services.AddScoped<IRolesBusiness, RolesBusiness>();
            services.AddScoped<ICategoriesBusiness, CategoriesBusiness>();
            services.AddScoped<ISlidesBusiness, SlidesBusiness>();
            services.AddScoped<ITestimonialsBusiness, TestimonialsBusiness>();
            services.AddScoped<ICommentsBusiness, CommentsBusiness>();
            services.AddScoped<IAuthBusiness, AuthBusiness>();
            services.AddScoped<ISendGridBusiness, SendGridHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Make sure only a HttpContextAccessor is available in the whole project.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // AWS stuff.
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IAmazonS3Client, AmazonS3Client>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt:key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OngProject", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autenticación JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.RoutePrefix = "api/docs";
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "OngProject v1");               
                });
            }

            app.UseMiddleware<PermissionAuthorizationMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AdminMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
