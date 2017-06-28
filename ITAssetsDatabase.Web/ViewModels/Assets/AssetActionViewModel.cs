using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITAssetsDatabase.BusinessDomain;

namespace ITAssetsDatabase.Web.ViewModels.Assets
{
    public class AssetActionViewModel
    {
        public Asset Assets { get; set; }
        public SelectList AssetActions { get; set; }

        public string State { get; set; }
        public string Hostname { get; set; }
        
    }
}