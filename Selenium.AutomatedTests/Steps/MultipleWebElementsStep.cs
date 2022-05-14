using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Selenium.AutomatedTests.Steps;

internal class MultipleWebElementsStep : IStep
{
    private readonly Func<IWebDriver, IEnumerable<IWebElement>> _selectionPredicate;
    private readonly Action<IEnumerable<IWebElement>> _action;

    public MultipleWebElementsStep(Func<IWebDriver, IEnumerable<IWebElement>> selectionPredicate, Action<IEnumerable<IWebElement>> action, string description)
    {
        _selectionPredicate = selectionPredicate;
        _action = action;
        Description = description;
    }

    public string Description { get; }
    public bool HasFailed { get; private set; }
    public string Result { get; private set; }

    public void Execute(IWebDriver webDriver)
    {

        var webElements = _selectionPredicate(webDriver);
        _action(webElements);
    }

    public void Fail(string message)
    {
        HasFailed = true;
        Result = message;
    }
}
