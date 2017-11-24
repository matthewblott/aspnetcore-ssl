using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcore
{
  public class Startup
  {
    public IHostingEnvironment HostingEnvironment { get; }

    IConfigurationRoot Configuration;

    public Startup(IHostingEnvironment env)
    {
      HostingEnvironment = env;

			var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
      
			Configuration = builder.Build();

    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
      services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
      app.UseMvc();

    }

  }
  
}