using System;

namespace Selenium.AutomatedTests;

/// <summary>
/// Runs automation scenario builder
/// </summary>
public static class AutomationScenarioRunner
{
    /// <summary>
    /// <para>Builds and runs <paramref name="automationScenarioBuilder"/>.</para>
    /// <para>If any step fails, execution is stopped and it will throw an exception of type <see cref="AutomationScenarioRunFailedException"/>.</para>
    /// </summary>
    /// <exception cref="AutomationScenarioRunFailedException"></exception>
    public static void Run(AutomationScenarioBuilder automationScenarioBuilder)
    {
        var scenarioReport = automationScenarioBuilder.BuildAndRun();
        if (scenarioReport.HasFailure)
        {
            throw new AutomationScenarioRunFailedException(scenarioReport.GetSummary());
        }
        Console.WriteLine(scenarioReport.GetSummary());
    }
}
