using OpenQA.Selenium;

namespace Selenium.AutomatedTests.Steps;

public interface IStep
{
    bool HasFailed { get; }
    string Description { get; }
    string Result { get; }

    void Execute(IWebDriver webDriver);
    void Fail(string message);
}
