namespace LLCS.Csv
{
    internal static class IsSame<T, U>
    {
        public static bool Value => typeof(T) == typeof(U);
    }
}