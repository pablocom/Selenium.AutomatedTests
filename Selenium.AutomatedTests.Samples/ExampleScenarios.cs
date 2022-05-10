using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Selenium.AutomatedTests.Samples;

public class ExampleScenarios
{
    protected IWebDriver ProvideWebDriver() => new ChromeDriver();

    [Fact]
    public void SearchForATextInGoogle()
    {
        using var builder = new AutomationScenarioBuilder(ProvideWebDriver());

        builder.NavigateTo("https://google.com")
            .WithSteps<GoogleSearchSteps>(step =>
            {
                step.WaitUntilSearchBarIsLoaded();
                step.ClickOnAcceptTermsAndConditions();
                step.Search("This is fine gif");
            });

        AutomationScenarioRunner.Run(builder);
    }
}

