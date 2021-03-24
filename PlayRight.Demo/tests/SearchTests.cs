using NUnit.Framework;
using PlayRight.Demo.AutomationBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayRight.Demo.tests
{
    public class SearchTests : BaseDriver
    {
        [Test]
        public async Task ICanClickTheSearchBar()
        {
            await CurrentPage.ClickAsync("//input[name='q']");
        }
    }
}
