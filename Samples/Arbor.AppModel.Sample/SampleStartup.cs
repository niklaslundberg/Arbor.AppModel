using Arbor.AppModel.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Sample;

public class SampleStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddMvc(options => options.Filters.Add<ValidationActionFilter>());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
    {
        app.UseRouting();

        app.UseEndpoints(builder => builder.MapControllers());
    }
}