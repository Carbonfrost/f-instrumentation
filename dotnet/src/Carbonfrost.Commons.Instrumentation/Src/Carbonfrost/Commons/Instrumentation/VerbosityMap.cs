//
// - VerbosityMap.cs -
//
// Copyright 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class VerbosityMap<T> : IDictionary<Verbosity, T> {

        private int version;
        private readonly T[] values = new T[8];

        public T All {
            get { return this[0]; }
            set {
                for (int i = 0; i < values.Length; i++)
                    values[i] = value;
            }
        }

        public T Quiet {
            get { return this[Verbosity.Quiet]; }
            set { this[Verbosity.Quiet] = value; }
        }

        public T Minimal {
            get { return this[Verbosity.Minimal]; }
            set { this[Verbosity.Minimal] = value; }
        }

        public T Normal {
            get { return this[Verbosity.Normal]; }
            set { this[Verbosity.Normal] = value; }
        }

        public T Detailed {
            get { return this[Verbosity.Detailed]; }
            set { this[Verbosity.Detailed] = value; }
        }

        public T Diagnostic {
            get { return this[Verbosity.Diagnostic]; }
            set { this[Verbosity.Diagnostic] = value; }
        }

        public VerbosityMap() {
        }

        public VerbosityMap(T defaultValue) {
            for (int i = 0; i < this.values.Length; i++)
                values[i] = defaultValue;
        }

        public bool Contains(Verbosity key) {
            return this.Keys.Contains(key);
        }

        internal void CopyBuffer(VerbosityMap<T> copyFrom) {
            Array.Copy(copyFrom.values, this.values, this.values.Length);
        }

        // Object overrides
        public override bool Equals(object obj) {
            VerbosityMap<T> other = obj as VerbosityMap<T>;
            if (other == null)
                return false;

            return this.values.SequenceEqual(other.values);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                foreach (var t in this.values)
                    hashCode += 109 * ((t == null) ? 37 : t.GetHashCode());
            }
            return hashCode;
        }


        internal void CopyFrom(VerbosityMap<T> sampleRates) {
            sampleRates.values.CopyTo(this.values, 0);
        }

        // IDictionary implementation
        public bool ContainsKey(Verbosity key) {
            return this.Keys.Contains(key);
        }

        void IDictionary<Verbosity, T>.Add(Verbosity key, T value) {
            throw new NotSupportedException();
        }

        bool IDictionary<Verbosity, T>.Remove(Verbosity key) {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<Verbosity, T>>.Add(KeyValuePair<Verbosity, T> item) {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<Verbosity, T>>.Clear() {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<Verbosity, T>>.Remove(KeyValuePair<Verbosity, T> item) {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T this[Verbosity key] {
            get {
                return this.values[(int) key];
            }
            set {
                this.values[(int) key] = value;
                version++;
            }
        }

        public ICollection<Verbosity> Keys {
            get {
                return AllVerbosityValues();
            }
        }

        public ICollection<T> Values {
            get { return this.values; } }

        public int Count {
            get { return this.values.Length; } }

        public bool IsReadOnly {
            get { return false; } }

        public bool TryGetValue(Verbosity key, out T value) {
            value = this.values[(int) key];
            return true;
        }

        public bool Contains(KeyValuePair<Verbosity, T> item) {
            return object.Equals(this.values[(int) item.Key], item.Value);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            this.values.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<Verbosity, T>[] array, int arrayIndex) {
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw Failure.IndexOutOfRange("arrayIndex", arrayIndex, 0, array.Length - 1);
            if (array.Rank != 1)
                throw Failure.RankNotOne("array");
            if (array.Length - arrayIndex < this.Count)
                throw Failure.NotEnoughSpaceInArray("arrayIndex", arrayIndex);

            foreach (var kvp in this)
                array[arrayIndex++] = kvp;
        }

        public IEnumerator<KeyValuePair<Verbosity, T>> GetEnumerator() {
            int startingVersion = this.version;
            int index = 0;

            foreach (T t in this.values) {
                if (startingVersion != this.version)
                    throw Failure.CollectionModified();

                yield return new KeyValuePair<Verbosity, T>(AllVerbosityValues()[index++], t);
            }
        }

        static Verbosity[] AllVerbosityValues() {
            return (Verbosity[]) Enum.GetValues(typeof(Verbosity));
        }

    }
}
