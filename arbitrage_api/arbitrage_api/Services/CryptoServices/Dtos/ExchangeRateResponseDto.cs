namespace arbitrage_api.Services.CryptoServices.Dtos
{
    public class ExchangeRateResponseDto
    {
        public string Result { get; set; }               
        public string Documentation { get; set; }       
        public string TermsOfUse { get; set; }          
        public long TimeLastUpdateUnix { get; set; }    
        public string TimeLastUpdateUtc { get; set; }   
        public long TimeNextUpdateUnix { get; set; }   
        public string TimeNextUpdateUtc { get; set; }   
        public string BaseCode { get; set; }           
        public string TargetCode { get; set; }          
        public decimal ConversionRate { get; set; }
    }
}
