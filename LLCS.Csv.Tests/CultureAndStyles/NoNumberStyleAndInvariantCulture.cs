using System.Globalization;

namespace LLCS.Csv.Tests.CultureAndStyles;

internal sealed class NoNumberStyleAndInvariantCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.None;
    public CultureInfo? CultureInfo => CultureInfo.InvariantCulture;
}
