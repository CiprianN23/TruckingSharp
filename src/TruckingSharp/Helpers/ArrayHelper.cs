namespace TruckingSharp.Helpers
{
    public static class ArrayHelper
    {
        public static T[] InitializeArray<T>(int length) where T : new()
        {
            var array = new T[length];
            for (var i = 0; i < length; ++i) array[i] = new T();

            return array;
        }
    }
}