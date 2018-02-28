using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Api.Services;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Core.Repositories;
using Rou.BlogPost.Model.DB;
using Swashbuckle.AspNetCore.Swagger;

namespace Rou.BlogPost.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<BlogPostDbContext> (options =>
                options.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")));
            services.AddScoped<IPostRepository, PostRepository> ();
            services.AddScoped<IBlogRepository, BlogRepository> ();
            services.AddScoped<IBlogService, BlogService> ();
            services.AddScoped<IPostService, PostService> ();
            // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
            services.AddScoped<IUnitOfWork, UnitOfWork> ();
            //Below option added to get rid of Newtonsoft.Json self referencing loop
            services.AddMvc ().AddJsonOptions (options => {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new Info { Title = "BlogPost api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logService) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            app.UseSwagger ();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "BlogPost api V1");
            });
            app.UseMvc ();
        }
    }
}