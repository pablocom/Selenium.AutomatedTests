

namespace Selenium.AutomatedTests.Core.Steps
{
    public abstract class SetupSteps
    {
        protected readonly AutomationScenarioBuilder _scenarioBuilder;

        protected SetupSteps(AutomationScenarioBuilder scenarioBuilder)
        {
            _scenarioBuilder = scenarioBuilder;
        }
    }
}