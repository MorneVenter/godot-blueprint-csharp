namespace Blueprint;

public static class PrettyNumbers
{
    public static string Format(double numberToFormat)
    {
        var number = Math.Abs(numberToFormat);
        var modifier = numberToFormat >= 0 ? 1.0 : -1.0;

        return number switch
        {
            < 999.0 => (number * modifier).ToString(),
            < 1_000_000.0 => $"{Math.Round(number / 1_000.0, 1) * modifier}K",
            _ => $"{Math.Round(number / 1_000_000.0, 1) * modifier}M",
        };
    }
}
