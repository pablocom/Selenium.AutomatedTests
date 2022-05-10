using System;
using System.Threading;
using OpenQA.Selenium;

namespace Selenium.AutomatedTests.Steps
{
    internal class WaitStep : IStep
    {
        public bool HasFailed { get; private set; }
        public string Description { get; }
        public string Result { get; private set; }

        private readonly TimeSpan WaitingTime;
        
        internal WaitStep(TimeSpan waitingTime)
        {
            WaitingTime = waitingTime;
            Description = $"Waiting {waitingTime:g}...";
        }

        public void Execute(IWebDriver webDriver) => Thread.Sleep(WaitingTime);

        public void Fail(string message)
        {
            HasFailed = true;
            Result = message;
        }
    }
}