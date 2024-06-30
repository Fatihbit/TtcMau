namespace TtcMau.Dtos
{
    public class LadingDto
    {
        public int LadingId { get; set; }
        public string ProductName { get; set; }
        public string TerminalName { get; set; }
        public string ShipName { get; set; }
        public int Hoeveelheid { get; set; }
        public string Tijd { get; set; }
        public DateTime Datum { get; set; }
    }
}
