﻿using System;
using FluentValidation;
using Atlas.MatchingAlgorithm.Exceptions;
using Atlas.HlaMetadataDictionary.Exceptions;

namespace Atlas.MatchingAlgorithm.ApplicationInsights.DonorProcessing
{
    public static class DonorProcessingFailureEventModelFactory<T> where T : Exception
    {
        public static DonorProcessingFailureEventModel GetEventModel(
            string eventName,
            DonorProcessingException<T> exception)
        {
            switch (exception)
            {
                case DonorProcessingException<ValidationException> validationException:
                    return new DonorInfoValidationFailureEventModel(eventName, validationException);
                case DonorProcessingException<MatchingDictionaryException> hlaException:
                    return new DonorHlaLookupFailureEventModel(eventName, hlaException);
                default:
                    throw new ArgumentOutOfRangeException($"No donor processing failure event model available for exception of type {typeof(T)}.");
            }
        }
    }
}
