using System.Globalization;

namespace LLCS.Csv.Tests.CultureAndStyles;

internal sealed class AllowThousandSeparatorAndFrenchCulture : INumberStyleAndCulture
{
    public NumberStyles NumberStyle => NumberStyles.AllowThousands;
    public CultureInfo? CultureInfo => CultureInfo.GetCultureInfo("fr-FR");
}
