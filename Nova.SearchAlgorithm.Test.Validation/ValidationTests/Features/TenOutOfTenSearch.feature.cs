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
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a TGS (4-field) typed match")]
        public virtual void _1010SearchWithATGS4_FieldTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a TGS (4-field) typed match", ((string[])(null)));
#line 5
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
    testRunner.And("the matching donor is TGS (four field) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a TGS (3-field) typed match")]
        public virtual void _1010SearchWithATGS3_FieldTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a TGS (3-field) typed match", ((string[])(null)));
#line 16
  this.ScenarioSetup(scenarioInfo);
#line 17
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.And("the matching donor is TGS (three field) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a TGS (2-field) typed match")]
        public virtual void _1010SearchWithATGS2_FieldTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a TGS (2-field) typed match", ((string[])(null)));
#line 27
  this.ScenarioSetup(scenarioInfo);
#line 28
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
    testRunner.And("the matching donor is TGS (two field) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a TGS typed match")]
        public virtual void _1010SearchWithATGSTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a TGS typed match", ((string[])(null)));
#line 38
  this.ScenarioSetup(scenarioInfo);
#line 39
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 40
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
    testRunner.And("the matching donor is TGS typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at C")]
        public virtual void _1010SearchWithUntypedDonorAtC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at C", ((string[])(null)));
#line 49
  this.ScenarioSetup(scenarioInfo);
#line 50
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 51
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at Dqb1")]
        public virtual void _1010SearchWithUntypedDonorAtDqb1()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at Dqb1", ((string[])(null)));
#line 60
  this.ScenarioSetup(scenarioInfo);
#line 61
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
    testRunner.And("the matching donor is untyped at Locus Dqb1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 69
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with untyped donor at C and Dqb1")]
        public virtual void _1010SearchWithUntypedDonorAtCAndDqb1()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with untyped donor at C and Dqb1", ((string[])(null)));
#line 71
  this.ScenarioSetup(scenarioInfo);
#line 72
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 73
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
    testRunner.And("the matching donor is untyped at Locus Dqb1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 81
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a three field truncated allele typed match")]
        public virtual void _1010SearchWithAThreeFieldTruncatedAlleleTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a three field truncated allele typed match", ((string[])(null)));
#line 83
  this.ScenarioSetup(scenarioInfo);
#line 84
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 85
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
    testRunner.And("the matching donor is three field truncated allele typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 92
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a two field truncated allele typed match")]
        public virtual void _1010SearchWithATwoFieldTruncatedAlleleTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a two field truncated allele typed match", ((string[])(null)));
#line 94
  this.ScenarioSetup(scenarioInfo);
#line 95
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 96
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
    testRunner.And("the matching donor is two field truncated allele typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 103
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a XX code typed match")]
        public virtual void _1010SearchWithAXXCodeTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a XX code typed match", ((string[])(null)));
#line 105
  this.ScenarioSetup(scenarioInfo);
#line 106
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 107
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
    testRunner.And("the matching donor is XX code typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 114
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a NMDP code typed match")]
        public virtual void _1010SearchWithANMDPCodeTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a NMDP code typed match", ((string[])(null)));
#line 116
  this.ScenarioSetup(scenarioInfo);
#line 117
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 118
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
    testRunner.And("the matching donor is NMDP code typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a serology typed match")]
        public virtual void _1010SearchWithASerologyTypedMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a serology typed match", ((string[])(null)));
#line 127
  this.ScenarioSetup(scenarioInfo);
#line 128
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 129
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
    testRunner.And("the matching donor is serology typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 136
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a mixed typing resolution")]
        public virtual void _1010SearchWithAMixedTypingResolution()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a mixed typing resolution", ((string[])(null)));
#line 138
  this.ScenarioSetup(scenarioInfo);
#line 139
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 140
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
    testRunner.And("the matching donor is differently typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a homozygous donor")]
        public virtual void _1010SearchWithAHomozygousDonor()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a homozygous donor", ((string[])(null)));
#line 149
  this.ScenarioSetup(scenarioInfo);
#line 150
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 151
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 152
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
    testRunner.And("the matching donor is TGS typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
    testRunner.And("the matching donor homozygous at locus A", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 157
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 158
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 159
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a p group match")]
        public virtual void _1010SearchWithAPGroupMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a p group match", ((string[])(null)));
#line 161
  this.ScenarioSetup(scenarioInfo);
#line 162
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 163
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
    testRunner.And("the matching donor is TGS typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 166
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
    testRunner.And("the match level is p-group", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 171
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10/10 Search with a g group match")]
        public virtual void _1010SearchWithAGGroupMatch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10/10 Search with a g group match", ((string[])(null)));
#line 173
  this.ScenarioSetup(scenarioInfo);
#line 174
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 175
    testRunner.And("the matching donor is a 10/10 match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
    testRunner.And("the matching donor is of type adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
    testRunner.And("the matching donor is TGS typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
    testRunner.And("the matching donor is in registry: Anthony Nolan", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 179
    testRunner.And("the match level is g-group", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
    testRunner.And("the search type is adult", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
    testRunner.And("the search is run against the Anthony Nolan registry only", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 183
    testRunner.Then("the results should contain the specified donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
