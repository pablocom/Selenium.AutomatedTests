using System;
using OpenQA.Selenium;

namespace Selenium.AutomatedTests.Steps
{
    internal class SingleWebElementStep : IStep
    {
        private readonly Func<IWebDriver, IWebElement> _selectionPredicate;
        private readonly Action<IWebElement> _action;

        public string Description { get; }
        public bool HasFailed { get; private set; }
        public string Result { get; private set; }

        public SingleWebElementStep(Func<IWebDriver, IWebElement> selectionPredicate, Action<IWebElement> action, string description)
        {
            _selectionPredicate = selectionPredicate;
            _action = action;
            Description = description;
        }

        public void Execute(IWebDriver webDriver)
        {
            var webElement = _selectionPredicate.Invoke(webDriver);
            _action(webElement);
        }

        public void Fail(string message)
        {
            HasFailed = true;
            Result = message;
        }
    }
}
