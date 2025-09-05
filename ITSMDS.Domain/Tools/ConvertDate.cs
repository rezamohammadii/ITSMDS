using static PersianDateTime;
namespace ITSMDS.Domain.Tools;

public class ConvertDate
{
    public static string ConvertToShamsi(DateTimeOffset timeOffset)
    {

        PersianDateTime persianDateTime = new PersianDateTime(timeOffset.DateTime);
        return persianDateTime.ToString();

    }
}
