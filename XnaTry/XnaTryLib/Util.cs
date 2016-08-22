using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ECS.Interfaces;
using Microsoft.Xna.Framework;

namespace XnaCommonLib
{
    public static class Util
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return from object value in Enum.GetValues(typeof(T)) select (T)value;
        }

        public static bool NotNull(params object[] objects)
        {
            return objects.All(obj => !ReferenceEquals(obj, null));
        }

        public static bool ComponentsEnabled(params IComponent[] components)
        {
            return components.All(comp => !ReferenceEquals(comp, null) && comp.Enabled);
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

        public static void WriteString(BinaryWriter writer, string str)
        {
            writer.Write(str.Length);
            writer.Write(Encoding.ASCII.GetBytes(str));
        }

        public static string ReadString(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            return Encoding.ASCII.GetString(reader.ReadBytes(length));
        }

        public static void WriteVector2(BinaryWriter writer, Vector2 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
        }

        public static Vector2 ReadVector2(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            return new Vector2(x, y);
        }

        public static void WriterGuid(BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }

        public static Guid ReadGuid(BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }
    }
}