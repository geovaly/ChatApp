using System;
using System.Collections.Generic;

namespace ChatApp.Utility
{
    public interface IMaybe
    {
        bool HasValue { get; }

        object Value { get; }

        object NullableValue { get; }
    }

    public struct Maybe<T> : IEquatable<Maybe<T>>, IMaybe
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>();
        public readonly bool HasValue;
        private readonly T _value;

        public Maybe(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _value = value;
            HasValue = true;
        }

        bool IMaybe.HasValue
        {
            get { return HasValue; }
        }

        object IMaybe.Value
        {
            get { return Value; }
        }

        public object NullableValue
        {
            get
            {
                if (HasValue)
                    return _value;
                else
                    return null;
            }
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException(string.Format("Maybe<{0}> must have a value.", typeof(T)));

                return _value;
            }
        }

        public static bool operator !=(Maybe<T> a, Maybe<T> b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Maybe<T> a, Maybe<T> b)
        {
            return a.Equals(b);
        }

        public static implicit operator Maybe<T>(T value)
        {
            return value.ToMaybe();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return HasValue == false;

            return obj is Maybe<T> && Equals((Maybe<T>)obj);
        }

        public bool Equals(Maybe<T> obj)
        {
            if (obj.HasValue != HasValue)
                return false;

            if (!HasValue)
                return true;

            return EqualityComparer<T>.Default.Equals(Value, obj.Value);
        }

        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : 0;
        }

        public T GetValueOrDefault(T defaultValue = default(T))
        {
            return HasValue ? Value : defaultValue;
        }

        public Maybe<TResult> With<TResult>(Func<T, Maybe<TResult>> selector)
        {
            return HasValue ? selector(Value) : Maybe<TResult>.Nothing;
        }

        public Maybe<TResult> With<TResult>(Func<T, TResult> selector)
        {
            return HasValue ? Maybe.ToMaybe(selector(Value)) : Maybe<TResult>.Nothing;
        }

        public Maybe<T> DoIfHasValue(Action<T> action)
        {
            if (HasValue)
                action(Value);

            return this;
        }

        public Maybe<T> DoIfNothing(Action action)
        {
            if (!HasValue)
                action();

            return this;
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : "<Nothing>";
        }

        public T ValueOrDefault(T defaultValue)
        {
            return HasValue ? Value : defaultValue;
        }

        public T ValueOrDefault(Func<T> defaultValueFactory)
        {
            return HasValue ? Value : defaultValueFactory.Invoke();
        }
    }

    public static class Maybe
    {
        public static Maybe<T> As<T>(this object value)
            where T : class
        {
            return (value as T).ToMaybe();
        }

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? Maybe<T>.Nothing : new Maybe<T>(value);
        }

        public static Maybe<T> Just<T>(T value)
        {
            return new Maybe<T>(value);
        }
        public static Maybe<T> Nothing<T>()
        {
            return Maybe<T>.Nothing;
        }

        public static Maybe<T> When<T>(bool condition, T value)
        {
            return condition ? new Maybe<T>(value) : Maybe<T>.Nothing;
        }

        public static Maybe<T> When<T>(bool condition, Func<T> creator)
        {
            if (condition)
                return new Maybe<T>(creator());

            return Maybe<T>.Nothing;
        }
    }
}