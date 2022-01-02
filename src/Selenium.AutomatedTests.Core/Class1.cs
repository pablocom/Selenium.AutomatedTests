using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;


namespace Selenium.AutomatedTests.Core
{
    public abstract class AutomatedTestBase
    {
        protected void RunAutomatedTest(Action<AutomationScenarioBuilder> testAction)
        {
            var options = new ChromeOptions();
            var driver = new ChromeDriver(options);
            driver.LaunchApp(Guid.NewGuid().ToString());

            AutomationScenarioTestReport scenarioReport;
            using (var scenarioBuilder = new AutomationScenarioBuilder(driver))
            {
                testAction(scenarioBuilder);

                scenarioReport = scenarioBuilder.BuildAndRun();
            }

            if (scenarioReport.HasFailure)
            {
                throw new AutomationScenarioRunFailedException(scenarioReport.GetSummary());
            }
            Console.WriteLine(scenarioReport.GetSummary());
        }
    }
}
