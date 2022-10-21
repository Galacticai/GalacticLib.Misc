// —————————————————————————————————————————————
//?
//!? 📜 TypeDictionary.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


namespace GalacticLib.Misc {
    /// <summary> Dictionary of <typeparamref name="TValue"/> with the <see cref=" Type"/> of <typeparamref name="TValue"/> as the key
    /// <br/> ⚠️ Warning: If you repeat the same <typeparamref name="TValue"/>, the first one will be applied. </summary>
    public class TypeDictionary<TValue> {
        #region Shortcuts

        /// <summary> Count of elements stored in this dictionary </summary>
        public int Count => Dictionary.Count;
        
        /// <summary> The key collection of this dictionary</summary>
        public Dictionary<Type, TValue>.KeyCollection Keys => Dictionary.Keys;
        /// <summary> The value collection of this dictionary</summary>
        public Dictionary<Type, TValue>.ValueCollection Values => Dictionary.Values;
        /// <summary> Get an element  </summary>
        public TValue this[Type type] => Dictionary[type];

        #endregion
        #region Methods
        /// <summary> Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its entries. </summary>
        public void TrimExcess() => Dictionary.TrimExcess();
        /// <summary> Removes all keys and values from this dictionary </summary>
        public virtual void Clear() => Dictionary.Clear();
        /// <summary> Removes the value with the specified key from this dictionary </summary>
        /// <typeparam name="ITValue"> Type inherited from <typeparamref name="TValue"/> </typeparam>
        /// <returns> true if the element is successfully found and removed; otherwise, false.
        /// <br/> This method returns false if <typeparamref name="ITValue"/> (type key) is not found in this dictionary.</returns>
        public virtual bool Remove<ITValue>() where ITValue : TValue
            => Dictionary.Remove(typeof(ITValue));
        /// <summary> Gets the value associated with the specified <typeparamref name="ITValue"/> (type key). </summary>
        /// <typeparam name="ITValue"> Type inherited from <typeparamref name="TValue"/> </typeparam>
        /// <returns> The value; Otherwise, <c>default(<typeparamref name="ITValue"/>)</c>. </returns>
            if (!contains) return default;
            return (ITValue)value;
        }
        public virtual bool Set<ITValue>(ITValue value) where ITValue : TValue
            => Set(value, false);
        /// <summary> Sets the value associated with the specified <typeparamref name="ITValue"/> (type key) </summary>
        /// <typeparam name="ITValue"> Type inherited from <typeparamref name="TValue"/> </typeparam>
        /// <returns> true if the operation is complete </returns>
        public virtual bool Set<ITValue>(ITValue value, bool force) where ITValue : TValue {
            if (value is null) return false;
            if (Contains<ITValue>() && !force) return false;
            Dictionary[value.GetType()] = value;
            return true;
        }
        /// <summary> Determines whether this dictionary contains the specified <typeparamref name="ITValue"/> (type key) </summary>
        /// <typeparam name="ITValue"> Type inherited from <typeparamref name="TValue"/> </typeparam>
        /// <returns> true if the operation is complete </returns>
        public bool Contains<ITValue>() where ITValue : TValue
            => Dictionary.ContainsKey(typeof(ITValue));
        /// <summary> Determines whether this dictionary contains the specified value of type <typeparamref name="ITValue"/> (type key) </summary>
        /// <typeparam name="ITValue"> Type inherited from <typeparamref name="TValue"/> </typeparam>
        /// <returns> true if the operation is complete </returns>
        public bool ContainsValue<ITValue>(ITValue value) where ITValue : TValue {
            //? No need to loop through all values
            //? X Dictionary.ContainsValue(value)
            //? > Directly:
            //?     + Check if type exists
            //?     + Compare value of that key to the provided value
            ITValue value_FromDictionary = Get<ITValue>();
            if (value_FromDictionary == null) return false;
            return Comparer<ITValue>.Default.Compare(value, value_FromDictionary) >= 0;
        }

        #endregion

        private Dictionary<Type, TValue> Dictionary { get; set; }
        /// <summary> ℹ️ Note: If you repeat the same type more than once, the last one will be applied. </summary>
        public TypeDictionary(params TValue[] values) {
            Dictionary = new();
            foreach (var value in values) Set(value);
        }

        /// <summary> Dictionary of <typeparamref name="TValue"/> with the <see cref=" Type"/> of <typeparamref name="TValue"/> as the key
        /// <br/> ⚠️ Warning: If you repeat the same <typeparamref name="TValue"/>, the first one will be applied. </summary>
        public TypeDictionary(Dictionary<Type, TValue> dictionary) {
            Dictionary = new();
            foreach (var value in dictionary.Values) Set(value);
        }
        /// <summary> Dictionary of <typeparamref name="TValue"/> with the <see cref=" Type"/> of <typeparamref name="TValue"/> as the key
        /// <br/> ⚠️ Warning: If you repeat the same <typeparamref name="TValue"/>, the first one will be applied. </summary>
        public TypeDictionary(List<TValue> list) {
            Dictionary = new();
            foreach (var value in list) Set(value);
        }
        /// <summary> Dictionary of <typeparamref name="TValue"/> with the <see cref=" Type"/> of <typeparamref name="TValue"/> as the key </summary>
        public TypeDictionary() {
            Dictionary = new();
        }

        #region Conversion
        /// <summary> Generates a list from the <see cref="Values"/> of this dictionary </summary>
        public List<TValue> ToList() => Values.ToList();
        /// <summary> Generates a regular <see cref="Dictionary{Type, TValue}"/> from this <see cref="TypeDictionary{TValue}"/> </summary>
        public Dictionary<Type, TValue> ToDictionary() => new(Dictionary);

        /// <summary> (implicit) Generates a list from the <see cref="Values"/> of this dictionary </summary>
        public static implicit operator List<TValue>(TypeDictionary<TValue> typeDictionary)
            => typeDictionary.ToList();
        /// <summary> (implicit) Generates a regular <see cref="Dictionary{Type, TValue}"/> from this <see cref="TypeDictionary{TValue}"/> </summary>
        public static implicit operator Dictionary<Type, TValue>(TypeDictionary<TValue> typeDictionary)
            => typeDictionary.ToDictionary();
        /// <summary> (implicit) Generates a <see cref="TypeDictionary{TValue}"/> from a <see cref="Dictionary{Type, TValue}"/> </summary>
        public static implicit operator TypeDictionary<TValue>(Dictionary<Type, TValue> dictionary)
            => new(dictionary.Values.ToArray());
        /// <summary> (implicit) Generates a <see cref="TypeDictionary{TValue}"/> from a <see cref="List{TValue}"/> </summary>
        public static implicit operator TypeDictionary<TValue>(List<TValue> list)
            => new(list);
        #endregion
    }
}
