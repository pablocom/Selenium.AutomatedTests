using System;
using OpenQA.Selenium;
using Selenium.AutomatedTests.Steps;

namespace Selenium.AutomatedTests.UnitTests;

internal class FakeFailureStep : IStep
{
    public bool HasFailed { get; private set; }
    public string Description { get; set; }
    public string? Result { get; private set; }

    internal FakeFailureStep(string description)
    {
        Description = description;
    }

    public void Execute(IWebDriver webDriver) => throw new Exception("Step failed");

    public void Fail(string message)
    {
        HasFailed = true;
        Result = message;
    }
}