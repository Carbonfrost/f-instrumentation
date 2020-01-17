//
// - LoggerLevelMap.cs -
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
using System.Linq;

using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    public class LoggerLevelMap<T> : IDictionary<LoggerLevel, T> {

        private int version;
        private readonly T[] values = new T[8];

        public T All {
            get { return values[0]; }
            set {
                for (int i = 0; i < values.Length; i++)
                    values[i] = value;
            }
        }

        public T Off {
            get { return this[LoggerLevel.Off]; }
            set { this[LoggerLevel.Off] = value; }
        }

        public T Debug {
            get { return this[LoggerLevel.Debug]; }
            set { this[LoggerLevel.Debug] = value; }
        }

        public T Trace {
            get { return this[LoggerLevel.Trace]; }
            set { this[LoggerLevel.Trace] = value; }
        }

        public T Info {
            get { return this[LoggerLevel.Info]; }
            set { this[LoggerLevel.Info] = value; }
        }

        public T Warn {
            get { return this[LoggerLevel.Warn]; }
            set { this[LoggerLevel.Warn] = value; }
        }

        public T Error {
            get { return this[LoggerLevel.Error]; }
            set { this[LoggerLevel.Error] = value; }
        }

        public T Fatal {
            get { return this[LoggerLevel.Fatal]; }
            set { this[LoggerLevel.Fatal] = value; }
        }

        public T Verbose {
            get { return this[LoggerLevel.Verbose]; }
            set { this[LoggerLevel.Verbose] = value; }
        }

        public LoggerLevelMap() {
        }

        public LoggerLevelMap(T defaultValue) {
            for (int i = 0; i < this.values.Length; i++)
                values[i] = defaultValue;
        }

        public LoggerLevelMap(IEnumerable<KeyValuePair<LoggerLevel, T>> copyFrom, T defaultValue)
            : this(defaultValue) {

            if (copyFrom != null) {
                foreach (var m in copyFrom)
                    this[m.Key] = m.Value;
            }
        }

        public LoggerLevelMap(LoggerLevelMap<T> copyFrom) {
            if (copyFrom == null)
                throw new ArgumentNullException("copyFrom");

            CopyBuffer(copyFrom);
        }

        public bool ContainsValue(T value) {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException("value");

            return this.Values.Contains(value);
        }

        private void CopyBuffer(LoggerLevelMap<T> copyFrom) {
            Array.Copy(copyFrom.values, this.values, this.values.Length);
        }

        // Object overrides
        public override bool Equals(object obj) {
            LoggerLevelMap<T> other = obj as LoggerLevelMap<T>;
            if (other == null)
                return false;

            return this.values.SequenceEqual(other.values);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                foreach (var t in this.values)
                    hashCode += 109 * (ReferenceEquals(t, null) ? 37 : t.GetHashCode());
            }
            return hashCode;
        }

        // IDictionary implementation
        public bool ContainsKey(LoggerLevel key) {
            if (key == null)
                throw new ArgumentNullException("key");

            return this.Keys.Contains(key);
        }

        void IDictionary<LoggerLevel, T>.Add(LoggerLevel key, T value) {
            throw new NotSupportedException();
        }

        bool IDictionary<LoggerLevel, T>.Remove(LoggerLevel key) {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<LoggerLevel, T>>.Add(KeyValuePair<LoggerLevel, T> item) {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<LoggerLevel, T>>.Clear() {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<LoggerLevel, T>>.Remove(KeyValuePair<LoggerLevel, T> item) {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T this[LoggerLevel key] {
            get {
                return this.values[key.ToInt32()];
            }
            set {
                this.values[key.ToInt32()] = value;
                version++;
            }
        }

        public ICollection<LoggerLevel> Keys {
            get {
                return LoggerLevel.AllValues;
            }
        }

        public ICollection<T> Values {
            get { return this.values; } }

        public int Count {
            get { return this.values.Length; } }

        public bool IsReadOnly {
            get; private set;
        }

        public bool TryGetValue(LoggerLevel key, out T value) {
            value = this.values[key.ToInt32()];
            return true;
        }

        public bool Contains(KeyValuePair<LoggerLevel, T> item) {
            return object.Equals(this.values[item.Key.ToInt32()], item.Value);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            this.values.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<LoggerLevel, T>[] array, int arrayIndex) {
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw Failure.IndexOutOfRange("arrayIndex", arrayIndex, 0, array.Length - 1);
            if (array.Rank != 1)
                throw Failure.RankNotOne("array");
            if (array.Length - arrayIndex < this.Count)
                throw Failure.NotEnoughSpaceInArray("arrayIndex", arrayIndex);

            foreach (var kvp in this)
                array[arrayIndex++] = kvp;
        }

        public IEnumerator<KeyValuePair<LoggerLevel, T>> GetEnumerator() {
            int startingVersion = this.version;
            int index = 0;

            foreach (T t in this.values) {
                if (startingVersion != this.version)
                    throw Failure.CollectionModified();

                yield return new KeyValuePair<LoggerLevel, T>(LoggerLevel.AllValues[index++], t);
            }
        }

        internal void MakeReadOnly() {
            this.IsReadOnly = true;
        }

        public LoggerLevelMap<T> Clone() {
            return CloneCore();
        }

        protected virtual LoggerLevelMap<T> CloneCore() {
            return new LoggerLevelMap<T>(this, default(T));
        }
    }
}
