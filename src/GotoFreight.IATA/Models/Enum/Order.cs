namespace GotoFreight.IATA.Models.Enum;

public enum OrderStaus
{
    Awaiting,
    Received,
    Completed,
    Cancelled
}

public static class OrderStausX
{
    public static int ToInt(this OrderStaus status)
    {
        return (int)status;
    }
}