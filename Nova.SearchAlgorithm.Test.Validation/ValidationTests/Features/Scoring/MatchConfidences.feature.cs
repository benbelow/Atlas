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
namespace Nova.SearchAlgorithm.Test.Validation.ValidationTests.Features.Scoring
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Scoring - Match Confidences")]
    public partial class Scoring_MatchConfidencesFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "MatchConfidences.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Scoring - Match Confidences", "  As a member of the search team\r\n  I want search results to have an appropriate " +
                    "match confidence", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Definite match at each locus - patient and donor unambiguously typed")]
        public virtual void DefiniteMatchAtEachLocus_PatientAndDonorUnambiguouslyTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Definite match at each locus - patient and donor unambiguously typed", ((string[])(null)));
#line 5
  this.ScenarioSetup(scenarioInfo);
#line 6
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.And("the matching donor is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("the patient is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
    testRunner.Then("the match confidence should be Definite at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exact match at each locus - patient unambiguously typed and donor ambiguously (si" +
            "ngle P group) typed")]
        public virtual void ExactMatchAtEachLocus_PatientUnambiguouslyTypedAndDonorAmbiguouslySinglePGroupTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exact match at each locus - patient unambiguously typed and donor ambiguously (si" +
                    "ngle P group) typed", ((string[])(null)));
#line 12
  this.ScenarioSetup(scenarioInfo);
#line 13
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
    testRunner.And("the matching donor is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("the patient is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
    testRunner.Then("the match confidence should be Exact at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exact match at each locus - patient ambiguously (single P group) typed and donor " +
            "unambiguously typed")]
        public virtual void ExactMatchAtEachLocus_PatientAmbiguouslySinglePGroupTypedAndDonorUnambiguouslyTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exact match at each locus - patient ambiguously (single P group) typed and donor " +
                    "unambiguously typed", ((string[])(null)));
#line 19
  this.ScenarioSetup(scenarioInfo);
#line 20
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.And("the matching donor is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.And("the patient is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
    testRunner.Then("the match confidence should be Exact at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exact match at each locus - patient and donor ambiguously (single P group) typed")]
        public virtual void ExactMatchAtEachLocus_PatientAndDonorAmbiguouslySinglePGroupTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exact match at each locus - patient and donor ambiguously (single P group) typed", ((string[])(null)));
#line 26
  this.ScenarioSetup(scenarioInfo);
#line 27
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
    testRunner.And("the matching donor is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
    testRunner.And("the patient is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
    testRunner.Then("the match confidence should be Exact at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match at each locus - patient unambiguously typed and donor ambiguously" +
            " (multiple P groups) typed")]
        public virtual void PotentialMatchAtEachLocus_PatientUnambiguouslyTypedAndDonorAmbiguouslyMultiplePGroupsTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match at each locus - patient unambiguously typed and donor ambiguously" +
                    " (multiple P groups) typed", ((string[])(null)));
#line 33
  this.ScenarioSetup(scenarioInfo);
#line 34
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 35
    testRunner.And("the matching donor is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.And("the patient is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
    testRunner.Then("the match confidence should be Potential at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match at each locus - patient ambiguously (multiple P groups) typed and" +
            " donor unambiguously typed")]
        public virtual void PotentialMatchAtEachLocus_PatientAmbiguouslyMultiplePGroupsTypedAndDonorUnambiguouslyTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match at each locus - patient ambiguously (multiple P groups) typed and" +
                    " donor unambiguously typed", ((string[])(null)));
#line 40
  this.ScenarioSetup(scenarioInfo);
#line 41
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
 testRunner.And("the matching donor is unambiguously typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("the patient is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
    testRunner.Then("the match confidence should be Potential at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match at each locus - patient ambiguously (single P group) typed and do" +
            "nor ambiguously (multiple P groups) typed")]
        public virtual void PotentialMatchAtEachLocus_PatientAmbiguouslySinglePGroupTypedAndDonorAmbiguouslyMultiplePGroupsTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match at each locus - patient ambiguously (single P group) typed and do" +
                    "nor ambiguously (multiple P groups) typed", ((string[])(null)));
#line 47
  this.ScenarioSetup(scenarioInfo);
#line 48
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
    testRunner.And("the matching donor is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.And("the patient is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
    testRunner.Then("the match confidence should be Potential at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match at each locus - patient ambiguously (multiple P groups) typed and" +
            " donor ambiguously (single P group) typed")]
        public virtual void PotentialMatchAtEachLocus_PatientAmbiguouslyMultiplePGroupsTypedAndDonorAmbiguouslySinglePGroupTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match at each locus - patient ambiguously (multiple P groups) typed and" +
                    " donor ambiguously (single P group) typed", ((string[])(null)));
#line 54
  this.ScenarioSetup(scenarioInfo);
#line 55
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
    testRunner.And("the matching donor is ambiguously (single P group) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
    testRunner.And("the patient is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
    testRunner.Then("the match confidence should be Potential at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match at each locus - patient and donor ambiguously (multiple P groups)" +
            " typed")]
        public virtual void PotentialMatchAtEachLocus_PatientAndDonorAmbiguouslyMultiplePGroupsTyped()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match at each locus - patient and donor ambiguously (multiple P groups)" +
                    " typed", ((string[])(null)));
#line 61
  this.ScenarioSetup(scenarioInfo);
#line 62
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
    testRunner.And("the matching donor is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
    testRunner.And("the patient is ambiguously (multiple P groups) typed at each locus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
    testRunner.When("I run a 10/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 66
    testRunner.Then("the match confidence should be Potential at each locus at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match - donor untyped at C")]
        public virtual void PotentialMatch_DonorUntypedAtC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match - donor untyped at C", ((string[])(null)));
#line 68
  this.ScenarioSetup(scenarioInfo);
#line 69
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
    testRunner.When("I run a 6/6 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 72
    testRunner.Then("the match confidence should be Potential at C at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match - patient untyped at C")]
        public virtual void PotentialMatch_PatientUntypedAtC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match - patient untyped at C", ((string[])(null)));
#line 74
  this.ScenarioSetup(scenarioInfo);
#line 75
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 76
    testRunner.And("the patient is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
    testRunner.When("I run a 6/6 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
    testRunner.Then("the match confidence should be Potential at C at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Potential match - patient and donor untyped at C")]
        public virtual void PotentialMatch_PatientAndDonorUntypedAtC()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Potential match - patient and donor untyped at C", ((string[])(null)));
#line 80
  this.ScenarioSetup(scenarioInfo);
#line 81
    testRunner.Given("a patient has a match", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 82
    testRunner.And("the matching donor is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
    testRunner.And("the patient is untyped at Locus C", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
    testRunner.When("I run a 6/6 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 85
    testRunner.Then("the match confidence should be Potential at C at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Mismatch confidence - double mismatch at locus A")]
        public virtual void MismatchConfidence_DoubleMismatchAtLocusA()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Mismatch confidence - double mismatch at locus A", ((string[])(null)));
#line 87
  this.ScenarioSetup(scenarioInfo);
#line 88
    testRunner.Given("a patient and a donor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 89
    testRunner.And("the donor has a double mismatch at locus A", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
    testRunner.When("I run an 8/10 search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 91
    testRunner.Then("the match confidence should be Mismatch at A at both positions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
