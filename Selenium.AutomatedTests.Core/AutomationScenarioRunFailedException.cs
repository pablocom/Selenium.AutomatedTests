using System;

namespace Selenium.AutomatedTests.Core
{
    [Serializable]
    public class AutomationScenarioRunFailedException : Exception
    {
        public AutomationScenarioRunFailedException(string message) : base(message)
        { }
    }
}