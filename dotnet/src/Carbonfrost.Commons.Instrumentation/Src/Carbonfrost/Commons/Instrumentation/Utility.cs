//
// Copyright 2010 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    static class Utility {

        public static string CreatePropertyQuery(string filter) {
            return "//" + filter.Replace(".", "/");
        }

        public static void AddMany<T>(this ICollection<T> target, IEnumerable<T> items) {
            foreach (var o in items)
                target.Add(o);
        }

        public static void SetProperty<T>(this PropertyTree source, string property, T value) {
            Property node = (Property) source[property];

            if (node == null) {
                source.AppendProperty(property);
                node = (Property) source[property];
            }

            node.Value = value;
        }

        public static T GetPropertyOrDefault<T>(this PropertyTree source, string property, T defaultValue) {
            PropertyNode node = source[property];

            if (node == null || node.Value == null)
                return defaultValue;

            else if (node.IsProperty)
                return (T) ValueSerializer.GetValueSerializer(typeof(T))
                    .ConvertFromString(Convert.ToString(node.Value), typeof(T), null);
            else
                return (T) node.Value;
        }

        public static TValue GetValueOrCache<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                           TKey key,
                                                           Func<TValue> func) {
            TValue result;
            if (source.TryGetValue(key, out result))
                return result;
            else
                return source[key] = func();
        }

        public static TValue GetValueOrCache<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                           TKey key,
                                                           TValue defaultValue) {
            TValue result;
            if (source.TryGetValue(key, out result))
                return result;
            else
                return source[key] = defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                             TKey key,
                                                             TValue defaultValue) {
            TValue result;
            if (source.TryGetValue(key, out result))
                return result;
            else
                return defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                             TKey key) {
            TValue result;
            if (source.TryGetValue(key, out result))
                return result;
            else
                return default(TValue);
        }

        public static void CheckBuffer(LoggerEvent[] buffer,
                                       int index,
                                       int length) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (index < 0 || index >= buffer.Length)
                throw Failure.IndexOutOfRange("index", index, 0, buffer.Length - 1);
            if (length < 0)
                throw Failure.Negative("length", length);
            if (length < 0 || (length > (buffer.Length - index)))
                throw Failure.CountOutOfRange("buffer", index, 0, buffer.Length - 1);
        }

        public static void Dispose(object o) {
            IDisposable d = o as IDisposable;
            if (d != null)
                d.Dispose();
        }

        public static StackFrame FindStackFrame(bool needFileInfo) {
#if NET
            StackTrace st = new StackTrace(1, needFileInfo);
            foreach (var sf in st.GetFrames()) {
                Type declaringType = sf.GetMethod().DeclaringType;
                if (!declaringType.IsDefined(typeof(SkipFramesAttribute), false))
                    return sf;
            }
#endif
            return null;
        }

        public static string SafeFilePath(string t) {
            StringBuilder sb = new StringBuilder(t);

            foreach (var c in Path.GetInvalidPathChars())
                sb.Replace(c, '_');

            return sb.ToString();
        }
    }
}
