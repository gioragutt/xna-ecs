using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilsLib.Utility
{
    public static partial class Utils
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return from object value in Enum.GetValues(typeof(T)) select (T)value;
        }

        public static bool NotNull(params object[] objects)
        {
            return objects.All(obj => !ReferenceEquals(obj, null));
        }

        public static void AssertArgumentNotNull(object argument, string parameterName)
        {
            if (ReferenceEquals(argument, null))
                throw new ArgumentNullException(parameterName);
        }

        public static void AssertStringArgumentNotNull(string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(parameterName);
        }

        public static IList<string> FormatCollection(string format, params object[] collection)
        {
            return collection.Select(item => string.Format(format, item)).ToList();
        }

        public static IList<string> FormatRange(string format, int min, int max)
        {
            var diff = max - min;
            var itr = Math.Sign(diff);
            var items = new List<int>();
            for (var i = min; i != max + itr; i += itr)
            {
                items.Add(i);
            }
            return items.Select(item => string.Format(format, item)).ToList();
        }
    }
}
