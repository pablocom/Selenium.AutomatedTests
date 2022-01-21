using Xunit;
using OpenQA.Selenium;
using NSubstitute;
using Selenium.AutomatedTests.Steps;

namespace Selenium.AutomatedTests.UnitTests;

public class AutomationScenarioBuilderTests
{
    private IWebDriver _webDriver;
    private IAutomationScenarioTestReportLogger _logger;
    private AutomationScenarioBuilder _automationScenarioBuilder;

    public AutomationScenarioBuilderTests()
    {
        _webDriver = Substitute.For<IWebDriver>();
        _logger = Substitute.For<IAutomationScenarioTestReportLogger>();
        _automationScenarioBuilder = new AutomationScenarioBuilder(_webDriver, _logger);
    }

    [Fact]
    public void Runs_with_no_steps_just_ok()
    {
        _automationScenarioBuilder.Run();

        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.HasFailure == false));
    }

    [Fact]
    public void Runs_with_single_step_ok()
    {
        AssumeStepsInScenario(Substitute.For<IStep>());

        _automationScenarioBuilder.Run();

        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.HasFailure == false));
    }

    [Fact]
    public void Runs_with_two_steps_ok_and_generates_summary()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        firstStep.Description.Returns("First step description");
        secondStep.Description.Returns("Second step description");
        AssumeStepsInScenario(firstStep, secondStep);

        _automationScenarioBuilder.Run();

        
        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.HasFailure == false));
        var expectedSummary = $"\n\nStep 1: First step description : \n\n" +
                              $"Step 2: Second step description : ";
        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.GetSummary() == expectedSummary));

    }

    [Fact]
    public void Runs_steps_in_order()
    {
        var firstStep = Substitute.For<IStep>();
        var secondStep = Substitute.For<IStep>();
        AssumeStepsInScenario(firstStep, secondStep);

        _automationScenarioBuilder.Run();

        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.HasFailure == false));

        Received.InOrder(() =>
        {
            firstStep.Execute(_webDriver);
            secondStep.Execute(_webDriver);
        });
    }

    [Fact]
    public void Stops_scenario_execution_if_exception_is_thrown()
    {
        var firstStep = Substitute.For<IStep>();
        firstStep.Description.Returns("First step description");
        var secondStep = new FakeFailureStep("Failure step");
        var thirdStep = Substitute.For<IStep>();
        AssumeStepsInScenario(firstStep, secondStep, thirdStep);

        _automationScenarioBuilder.Run();
        
        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.HasFailure == true));
        var expectedSummaryStart = $"\n\nStep 1: First step description : \n\n" +
                                   $"Step 2: Failure step";
        _logger.Received().Log(Arg.Is<AutomationScenarioTestReport>(x => x.GetSummary().Contains(expectedSummaryStart)));
        thirdStep.DidNotReceive().Execute(_webDriver);
    }

    private void AssumeStepsInScenario(params IStep[] steps)
    {
        foreach (var step in steps)
            _automationScenarioBuilder.WithStep(step);
    }
}