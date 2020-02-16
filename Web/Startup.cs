using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web
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
            services.AddHostedService<ValidateOptionsService>();

            var pagingOptionsLimit = Configuration.GetValue<int>("DefaultPagingOptions:limit");

            var pagingOptionsSection = Configuration.GetSection("DefaultPagingOptions");

            var pageOptionsStringValue = pagingOptionsSection["limit"];
            var pageOptionsLimitValue = pagingOptionsSection.GetValue<int>("limit");

            var stronglyTypedPagingOptions = new DefaultPagingOptions();
            Configuration.Bind("DefaultPagingOptions", stronglyTypedPagingOptions);

            services.Configure<DefaultPagingOptions>(Configuration.GetSection("DefaultPagingOptions"));
            services.Configure<FeatureSettings>(Configuration.GetSection("FeatureSettings"));

            services.AddOptions<ValidateSettings>().Bind(Configuration.GetSection("ValidateSettings")).ValidateDataAnnotations();

            if (Configuration.GetValue<bool>("CloudStorage"))
            {
                services.AddTransient<IFileService, AzureFileService>();
            }
            else
            {
                services.AddTransient<IFileService, LocalDiskFileService>();
            }

            services.AddSingleton<MonitorService, MonitorService>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
