using ArkEcho.Core;
using ArkEcho.RazorPage;
using ArkEcho.RazorPage.Data;
using ArkEcho.WebPage;
using ArkEcho.WebPage.Data;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ArkEcho.WA
{
    /* TODO
	 * Anzeige
			-> Interpreten A-Z -> Doppel & dreifach Interpreten zusammenfassen
			-> Album A-Z
			-> Titel A-Z
			-> Ordner	-> Baumstruktur, ganzen Ordner abspielen
	*/
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault();

            AppEnvironment environment = new AppEnvironment(Resources.ARKECHOWEBPAGE, builder.HostEnvironment.IsDevelopment(), Resources.Platform.Web, false);

            builder.RootComponents.Add<ArkEchoApp>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazoredLocalStorageAsSingleton(); // For WebLocalStorage
            builder.Services.AddArkEchoServices<WebLocalStorage, WebAppModel>(environment);

            await builder.Build().RunAsync();
        }
    }
}