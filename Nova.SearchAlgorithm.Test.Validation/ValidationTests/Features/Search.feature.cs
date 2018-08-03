﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Nova.SearchAlgorithm.Test.Validation.ValidationTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Search")]
    public partial class SearchFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Search.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Search", "  As a member of the search team\r\n  I want to be able to search for matching dono" +
                    "rs", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("6/6 Search for a matching donor")]
        public virtual void _66SearchForAMatchingDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("6/6 Search for a matching donor", ((string[])(null)));
#line 5
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.Given("I search for recognised hla", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
    testRunner.And("The search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
    testRunner.And("The search is run for Anthony Nolan\'s registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
    testRunner.When("I run a 6/6 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
    testRunner.Then("The result should contain at least one donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("8/8 Search for a matching donor")]
        public virtual void _88SearchForAMatchingDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("8/8 Search for a matching donor", ((string[])(null)));
#line 12
  this.ScenarioSetup(scenarioInfo);
#line 13
    testRunner.Given("I search for recognised hla", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
    testRunner.And("The search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
    testRunner.And("The search is run for Anthony Nolan\'s registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
    testRunner.When("I run an 8/8 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
    testRunner.Then("The result should contain at least one donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search for a matching donor")]
        public virtual void _1010SearchForAMatchingDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search for a matching donor", ((string[])(null)));
#line 19
  this.ScenarioSetup(scenarioInfo);
#line 20
    testRunner.Given("I search for recognised hla", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
    testRunner.And("The search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.And("The search is run for Anthony Nolan\'s registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
    testRunner.Then("The result should contain at least one donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("9/10 Search for a matching donor")]
        public virtual void _910SearchForAMatchingDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("9/10 Search for a matching donor", ((string[])(null)));
#line 26
  this.ScenarioSetup(scenarioInfo);
#line 27
    testRunner.Given("I search for recognised hla", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
    testRunner.And("The search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
    testRunner.And("The search is run for Anthony Nolan\'s registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.When("I run a 9/10 search at locus A", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
    testRunner.Then("The result should contain no donors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("6/6 Search for a matching cord")]
        public virtual void _66SearchForAMatchingCord()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("6/6 Search for a matching cord", ((string[])(null)));
#line 33
  this.ScenarioSetup(scenarioInfo);
#line 34
    testRunner.Given("I search for recognised hla", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 35
    testRunner.And("The search type is cord", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.And("The search is run for Anthony Nolan\'s registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.When("I run a 6/6 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
    testRunner.Then("The result should contain at least one donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
