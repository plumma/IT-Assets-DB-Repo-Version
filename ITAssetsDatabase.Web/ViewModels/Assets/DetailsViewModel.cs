
using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;

namespace ITAssetsDatabase.Web.ViewModels.Assets
{
    public class DetailsViewModel
    {
        public Asset Asset { get; set; }       
        public ICollection<AuditClass> Audit { get; set; }
    } 
}