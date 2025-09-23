﻿namespace star_events.Models.Configuration
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; } =  string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string WebhookSecret {  get; set; } = string.Empty;

        public string Currency { get; set; } = "lkr";

    }
}
