using NUnit.Framework;
using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayRight.Demo.AutomationBase
{
    public abstract class BaseDriver
    {
        protected static IBrowser Browser { get; private set; }
        protected static IBrowserContext Context { get; private set; }
        protected static IPage CurrentPage { get; private set; }

        public BaseDriver()
        {
            if (Browser is null)
            {
                Task.Run(async () => await InitComponents()).Wait();
            }
        }

        [SetUp]
        protected async Task Navigate()
        {
            await Task.Run(async () =>
            {
                await CurrentPage.GoToAsync("https://www.google.com", waitUntil: LifecycleEvent.Load);
            });
        }

        [TearDown]
        protected async Task Flush()
        {
            await Task.Run(async () =>
            {
                await Browser.CloseAsync();
            });
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
            var context =  browser.Result.NewContextAsync();
            var page    =  context.Result.NewPageAsync();

            await Task.WhenAll(browser, context, page);

            Browser     = browser.Result;
            Context     = context.Result;
            CurrentPage = page.Result;
        }
    }
}
