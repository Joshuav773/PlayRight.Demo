using NUnit.Framework;
using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayRight.Demo.AutomationBase
{
    public abstract class BaseDriver
    {
        protected static IBrowser Browser { get; private set; }
        protected static IBrowserContext Context { get; private set; }
        protected static IPage CurrentPage { get; private set; }

        private string _videosDir
        {
            get
            {
                var projectPath = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(x => x.FullName.Contains("PlayRight.Demo"))
                    .Location;

                var toRemove = projectPath.IndexOf("bin");
                return $@"{ projectPath.Remove(toRemove) }Videos\";
            }
        }

        protected BaseDriver()
        {
            if (Browser is null)
            {
                Task.Run(async () => await InitComponents()).Wait();
            }
        }

        [SetUp]
        protected async Task Navigate()
        {
            await CurrentPage.GoToAsync("https://www.google.com", waitUntil: LifecycleEvent.Load);
        }

        [TearDown]
        protected async Task Flush()
        {
            await Context.CloseAsync();
            await Browser.CloseAsync();
        }

        private async Task<IBrowser> GetBrowserAsync()
        {
            await Playwright.InstallAsync();
            var playwright = await Playwright.CreateAsync();

            return await playwright
                    .Chromium
                    .LaunchAsync(headless: false)
                    ;
        }

        private async Task InitComponents()
        {
            var browser =  GetBrowserAsync();

            var context =  browser.Result.NewContextAsync
                (
                    recordVideo: new RecordVideoOptions
                    {
                        Dir = _videosDir,

                        Size = new ViewportSize
                        {
                            Width = 1920,
                            Height = 1080
                        }
                    }
                );

            var page    =  context.Result.NewPageAsync();

            await Task.WhenAll(browser, context, page);

            Browser     = browser.Result;
            Context     = context.Result;
            CurrentPage = page.Result;
        }
    }
}
