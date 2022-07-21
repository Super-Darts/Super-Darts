/*
 *  Author: James Greensill.
 *  Usage:  This is a simple data structure to hold a min & max range.
 */

using System;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    /// <summary>
    /// A class that implements a Min & Max Numerical Range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public struct Range<T> where T : struct, IComparable,
        IComparable<T>,
        IConvertible,
        IEquatable<T>,
        IFormattable

    {
        /// <summary>
        /// Minimum Value of the Range.
        /// </summary>
        [field: SerializeField] public T Min { get; set; }

        /// <summary>
        /// Maximum Value of the Range.
        /// </summary>
        [field: SerializeField] public T Max { get; set; }

        public override string ToString() => $"({Min} - {Max})";

        public bool Bounds(T other)
        {
            return Min.CompareTo(other) <= 0 && Max.CompareTo(other) >= 0;
        }

        public bool Equals(Range<T> other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            return obj is Range<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }

        public static bool operator >(Range<T> lhs, Range<T> rhs)
        {
            return lhs.Max.CompareTo(rhs.Min) < 0;
        }

        public static bool operator <(Range<T> lhs, Range<T> rhs)
        {
            return lhs.Min.CompareTo(rhs.Max) > 0;
        }

        public static bool operator ==(Range<T> lhs, Range<T> rhs)
        {
            return lhs.Min.CompareTo(rhs.Min) == 0 && lhs.Max.CompareTo(rhs.Max) == 0;
        }

        public static bool operator !=(Range<T> lhs, Range<T> rhs)
        {
            return lhs.Min.CompareTo(rhs.Min) != 0 && lhs.Max.CompareTo(rhs.Max) != 0;
        }
    }
}