namespace arbitrage_api.Services.CryptoServices.Dtos
{
    public class ValrResponseDto
    {
        public string CurrencyPair { get; set; }       
        public string AskPrice { get; set; } 
        public string BidPrice { get; set; }         
        public string LastTradedPrice { get; set; }  
        public string PreviousClosePrice { get; set; } 
        public string BaseVolume { get; set; }   
        public string QuoteVolume { get; set; } 
        public string HighPrice { get; set; }  
        public string LowPrice { get; set; }       
        public DateTime Created { get; set; }        
        public string ChangeFromPrevious { get; set; } 
        public string MarkPrice { get; set; }
    }
}
