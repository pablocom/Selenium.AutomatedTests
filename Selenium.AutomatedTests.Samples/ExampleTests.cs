using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.AutomatedTests.Core;
using Selenium.AutomatedTests.Core.Samples;
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
                    .NavigateTo("https://google.com")
                    .WithSteps<GoogleSearchSteps>(step =>
                    {
                        step.WaitUntilSearchBarIsLoaded();
                        step.ClickOnAcceptTermsAndConditions();
                        step.Search("This is fine gif");
                    })
                    .WithStep(
                        description: $"Some step description",
                        selectionPredicate: webDriver => webDriver.FindElement(By.Id("elementId")),
                        action: webElement => { webElement.SendKeys("Some input"); }
                    );
            });
        }

        protected override IWebDriver ProvideWebDriver() => new ChromeDriver();
    }
}