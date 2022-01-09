# Selenium.AutomatedTests.Core

Easily write maintainable end-to-end tests using Selenium by writing automation scenarios in a fluent fashion.

It consist on a set of tools built atop existing features in Selenium.

The aim of this package is to provide an easy way to write clear and explicit intention-revealing end-to-end tests, providing a builder to setup an automation scenario defined by steps.
It is agnostic from any testing framework, so it can be used indifferently with xUnit, NUnit, etc

## Installation
Install the library from [NuGet](https://www.nuget.org/packages/Selenium.AutomatedTests.Core):
``` console
dotnet add package Selenium.AutomatedTests.Core --prerelease
```

## Example usage

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
