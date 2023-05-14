using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSender
{
    public enum Freq
    {
        NEVER,
        DAILY,
        WEEKLY,
        MONTHLY
    }
    public class SettingsInfo
    {
        public int Port { get; set; } = 0;
        public string SMTP { get; set; } = "";
        public Freq Freq { get; set; } = 0;
        public string Sender { get; set; } = "";
        public string Password { get; set; } = "";
        public bool SSL { get; set; } = false;
        public List<string> SendTo { get; set; } = new List<string>();

    }
}
