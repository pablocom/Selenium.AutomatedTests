using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Selenium.AutomatedTests.Samples;

public class ExampleScenarios : AutomatedTestBase
{
    [Fact]
    public void SearchForATextInGoogle()
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
                });
        });
    }

    protected override IWebDriver ProvideWebDriver() => new ChromeDriver();
}

