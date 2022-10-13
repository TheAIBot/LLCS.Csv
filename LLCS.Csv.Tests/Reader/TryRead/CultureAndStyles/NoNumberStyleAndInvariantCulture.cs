using System.Globalization;

namespace LLCS.Csv.Tests.Reader.TryRead.CultureAndStyles;

internal sealed class NoNumberStyleAndInvariantCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.None;
    public CultureInfo? CultureInfo => CultureInfo.InvariantCulture;
}
