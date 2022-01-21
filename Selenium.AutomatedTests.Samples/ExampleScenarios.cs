using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Selenium.AutomatedTests.Samples;

public class ExampleScenarios
{
    protected IWebDriver ProvideWebDriver() => new ChromeDriver();

    protected IAutomationScenarioTestReportLogger ProvideAutomationScenarioTestReportLogger() => new ConsoleAutomationScenarioTestReportLogger();

        [Fact]
    public void SearchForATextInGoogle()
    {
        var builder = new AutomationScenarioBuilder(ProvideWebDriver(), ProvideAutomationScenarioTestReportLogger());

        builder.NavigateTo("https://google.com")
            .WithSteps<GoogleSearchSteps>(step =>
            {
                step.WaitUntilSearchBarIsLoaded();
                step.ClickOnAcceptTermsAndConditions();
                step.Search("This is fine gif");
            })
            .Run();
    }
}

