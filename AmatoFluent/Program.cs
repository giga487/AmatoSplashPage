using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using AmatoFluent.ViewModels;
using AmatoFluent.ViewModels.SpaceInvaders;

namespace AmatoFluent
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped((IServiceProvider sp) => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddFluentUIComponents();
            
			builder.Services.AddTransient<BouncingBallsViewModel>();
			builder.Services.AddTransient<SpaceInvadersViewModel>();

			await builder.Build().RunAsync();
		}
	}
}
