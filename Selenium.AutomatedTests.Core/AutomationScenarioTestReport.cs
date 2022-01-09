using Selenium.AutomatedTests.Core.Steps;
using System.Collections.Generic;

namespace Selenium.AutomatedTests.Core
{
    /// <summary>
    /// Contains information about scenario execution
    /// </summary>
    public class AutomationScenarioTestReport
    {
        private readonly ICollection<string> results;
        private int _stepIndex = 1;

        public bool HasFailure { get; private set; }

        internal AutomationScenarioTestReport()
        {
            results = new List<string>();
        }

        internal void AddResultFrom(IStep step)
        {
            if (step.HasFailed)
            {
                HasFailure = true;
            }
            results.Add(FormatResult(step));
            _stepIndex++;
        }

        public string GetSummary()
        {
            return $"\n\n{string.Join("\n\n", results)}";
        }

        private string FormatResult(IStep step)
        {
            return $"Step {_stepIndex}: {step.Description} : {step.Result}";
        }
    }
}
