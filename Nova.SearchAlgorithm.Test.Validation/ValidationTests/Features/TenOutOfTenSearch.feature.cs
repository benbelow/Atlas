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
    [NUnit.Framework.DescriptionAttribute("Ten Out Of Ten Search")]
    public partial class TenOutOfTenSearchFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "TenOutOfTenSearch.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Ten Out Of Ten Search", "  As a member of the search team\r\n  I want to be able to run a 10/10 search", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a TGS typed match")]
        public virtual void _1010SearchWithATGSTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a TGS typed match", ((string[])(null)));
#line 5
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
    testRunner.And("the matching donor is TGS typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at C")]
        public virtual void _1010SearchWithUntypedDonorAtC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at C", ((string[])(null)));
#line 16
  this.ScenarioSetup(scenarioInfo);
#line 17
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
    testRunner.And("the matching donor is TGS typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at Dqb1")]
        public virtual void _1010SearchWithUntypedDonorAtDqb1()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at Dqb1", ((string[])(null)));
#line 28
  this.ScenarioSetup(scenarioInfo);
#line 29
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
    testRunner.And("the matching donor is TGS typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
    testRunner.And("the matching donor is untyped at Locus Dqb1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at C and Dqb1")]
        public virtual void _1010SearchWithUntypedDonorAtCAndDqb1()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at C and Dqb1", ((string[])(null)));
#line 40
  this.ScenarioSetup(scenarioInfo);
#line 41
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("the matching donor is TGS typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
    testRunner.And("the matching donor is untyped at Locus Dqb1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 51
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a three field truncated allele typed match")]
        public virtual void _1010SearchWithAThreeFieldTruncatedAlleleTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a three field truncated allele typed match", ((string[])(null)));
#line 53
  this.ScenarioSetup(scenarioInfo);
#line 54
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
    testRunner.And("the matching donor is three field truncated allele typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a two field truncated allele typed match")]
        public virtual void _1010SearchWithATwoFieldTruncatedAlleleTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a two field truncated allele typed match", ((string[])(null)));
#line 64
  this.ScenarioSetup(scenarioInfo);
#line 65
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 66
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
    testRunner.And("the matching donor is two field truncated allele typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a XX code typed match")]
        public virtual void _1010SearchWithAXXCodeTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a XX code typed match", ((string[])(null)));
#line 75
  this.ScenarioSetup(scenarioInfo);
#line 76
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 77
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
    testRunner.And("the matching donor is XX code typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a NMDP code typed match")]
        public virtual void _1010SearchWithANMDPCodeTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a NMDP code typed match", ((string[])(null)));
#line 86
  this.ScenarioSetup(scenarioInfo);
#line 87
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 88
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
    testRunner.And("the matching donor is NMDP code typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 95
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a serology typed match")]
        public virtual void _1010SearchWithASerologyTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a serology typed match", ((string[])(null)));
#line 97
  this.ScenarioSetup(scenarioInfo);
#line 98
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 99
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
    testRunner.And("the matching donor is serology typed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
