namespace CalculadoraSQIA.Application.Helpers
{
    public static class DiasUteisHelper
    {
        public static bool EhDiaUtil(DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
