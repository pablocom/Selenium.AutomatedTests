using OpenQA.Selenium;
using System;

namespace Selenium.AutomatedTests
{
    /// <summary>
    /// Defines an automation test base class to encapsulate automation scenario setup and execution
    /// </summary>
    public abstract class AutomatedTestBase
    {
        protected abstract IWebDriver ProvideWebDriver();

        /// <summary>
        /// <para>Configures automation scenario  with the provided <see cref="IWebDriver"/>.</para>
        /// <para>If any step fails, it will stop execution and throw <see cref="AutomationScenarioRunFailedException"/>.</para>
        /// </summary>
        /// <param name="testAction">Test action</param>
        /// <exception cref="AutomationScenarioRunFailedException"></exception>
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
