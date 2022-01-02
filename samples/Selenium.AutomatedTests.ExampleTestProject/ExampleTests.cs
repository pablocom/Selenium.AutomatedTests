using Selenium.AutomatedTests.Core;
using Xunit;

namespace Selenium.AutomatedTests.ExampleTestProject
{
    public class ExampleTests : AutomatedTestBase
    {
        [Fact]
        public void SuccessfulTest()
        {
            RunAutomatedTest(builder =>
            {
                builder.NavigateToUrl("https://google.com");
            });
        }
    }
}