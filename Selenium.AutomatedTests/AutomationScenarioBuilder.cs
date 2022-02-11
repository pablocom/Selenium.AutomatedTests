using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.AutomatedTests.Steps;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Selenium.AutomatedTests
{
    /// <summary>
    /// Represents automation scenario setup.
    /// </summary>
    public class AutomationScenarioBuilder : IDisposable
    {
        private static readonly TimeSpan DefaultVisibilityTimeout = TimeSpan.FromSeconds(15);

        private readonly IWebDriver _webDriver;
        private readonly Queue<IStep> _steps = new Queue<IStep>();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="webDriver"></param>
        public AutomationScenarioBuilder(IWebDriver webDriver)
        {
            _webDriver = webDriver;
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

        /// <summary>
        /// Enqueues step to wait until web element with <paramref name="elementSelector"/> is visible.
        /// </summary>
        /// <param name="elementSelector">Web element selector</param>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder WaitUntilVisible(By elementSelector)
        {
            return WaitUntilVisible(elementSelector, _ => { });
        }

        /// <summary>
        /// Enqueues step to wait until web element with <paramref name="elementSelector"/> is visible and 
        ///     executes <paramref name="action"/> if the element is located.
        /// </summary>
        /// <param name="elementSelector">Web element selector</param>
        /// <param name="action"></param>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder WaitUntilVisible(By elementSelector, Action<IWebElement> action)
        {
            var item = new SingleWebElementStep(
                webDriver =>
                {
                    var wait = new WebDriverWait(_webDriver, DefaultVisibilityTimeout);
                    return wait.Until(ExpectedConditions.ElementIsVisible(elementSelector));
                },
                action,
                description: $"Waiting until element with {elementSelector.Criteria} is visible..."
            );
            _steps.Enqueue(item);

            return this;
        }

        /// <summary>
        /// Enqueues step that sleeps number of <paramref name="milliseconds"/>
        /// </summary>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        /// <summary>
        /// Enqueues step that navigates to <paramref name="url"/>
        /// </summary>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder NavigateTo(string url)
        {
            var step = new NavigationStep(url);
            _steps.Enqueue(step);
            return this;
        }

        /// <summary>
        /// Enqueues step to find web element and invokes <paramref name="action"/> with located web element
        /// </summary>
        /// <param name="selectionPredicate">Function to locate single web element in <see cref="IWebDriver"/></param>
        /// <param name="action">Action invoked with the located web element. It can be used for assertions and interactions.</param>
        /// <param name="description">Step description for execution summary reporting.</param>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder WithStep(Func<IWebDriver, IWebElement> selectionPredicate,
            Action<IWebElement> action, string description)
        {
            var item = new SingleWebElementStep(selectionPredicate, action, description);
            _steps.Enqueue(item);
            return this;
        }

        /// <summary>
        /// Enqueues step to find web element and invokes <paramref name="action"/> with located web element
        /// </summary>
        /// <param name="selectionPredicate">Function to locate multiple web elements in <see cref="IWebDriver"/></param>
        /// <param name="action">Action invoked with located web elements. It can be used for assertions and interactions.</param>
        /// <param name="description">Step description for execution summary reporting.</param>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder WithStep(Func<IWebDriver, IEnumerable<IWebElement>> selectionPredicate,
           Action<IEnumerable<IWebElement>> action, string description)
        {
            var item = new MultipleWebElementsStep(selectionPredicate, action, description);
            _steps.Enqueue(item);
            return this;
        }

        /// <summary>
        /// Adding generic step
        /// </summary>
        /// <param name="steps"></param>
        /// <typeparam name="TSetupSteps"></typeparam>
        /// <returns></returns>
        public AutomationScenarioBuilder WithSteps<TSetupSteps>(Action<TSetupSteps> steps)
            where TSetupSteps : SetupSteps
        {
            var setupSteps = (TSetupSteps)Activator.CreateInstance(typeof(TSetupSteps), this);
            steps(setupSteps);
            return this;
        }

        /// <summary>
        /// Enqueues custom <see cref="IStep"/>
        /// </summary>
        /// <returns>The same <see cref="AutomationScenarioBuilder"/> instance</returns>
        public AutomationScenarioBuilder WithStep(IStep step)
        {
            _steps.Enqueue(step);
            return this;
        }

        /// <summary>
        /// clean at the de-construction of the object 
        /// </summary>
        public void Dispose()
        {
            _webDriver.Dispose();
        }
    }
}
