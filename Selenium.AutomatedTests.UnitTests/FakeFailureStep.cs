using System;
using OpenQA.Selenium;
using Selenium.AutomatedTests.Steps;

namespace Selenium.AutomatedTests.UnitTests;

internal class FakeFailureStep : IStep
{
    private readonly IWebDriver _webDriver;
    public bool HasFailed { get; private set; }
    public string Description { get; set; }
    public string? Result { get; private set; }

    internal FakeFailureStep(IWebDriver webDriver, string description)
    {
        _webDriver = webDriver;
        Description = description;
    }

    public void Execute() => throw new Exception("Step failed");

    public void Fail(string message)
    {
        HasFailed = true;
        Result = message;
    }
}