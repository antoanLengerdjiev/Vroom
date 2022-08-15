using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vroom.Data.Common;
using Microsoft.AspNetCore.Identity;
using Vroom.Data.Contracts;
using Vroom.MappingProfiles;
using Vroom.Providers.Contracts;
using Vroom.Providers;
using Vroom.Data.Models;
using Vroom.Service.Contracts;
using Vroom.Service.Data;
using Vroom.Service.Data.MappingProfiles;
using Vroom.MappingProfiles.Resolvers;
using Vroom.Providers.EncodingHelpers;
using Vroom.Models.Factories.NewFolder;
using Vroom.Models.Factories;

namespace Vroom
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
            services.AddAutoMapper(typeof(WebMappingProfile), typeof(ServiceMappingProfile));
            services.AddDbContext<VroomDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("VroomDatabase")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)       
                .AddEntityFrameworkStores<VroomDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<IDBInitializer, DBInitializer>();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.AreaViewLocationFormats.Clear();
            //    options.AreaViewLocationFormats.Add("/MyArea/{2}/Views/{1}/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/MyArea/{2}/Views/Shared/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            //});
            services.AddControllersWithViews();
            //services.AddCloudscribePagination();
            services.AddHttpContextAccessor();
            services.AddTransient<IHttpContextProvider, HttpContextProvider>();
            services.AddScoped<IEncodingProvider, EncodingProvider>();
            services.AddScoped<IWebEncodersProvider, WebEncodersProvider>();
            services.AddScoped<IApplicationSignInManager<ApplicationUser>, ApplicationSignInManager<ApplicationUser>>();
            services.AddScoped<IApplicationRoleManager<IdentityRole>, ApplicationRoleManager<IdentityRole>>();
            services.AddScoped<IApplicationUserManager<ApplicationUser>, ApplicationUserManager<ApplicationUser>>();
            services.AddScoped<IVroomDbContextSaveChanges>(provider => provider.GetService<VroomDbContext>());
            services.AddScoped<IVroomDbContext>(provider => provider.GetService<VroomDbContext>());
            services.AddScoped(typeof(IEfDbRepository<>), typeof(EfDbRepository<>));
            services.AddScoped<IMakeService, MakeService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IBikeService, BikeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IIOProvider, IOProvider>();
            services.AddSingleton<UniqueCode>();
            services.AddScoped<ISelectedItemFactory, SelectedItemFactory>();
            services.AddSingleton<IEncryptionProvider, EncryptionDataProtectorProvider>();
            services.AddTransient<IdEncodeResolver>();
            services.AddTransient<IdDecodeResolver>();
            services.AddTransient<MakeIdEncodeResolver>();
            services.AddTransient<MakeIdDecodeResolver>();
            services.AddTransient<ModelIdEncodeResolver>();
            services.AddTransient<ModelIdDecodeResolver>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDBInitializer dBInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            dBInitializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                     name: "MyArea",
                     areaName: "Administration",
                     pattern: "Administration/{controller=Home}/{action=Index}/{id?}"
                   );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Bike}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                
            });

        }
    }
}
