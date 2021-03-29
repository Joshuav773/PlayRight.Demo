using NUnit.Framework;
using PlayRight.Demo.AutomationBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayRight.Demo.tests
{
    [TestFixture]
    public class SearchTests : BaseDriver
    {
        [Test]
        public async Task ICanClickTheSearchBar()
        {
            //TODO: this should come from some page object i.e. HomePage.SearchBarLocator
            var searchBarLoc = "xpath=//input[@name='q']";

            await CurrentPage.ClickAsync(searchBarLoc);
            await CurrentPage.TypeAsync(searchBarLoc, "Hey, i worked!");

            await Task.Delay(2000);
        }
    }
}
