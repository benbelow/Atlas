﻿namespace Atlas.Utils.Hla.Models.HlaTypings
{
    public static class MolecularTypingNameConstants
    {
        public const char Prefix = '*';
        public const char FieldDelimiter = ':';
        public static readonly char[] ExpressionSuffixArray = { 'N', 'C', 'S', 'L', 'Q', 'A' };
        public static readonly string ExpressionSuffixes = string.Join(",", ExpressionSuffixArray);
    }
}
