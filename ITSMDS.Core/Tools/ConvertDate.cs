using static PersianDateTime;
namespace ITSMDS.Core.Tools;

public class ConvertDate
{
    public static string ConvertToShamsi(DateTimeOffset timeOffset)
    {

        PersianDateTime persianDateTime = new PersianDateTime(timeOffset.DateTime);
        return persianDateTime.ToString();

    }
}
