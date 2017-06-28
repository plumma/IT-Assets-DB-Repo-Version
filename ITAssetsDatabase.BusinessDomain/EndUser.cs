using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetsDatabase.BusinessDomain
{

    public class EndUser
    {
    
        public int Id { get; set; }
        public string SID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Extension { get; set; }
        public string Domain { get; set; }
        public string DomainLogon { get; set; }
        public int? BusinessAreaId { get; set; }
    
       
        public virtual BusinessArea BusinessArea { get; set; }

        [NotMapped]
        public string FullName
        {

            get { return SecondName + ", " + FirstName; }

        }      

        [NotMapped]
        public int? itemorderId { get; set; }
         

    }
}
