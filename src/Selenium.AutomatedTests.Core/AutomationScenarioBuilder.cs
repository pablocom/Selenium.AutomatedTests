using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.AutomatedTests.Core.Steps;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.AutomatedTests.Core
{
    public class AutomationScenarioBuilder : IDisposable
    {
        private static readonly TimeSpan DefaultVisibilityTimeout = TimeSpan.FromSeconds(30);

        private readonly IWebDriver _webDriver;
        private readonly Queue<IStep> _steps;

        public AutomationScenarioBuilder(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _steps = new Queue<IStep>();
        }

        public AutomationScenarioTestReport BuildAndRun()
        {
            var testReport = new AutomationScenarioTestReport();
            while (_steps.Any() && !testReport.HasFailure)
            {
                var currentStep = _steps.Dequeue();
                try
                {
                    currentStep.Execute(_webDriver);
                }
                catch (Exception ex)
                {
                    currentStep.Fail($"\n\t Exception on step : {currentStep.Description} \n\n {ex}");
                }
                finally
                {
                    testReport.AddResultFrom(currentStep);
                }
            }

            return testReport;
        }

        public AutomationScenarioBuilder WaitUntilVisible(By elementSelector)
        {
            return WaitUntilVisible(elementSelector, _ => { });
        }

        public AutomationScenarioBuilder WaitUntilVisible(By elementSelector, Action<IWebElement> action)
        {
            var item = new SingleWebElementStep(
                webDriver =>
                {
                    var wait = new WebDriverWait(_webDriver, DefaultVisibilityTimeout);
                    return wait.Until(ExpectedConditions.ElementIsVisible(elementSelector));
                },
                action: action,
                description: $"Waiting until element with {elementSelector.Criteria} is visible..."
            );

            _steps.Enqueue(item);

            return this;
        }

        public AutomationScenarioBuilder NavigateToUrl(string url)
        {
            var step = new NavigationStep(url);
            _steps.Enqueue(step);
            return this;
        }

        public AutomationScenarioBuilder WithStep(Func<IWebDriver, IWebElement> selectionPredicate,
            Action<IWebElement> action, string description)
        {
            var item = new SingleWebElementStep(selectionPredicate, action, description);
            _steps.Enqueue(item);
            return this;
        }

        public AutomationScenarioBuilder WithStep(Func<IWebDriver, IEnumerable<IWebElement>> selectionPredicate,
           Action<IEnumerable<IWebElement>> action, string description)
        {
            var item = new MultipleWebElementsStep(selectionPredicate, action, description);
            _steps.Enqueue(item);
            return this;
        }

        public AutomationScenarioBuilder WithSteps<TSetupSteps>(params Action<TSetupSteps>[] steps)
            where TSetupSteps : SetupSteps
        {
            var setupSteps = (TSetupSteps)Activator.CreateInstance(typeof(TSetupSteps), this);
            foreach (var step in steps)
            {
                step(setupSteps);
            }
            return this;
        }

        public AutomationScenarioBuilder WithStep(IStep step)
        {
            _steps.Enqueue(step);
            return this;
        }

        public void Dispose()
        {
            _webDriver.Close();
            _webDriver.Dispose();
        }
    }
}
