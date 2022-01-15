using OpenQA.Selenium;
using Selenium.AutomatedTests.Steps;

namespace Selenium.AutomatedTests.Samples;

public class GoogleSearchSteps : SetupSteps
{
    private static readonly By SearchBarElementLocator = By.ClassName("gLFyf");
    private static readonly By AcceptTermsAndConditionsElementLocator = By.ClassName("sy4vM");

    public GoogleSearchSteps(AutomationScenarioBuilder scenarioBuilder) : base(scenarioBuilder)
    {
    }

    public void WaitUntilSearchBarIsLoaded()
    {
        _scenarioBuilder.WaitUntilVisible(SearchBarElementLocator);
    }

    public void ClickOnAcceptTermsAndConditions()
    {
        _scenarioBuilder
            .WithStep(
                description: $"Clicking on accept terms and conditions...",
                selectionPredicate: webDriver => webDriver.FindElement(AcceptTermsAndConditionsElementLocator),
                action: webElement => { webElement.Click(); });
    }

    public void Search(string text)
    {
        _scenarioBuilder
            .WaitUntilVisible(SearchBarElementLocator)
            .WithStep(
                description: $"Filling search var with text: {text}",
                selectionPredicate: webDriver => webDriver.FindElement(SearchBarElementLocator),
                action: webElement => { webElement.SendKeys(text); })
            .WithStep(
                description: $"Executing search...",
                selectionPredicate: webDriver => webDriver.FindElement(By.ClassName("gLFyfasdasd")),
                action: webElement => { });
    }
}
