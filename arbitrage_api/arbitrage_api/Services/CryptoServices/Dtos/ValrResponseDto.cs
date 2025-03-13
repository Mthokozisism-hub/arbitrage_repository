namespace arbitrage_api.Services.CryptoServices.Dtos
{
    public class ValrResponseDto
    {
        public string Timestamp { get; set; }
        public decimal Open { get; set; }    
        public decimal High { get; set; }   
        public decimal Low { get; set; }    
        public decimal Last { get; set; }    
        public decimal Volume { get; set; }  
        public decimal Vwap { get; set; }   
        public decimal Bid { get; set; }     
        public decimal Ask { get; set; }     
        public int Side { get; set; }        
        public decimal Open24 { get; set; }  
        public decimal PercentChange24 { get; set; } 
    }
}
