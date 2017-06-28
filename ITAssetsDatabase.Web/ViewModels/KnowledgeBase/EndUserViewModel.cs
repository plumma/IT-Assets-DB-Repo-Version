namespace ITAssetsDatabase.Web.ViewModels.Assets
{
    public class EndUserViewModel
    {
        
        public string label { get; set; }  // Display name plus logon
        public string value { get; set; }  // SID


        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public string Logon { get; set; } 
    } 
} 
