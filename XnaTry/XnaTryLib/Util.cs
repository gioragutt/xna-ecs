using System;
using System.Collections.Generic;

namespace XnaTryLib
{
    internal class Util
    {
        static IEnumerable<T> GetEnumValues<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            foreach (var value in values)
                yield return (T)value;
        }
    }
}