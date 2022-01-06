using OpenQA.Selenium;
using System;

namespace Selenium.AutomatedTests.Core
{
    public abstract class AutomatedTestBase
    {
        protected abstract IWebDriver ProvideWebDriver();

        protected void RunAutomatedTest(Action<AutomationScenarioBuilder> testAction)
        {
            using var scenarioBuilder = new AutomationScenarioBuilder(ProvideWebDriver());
            testAction(scenarioBuilder);

            var scenarioReport = scenarioBuilder.BuildAndRun();
            if (scenarioReport.HasFailure)
            {
                throw new AutomationScenarioRunFailedException(scenarioReport.GetSummary());
            }
            Console.WriteLine(scenarioReport.GetSummary());
        }
    }
}
