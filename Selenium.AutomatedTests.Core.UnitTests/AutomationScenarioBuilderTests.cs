using System;
using Xunit;
using OpenQA.Selenium;
using Selenium.AutomatedTests.Core.Steps;
using NSubstitute;

namespace Selenium.AutomatedTests.Core.UnitTests;

public class AutomationScenarioBuilderTests
{
    private IWebDriver _webDriver;
    private AutomationScenarioBuilder _automationScenarioBuilder;

    public AutomationScenarioBuilderTests()
    {
        _webDriver = Substitute.For<IWebDriver>();
        _automationScenarioBuilder = new AutomationScenarioBuilder(_webDriver);
    }

    [Fact]
    public void Runs_with_no_steps_just_ok()
    {
        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
    }

    [Fact]
    public void Runs_with_single_step_ok()
    {
        _automationScenarioBuilder.WithStep(Substitute.For<IStep>());

        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
    }

    [Fact]
    public void Runs_with_two_steps_ok_and_generates_summary()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        firstStep.Description.Returns("First step description");
        secondStep.Description.Returns("Second step description");
        _automationScenarioBuilder.WithStep(firstStep);
        _automationScenarioBuilder.WithStep(secondStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();

        var expectedSummary = $"\n\nStep 1: First step description : \n\n" +
                                  $"Step 2: Second step description : ";
        Assert.False(testReport.HasFailure);
        Assert.Equal(expectedSummary, testReport.GetSummary());
    }

    [Fact]
    public void Executes_steps_in_order()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();

        _automationScenarioBuilder.WithStep(firstStep);
        _automationScenarioBuilder.WithStep(secondStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();

        Assert.False(testReport.HasFailure);
        Received.InOrder(() =>
        {
            firstStep.Execute(_webDriver);
            secondStep.Execute(_webDriver);
        });
    }

    [Fact]
    public void Stops_scenario_execution_if_exception_is_thrown_in_step()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        var thirdStep = Substitute.For<IStep>();
        
        secondStep.When(x => x.Execute(_webDriver)).Throw<Exception>();
        
        _automationScenarioBuilder.WithStep(firstStep);
        _automationScenarioBuilder.WithStep(secondStep);
        _automationScenarioBuilder.WithStep(thirdStep);

        var testReport = _automationScenarioBuilder.BuildAndRun();
        
        Assert.True(testReport.HasFailure);
        firstStep.Received(1).Execute(_webDriver);
        secondStep.Received(1).Execute(_webDriver);
        thirdStep.DidNotReceive().Execute(_webDriver);
    }
}
