namespace TechnoSpaceAPIs.Models
{
    public class PayData
    {
        public long PAYGATE_ID { get; set; }
        public string REFERENCE { get; set; }
        public int AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string RETURN_URL { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string LOCALE { get; set; }
        public string COUNTRY { get; set; }
        public string EMAIL { get; set; }
        public string CHECKSUM { get; set; }

        public System.Collections.Generic.Dictionary<string, string> ToDictionary()
        {
            return new System.Collections.Generic.Dictionary<string, string>
            {
                { nameof(PAYGATE_ID), PAYGATE_ID.ToString() },
                { nameof(REFERENCE), REFERENCE },
                { nameof(AMOUNT), AMOUNT.ToString() },
                { nameof(CURRENCY), CURRENCY },
                { nameof(RETURN_URL), RETURN_URL },
                { nameof(TRANSACTION_DATE), TRANSACTION_DATE },
                { nameof(LOCALE), LOCALE },
                { nameof(COUNTRY), COUNTRY },
                { nameof(EMAIL), EMAIL },
                { nameof(CHECKSUM), CHECKSUM }
            };
        }
    }
}
