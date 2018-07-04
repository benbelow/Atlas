﻿using System;
using System.Net;
using Nova.Utils.Http.Exceptions;

namespace Nova.SearchAlgorithm.MatchingDictionary.Exceptions
{
    public class MatchingDictionaryHttpException : NovaHttpException
    {
        public MatchingDictionaryHttpException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public MatchingDictionaryHttpException(string message, Exception inner)
            : base(HttpStatusCode.InternalServerError, message, inner)
        {
        }
    }
}