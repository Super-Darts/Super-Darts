namespace Assets.Scripts.Extensions
{
    public static class CollectionEx
    {
        /// <summary>
        /// Gets a random element from a array.
        /// </summary>
        /// <typeparam name="T">Type of array</typeparam>
        /// <param name="array">List<T</param>
        /// <returns>A random element</returns>
        public static T RandomElement<T>(this T[] array) => array[new System.Random().Next(0, array.Length)];
    }
}