/// —————————————————————————————————————————————
//?
//!? 📜 Objects.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
/// —————————————————————————————————————————————


using System.Dynamic;

namespace GalacticLib.Misc {
    /// <summary> Various tools for <see cref="object"/>s </summary>
    public static class Objects {
        /// <summary> Find all subclasses of the type {<typeparamref name="T"/>}</summary>
        public static IEnumerable<Type> FindSubClassesOf<T>() {
            var baseType = typeof(T);
            return baseType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType));
        }

        public static ExpandoObject Combine_ExpandoObjects(ExpandoObject obj1, ExpandoObject obj2) {
            IDictionary<string, object> dict1 = obj1;
            IDictionary<string, object> dict2 = obj2;
            IDictionary<string, object> result = new ExpandoObject();
            foreach (var (Key, Value) in dict1.Concat(dict2))
                result[Key] = Value;
            return (ExpandoObject)result;
        }
    }
}
