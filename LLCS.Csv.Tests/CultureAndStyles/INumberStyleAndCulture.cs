using System.Globalization;

namespace LLCS.Csv.Tests.CultureAndStyles;

internal interface INumberStyleAndCulture
{
    NumberStyles NumberStyle { get; }
    CultureInfo? CultureInfo { get; }
}
