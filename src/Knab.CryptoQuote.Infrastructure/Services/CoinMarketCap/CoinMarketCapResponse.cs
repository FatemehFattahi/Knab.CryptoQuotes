namespace Knab.CryptoQuote.Infrastructure.Services.CoinMarketCap;

#nullable disable

internal sealed class CoinMarketCapResponse
{
    public class Status
    {
        public string timestamp { get; set; }
        
        public int error_code { get; set; }
        
        public string error_message { get; set; }
        
        public int elapsed { get; set; }
        
        public int credit_count { get; set; }
        
        public string notice { get; set; }
    }
    
    public class CryptoCurrencyRate
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int num_market_pairs { get; set; }
        public string date_added { get; set; }
        public List<TagsItem> tags { get; set; }
        public int max_supply { get; set; }
        public int circulating_supply { get; set; }
        public int total_supply { get; set; }
        public int is_active { get; set; }
        public string platform { get; set; }
        public int cmc_rank { get; set; }
        public int is_fiat { get; set; }
        public string self_reported_circulating_supply { get; set; }
        public string self_reported_market_cap { get; set; }
        public string tvl_ratio { get; set; }
        public string last_updated { get; set; }
        public Quote quote { get; set; }
        
        public class TagsItem
        {
            public string slug { get; set; }
            public string name { get; set; }
            public string category { get; set; }
        }
        
        public class Quote
        {
            public Currency CurrencyVolume { get; set; }
        
            public class Currency
            {
                public double price { get; set; }
                public double volume_24h { get; set; }
                public double volume_change_24h { get; set; }
                public double percent_change_1h { get; set; }
                public double percent_change_24h { get; set; }
                public double percent_change_7d { get; set; }
                public double percent_change_30d { get; set; }
                public double percent_change_60d { get; set; }
                public double percent_change_90d { get; set; }
                public double market_cap { get; set; }
                public double market_cap_dominance { get; set; }
                public double fully_diluted_market_cap { get; set; }
                public string tvl { get; set; }
                public string last_updated { get; set; }
            }
        }
    }
}