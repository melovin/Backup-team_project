namespace DatabaseTest.DataClasses
{
    public enum Freq
    {
        NEVER,
        DAILY,
        WEEKLY,
        MONTHLY
    }
    public class EmailSettings
    {
        public int Port { get; set; }
        public string SMTP { get; set; }
        public Freq Freq { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public bool SSL { get; set; }
    }
}
