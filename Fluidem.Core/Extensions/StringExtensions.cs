namespace Fluidem.Core.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string textVar)
        {
            return int.Parse(textVar);
        }
    }
}