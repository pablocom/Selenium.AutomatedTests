using System;

namespace Selenium.AutomatedTests
{
    /// <summary>
    /// Represents failure in the execution of an automation scenario
    /// </summary>
    [Serializable]
    public class AutomationScenarioRunFailedException : Exception
    {
        internal AutomationScenarioRunFailedException(string automationTestReportSummary)
            : base(automationTestReportSummary)
        { }
    }
}