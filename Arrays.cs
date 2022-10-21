// —————————————————————————————————————————————
//? 
//!? 📜 Arrays.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//? 
// —————————————————————————————————————————————

using System.Linq;

namespace GalacticLib.Misc {
    /// <summary> Various tools for arrays </summary>
    public static class Arrays {
        /// <summary> Add an <paramref name="element"/> item to the end of <paramref name="array"/></summary>
        /// <typeparam name="T">Type of the array to be used</typeparam>
        /// <param name="array">Array to manipulate</param>
        /// <param name="element">Element to add to <paramref name="array"/></param>
        /// <returns>Array {<paramref name="array"/>, <paramref name="element"/>} as <typeparamref name="T"/>[]</returns>
        public static T[] AddArrays<T>(this T[] array, T element)
            => AddArrays(array, new T[] { element });
        /// <summary> Add an <paramref name="expansion"/> array to the end of <paramref name="array"/></summary>
        /// <typeparam name="T">Type of the array to be used</typeparam>
        /// <param name="array">Array to manipulate</param>
        /// <param name="expansion">Element to add to <paramref name="array"/></param>
        /// <returns>Array {<paramref name="array"/>, <paramref name="expansion"/>} as <typeparamref name="T"/>[]</returns>
        public static T[] AddArrays<T>(this T[] array, T[] expansion)
            => array.Concat(expansion).ToArray();

        /// <summary> Determines if <paramref name="values"/> array contains a value of the type {<typeparamref name="TTarget"/>} </summary>
        /// <typeparam name="T">Type of the <paramref name="values"/> array </typeparam>
        /// <typeparam name="TTarget"> Target type inherited from <typeparamref name="T"/> </typeparam>
        public static bool ContainsSubType<T, TTarget>(this T[] values) where TTarget : T {
            foreach (var value in values)
                if (value != null)
                    if (value.GetType() is TTarget)
                        return true;
            return false;
        }
    }
}
