using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.AutomatedTests.Core;
using Xunit;

namespace Selenium.AutomatedTests.Samples
{
    public class ExampleTests : AutomatedTestBase
    {
        [Fact]
        public void SuccessfulTest()
        {
            RunAutomatedTest(builder =>
            {
                builder
                    .NavigateToUrl("https://google.com")
                    .WithSteps<GoogleSearchSteps>(step =>
                    {
                        step.WaitUntilSearchBarIsLoaded();
                        step.ClickOnAcceptTermsAndConditions();
                        step.Search("This is fine gif");
                    });
            });
        }

        protected override IWebDriver ProvideWebDriver() => new ChromeDriver();
    }
}