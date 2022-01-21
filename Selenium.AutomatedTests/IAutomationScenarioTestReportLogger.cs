namespace Selenium.AutomatedTests
{
    /// <summary>
    /// Interface for logging the results of the execution
    /// </summary>
    public interface IAutomationScenarioTestReportLogger
    {
        public void Log(AutomationScenarioTestReport report);
    }
}