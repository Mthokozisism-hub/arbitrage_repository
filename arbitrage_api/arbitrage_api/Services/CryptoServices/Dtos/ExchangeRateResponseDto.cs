namespace arbitrage_api.Services.CryptoServices.Dtos
{
    public class ExchangeRateResponseDto
    {
        public string Result { get; set; }               
        public string Documentation { get; set; }       
        public string Terms_Of_Use { get; set; }          
        public long Time_Last_Update_Unix { get; set; }    
        public string Time_Last_Update_Utc { get; set; }   
        public long Time_Next_Update_Unix { get; set; }   
        public string Time_Next_Update_Utc { get; set; }   
        public string Base_Code { get; set; }           
        public string Target_Code { get; set; }          
        public decimal Conversion_Rate { get; set; }
    }
}
