using System.Globalization;
using Godot;

namespace Project.Assets.Scripts.Utilities
{
    public static class PrettyNumbers
    {
        public static string Format(float numberToFormat)
        {
            var number = Mathf.Abs(numberToFormat);
            var modifier = numberToFormat >= 0 ? 1.0f : -1.0f;
            if (number < 999.0f)
            {
                return NumberToPrettyString(number * modifier, string.Empty);
            }
            else if (number is >= 1000.0f and < 1000000.0f)
            {
                var formatted_number = Mathf.Snapped(number / 1000.0f, 0.1f) * modifier;
                return NumberToPrettyString(formatted_number, "K");
            }
            else if (number >= 1000000.0f)
            {
                var formatted_number = Mathf.Snapped(number / 1000000.0f, 0.1f) * modifier;
                return NumberToPrettyString(formatted_number, "M");
            }
            return NumberToPrettyString(number * modifier, string.Empty);
        }

        private static string NumberToPrettyString(float number, string suffix)
        {
            var stringNumber = number.ToString(CultureInfo.InvariantCulture);
            return $"{stringNumber}{suffix}";
        }
    }
}
