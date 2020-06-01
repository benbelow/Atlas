﻿namespace Atlas.Common.GeneticData
{
    /// <summary>
    /// Data type to hold an instance of T as a pair of items.
    /// "Item1" and "Item2" are arbitrary.
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the information that is required at each item.</typeparam>
    public class UnorderedPair<T>
    {
        public T Item1 { get; set; }
        public T Item2 { get; set; }

        #region Equality

        /// <summary>
        /// An UnorderedPairs are considered the same regardless of the order of the represented items.
        /// </summary>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((UnorderedPair<T>)other);
        }

        protected bool Equals(UnorderedPair<T> other)
        {
            return Item1.Equals(other.Item2) && Item2.Equals(other.Item1)
                   || Item1.Equals(other.Item1) && Item2.Equals(other.Item2);
        }

        public override int GetHashCode()
        {
            return Item1.GetHashCode() ^ Item2.GetHashCode();
        }

        #endregion
    }
}