﻿using System;

namespace Selenium.AutomatedTests.Core
{
    /// <summary>
    /// Represents failure in the execution of an automation scenario
    /// </summary>
    [Serializable]
    public class AutomationScenarioRunFailedException : Exception
    {
        internal AutomationScenarioRunFailedException(string message) : base(message)
        { }
    }
}