﻿namespace TwentyFirst.Services.AuthMessageSender
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }

        public string SendGridDefaultEmailSender { get; set; }
    }
}
