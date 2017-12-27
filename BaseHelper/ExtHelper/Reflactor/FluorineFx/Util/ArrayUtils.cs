namespace FluorineFx.Util
{
    using System;

    public abstract class ArrayUtils
    {
        private ArrayUtils()
        {
        }

        public static Array Resize(Array array, int newSize)
        {
            Array destinationArray = Array.CreateInstance(array.GetType().GetElementType(), newSize);
            Array.Copy(array, 0, destinationArray, 0, Math.Min(array.Length, newSize));
            return destinationArray;
        }
    }
}

