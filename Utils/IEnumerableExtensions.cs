namespace Utils {
    public static class IEnumerableExtensions {
        public static IEnumerable<KeyValuePair<int, T>> Enumerate<T>(this IEnumerable<T> enumerable, int start = 0) {
            int index = start;
            foreach (var item in enumerable) {
                yield return new(index++, item);
            }
        }
    }
}
