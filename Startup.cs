using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop.Data;

namespace Shop
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
         // API Doc
         services.AddCors();
         // Compreesão da API
         services.AddResponseCompression(options =>
         {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
         });

         // Cachea toda a aplicação
         //services.AddResponseCaching();
         services.AddControllers();
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
         });

         // Autenticação do token
         var key = Encoding.ASCII.GetBytes(Settings.Secret);

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
               IssuerSigningKey = new SymmetricSecurityKey(key),
               ValidateIssuer = false,
               ValidateAudience = false
            };
         });

         // Dependency Injection for DataBase in Memory with name Shop
         services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Shop"));
         //services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));
         // Similar to Using, creates a connection to the BD and then closes
         // services.AddScoped<DataContext, DataContext>();
         /*
                  services.AddSwaggerGen(c =>
                  {
                     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop Api", Version = "v1" });
                  });

                  */
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            // Documentação da api
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop v1"));
         }

         app.UseHttpsRedirection();
         // Documentação da api

         app.UseRouting();

         // Documentação da api
         app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

         // Autenticação
         app.UseAuthentication();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}
