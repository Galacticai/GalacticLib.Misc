// —————————————————————————————————————————————
//?
//!? 📜 Mask.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


namespace GalacticLib.Misc {
    /// <summary> Masks the original value with a different one,
    /// <br/> while keeping the original value safely as <see cref="OriginalValue"/>
    /// <br/><br/>
    /// _______________
    /// <br/>
    /// To get the currently active value of this mask:
    /// <br/><list type="bullet">
    ///     <item> (Implicit) `<c> mask </c>`  (Not all types work with this)</item>
    ///     <item> (Explicit) `<c> (<typeparamref name="TValue"/>)mask </c>` </item>
    ///     <item>   (Direct) `<c> <see cref="Value"/> </c>` </item>
    /// </list></summary>
    /// <typeparam name="TMaskKey"> Type of the key of <see cref="MaskFunctions"/> </typeparam>
    /// <typeparam name="TValue"> Type of the <see cref="Value"/> </typeparam>
    public class Mask<TMaskKey, TValue> where TMaskKey : notnull {
        /// <summary> Clear <see cref="MaskFunctions"/> </summary>
        /// <returns> <see cref="OriginalValue"/> </returns>
        public TValue Reset() {
            MaskFunctions.Clear();
            return OriginalValue;
        }
        /// <summary> The base value that is masked with <see cref="MaskFunctions"/> </summary>
        public TValue OriginalValue { get; }

        /// <summary> Functions that warp the result of the <see cref="OriginalValue"/> and produce a modified version of it </summary>
        public Dictionary<TMaskKey, Func<TValue, TValue>> MaskFunctions { get; set; }
        /// <summary> Generates a warped version of <see cref="OriginalValue"/> using <see cref="MaskFunctions"/> </summary>
        /// <returns> The modified version of <see cref="OriginalValue"/> </returns>
        public TValue Value
            => MaskFunctions.Values.Aggregate(
                OriginalValue,
                (current, maskFunction) => maskFunction(current)
            );

        /// <summary> Masks the original value with a different one,
        /// <br/> while keeping the original value safely as <see cref="OriginalValue"/> </summary>
        public Mask(TValue originalValue, Dictionary<TMaskKey, Func<TValue, TValue>> maskFunctions) {
            OriginalValue = originalValue;
            MaskFunctions = maskFunctions;
        }
        /// <summary> Masks the original value with a different one,
        /// <br/> while keeping the original value safely as <see cref="OriginalValue"/> </summary>
        public Mask(TValue originalValue)
                    : this(originalValue, new()) { }

        //!? ! Data loss warning: Don't convert from TValue to Mask implicitly
        // public static implicit operator Mask<TMaskKey, TValue>(TValue value)
        //     => value;
        /// <summary> (implicit) Gets the <see cref="Value"/> or <paramref name="mask"/> </summary>
        public static implicit operator TValue(Mask<TMaskKey, TValue> mask)
            => mask.Value;
    }
}
