using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace aspnetcore
{
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    IConfiguration configuration;
    IHostingEnvironment env;

    public ValuesController(IHostingEnvironment env, IConfiguration configuration)
    {
      this.env = env;
      this.configuration = configuration;
    }

    [HttpGet]
    [Route("~/")]
    public string Home()
    {
      return "Hello World!";
    }

    [HttpGet]
    [Route("~/api/env")]
    public string GetEnv()
    {
      return this.env.EnvironmentName;
    }

    [HttpGet]
    [Route("~/api/hosting")]
    public string GetHosting()
    {
      var setting = configuration.GetSection("urls");
      var uri = new Uri(setting.Value);

      return setting.Value;
    }

    [HttpGet]
    [Route("~/api/port")]
    public int GetPort()
    {
      var setting = configuration.GetSection("urls");
      var uri = new Uri(setting.Value);
      return uri.Port;
    }

  }
  
}
