# Selenium.AutomatedTests.Core

This project brings together a collection of tools to enhance the Selenium automation test development experience. It consists of an AutomationScenarioTestBuilder that configures a queue of steps the test has to run to consider the test result as successful.

The aim of this package is to provide an easy way to write clear and explicit intention-revealing end-to-end tests by allowing users to write scenarios in a fluent fashion.

Here is an example of what can be done with it:

```csharp
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
            })
            .WithStep(
                description: $"Some step description",
                selectionPredicate: webDriver => webDriver.FindElement(By.Id("elementId")),
                action: webElement => { webElement.SendKeys("Some input"); }
            );
    });
}

```
