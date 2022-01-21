using System;

namespace Selenium.AutomatedTests.Samples;

public class ConsoleAutomationScenarioTestReportLogger : IAutomationScenarioTestReportLogger
{
    public void Log(AutomationScenarioTestReport report)
    {
        Console.Write(report.GetSummary());
    }
}