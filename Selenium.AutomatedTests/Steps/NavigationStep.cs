﻿using OpenQA.Selenium;
using System;

namespace Selenium.AutomatedTests.Steps
{
    internal class NavigationStep : IStep
    {
        private readonly IWebDriver _webDriver;
        private readonly Uri _url;

        public string Description { get; }
        public bool HasFailed { get; private set; }
        public string Result { get; private set; }

        public NavigationStep(IWebDriver webDriver, string url)
        {
            _webDriver = webDriver;
            _url = new Uri(url);
            Description = $"Navigating to {_url.AbsoluteUri}...";
        }

        public void Execute()
        {
            _webDriver.Navigate().GoToUrl(_url);
        }

        public void Fail(string message)
        {
            HasFailed = true;
            Result = message;
        }
    }
}
