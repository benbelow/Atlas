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
namespace Nova.SearchAlgorithm.Test.Validation.ValidationTests.Features.TenOutOfTenSearch
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Ten Out Of Ten Search")]
    public partial class TenOutOfTenSearchFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Search.feature"
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
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a homozygous donor")]
        public virtual void _1010SearchWithAHomozygousDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a homozygous donor", ((string[])(null)));
#line 5
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
    testRunner.And("the matching donor is homozygous at locus A", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a homozygous patient")]
        public virtual void _1010SearchWithAHomozygousPatient()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a homozygous patient", ((string[])(null)));
#line 11
  this.ScenarioSetup(scenarioInfo);
#line 12
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
    testRunner.And("the patient is homozygous at locus A", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a p group match")]
        public virtual void _1010SearchWithAPGroupMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a p group match", ((string[])(null)));
#line 17
  this.ScenarioSetup(scenarioInfo);
#line 18
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
    testRunner.And("the match level is p-group", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a match with differing fourth field")]
        public virtual void _1010SearchWithAMatchWithDifferingFourthField()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a match with differing fourth field", ((string[])(null)));
#line 23
  this.ScenarioSetup(scenarioInfo);
#line 24
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
    testRunner.And("the matching donor is \'TGS derived data at four-field resolution\' typed at each l" +
                    "ocus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
    testRunner.And("the match level is three field (different fourth field)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a match with differing third field")]
        public virtual void _1010SearchWithAMatchWithDifferingThirdField()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a match with differing third field", ((string[])(null)));
#line 30
  this.ScenarioSetup(scenarioInfo);
#line 31
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
    testRunner.And("the matching donor is \'TGS derived data at three-field resolution\' typed at each " +
                    "locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("the match level is two field (different third field)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a match with an expression suffix")]
        public virtual void _1010SearchWithAMatchWithAnExpressionSuffix()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a match with an expression suffix", ((string[])(null)));
#line 37
  this.ScenarioSetup(scenarioInfo);
#line 38
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
    testRunner.And("the matching donor has an allele with any (non-null) expression suffix at locus A" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
    testRunner.And("the matching donor has an allele with any (non-null) expression suffix at locus B" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
    testRunner.And("the matching donor has an allele with any (non-null) expression suffix at locus C" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
    testRunner.And("the matching donor has an allele with any (non-null) expression suffix at locus D" +
                    "QB1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("the matching donor has an allele with any (non-null) expression suffix at locus D" +
                    "PB1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a cross match")]
        public virtual void _1010SearchWithACrossMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a cross match", ((string[])(null)));
#line 47
  this.ScenarioSetup(scenarioInfo);
#line 48
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
    testRunner.And("the match orientation is cross at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 51
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a direct match")]
        public virtual void _1010SearchWithADirectMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a direct match", ((string[])(null)));
#line 53
  this.ScenarioSetup(scenarioInfo);
#line 54
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
    testRunner.And("the match orientation is direct at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
