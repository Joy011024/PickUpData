namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class ValidationUtils
    {
        private ValidationUtils()
        {
        }

        public static void ArgumentConditionTrue(bool condition, string parameterName, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void ArgumentIsPositive<T>(T value, string parameterName) where T: struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) != 1)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Positive number required.");
            }
        }

        public static void ArgumentIsPositive(int value, string parameterName, string message)
        {
            if (value > 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        public static void ArgumentNotNegative(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Argument cannot be negative.");
            }
        }

        public static void ArgumentNotNegative(int value, string parameterName, string message)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName)
        {
            ArgumentNotNullOrEmpty<T>(collection, parameterName, string.Format("Collection '{0}' cannot be empty.", parameterName));
        }

        public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName)
        {
            ArgumentNotNullOrEmpty(collection, parameterName, string.Format("Collection '{0}' cannot be empty.", parameterName));
        }

        public static void ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (value.Length == 0)
            {
                throw new ArgumentException(string.Format("'{0}' cannot be empty.", parameterName), parameterName);
            }
        }

        public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName, string message)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (collection.Count == 0)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName, string message)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (collection.Count == 0)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void ArgumentNotNullOrEmptyOrWhitespace(string value, string parameterName)
        {
            ArgumentNotNullOrEmpty(value, parameterName);
            if (StringUtils.IsWhiteSpace(value))
            {
                throw new ArgumentException(string.Format("'{0}' cannot only be whitespace.", parameterName), parameterName);
            }
        }

        public static void ArgumentNotZero(int value, string parameterName)
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Argument cannot be zero.");
            }
        }

        public static void ArgumentNotZero(int value, string parameterName, string message)
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
        {
            ArgumentNotNull(enumType, "enumType");
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an Enum.", enumType), parameterName);
            }
        }

        public static void ObjectNotDisposed(bool disposed, Type objectType)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(objectType.Name);
            }
        }

        public static void ObjectNotNull(object value, string variableName)
        {
            if (value == null)
            {
                throw new NullReferenceException(string.Format("{0} cannot be null.", variableName));
            }
        }
    }
}

