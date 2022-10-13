using System.Globalization;

namespace LLCS.Csv.Tests.CultureAndStyles;

internal sealed class AllowThousandSeparatorAndInvariantCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.AllowThousands;
    public CultureInfo? CultureInfo => CultureInfo.InvariantCulture;
}
