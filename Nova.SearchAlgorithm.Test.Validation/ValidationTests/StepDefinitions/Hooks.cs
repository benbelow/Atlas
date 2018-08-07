﻿using Nova.SearchAlgorithm.Data;
using Nova.SearchAlgorithm.Test.Validation.TestData;
using TechTalk.SpecFlow;

namespace Nova.SearchAlgorithm.Test.Validation.ValidationTests.StepDefinitions
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            TestDataService.SetupTestData();
            AlgorithmTestingService.StartServer();
            AlgorithmTestingService.RunHlaRefresh();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            AlgorithmTestingService.StopServer();
        }
        
    }
}
