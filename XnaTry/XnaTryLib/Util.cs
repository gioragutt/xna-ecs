﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaTryLib
{
    internal static class Util
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return from object value in Enum.GetValues(typeof(T)) select (T)value;
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
    }
}