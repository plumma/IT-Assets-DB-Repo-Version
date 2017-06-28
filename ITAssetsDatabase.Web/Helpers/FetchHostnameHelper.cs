using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using ITAssetsDatabase.DataAccess.Repositorys.Assets;



namespace ITAssetsDatabase.Web.Helpers
{
    public class FetchHostnameHelper
    {

        public static Hostname Fetch(int LocationId, string Type, int Staff, string Domain)
        {
            LocationRepository _Location = new LocationRepository();
            HostnameRepository _Hostname = new HostnameRepository();

            if (Domain == "PD")
            {

                var LocationCode = _Location.GetLocationById(LocationId).LocationCode;                    
                var Location = _Location.GetLocationById(LocationId).Town;

                if (Type == "Laptop")   // Laptop
                {
                    LocationCode = "PDLPRDM";
                    Location = "Mobile";
                }

                // Filters out all hostnames with relevant location codes then orders them by their number

                var filter = _Hostname.GetHostnames().Where(x => x.LocationCode == LocationCode).OrderBy(n => n.Number).Select(n => n.Number).ToList();

                // Looks at last number i.e. filter[19] and converts it to an integer

                var lastnumber = (Convert.ToInt32(filter[filter.Count - 1]));

                var hostname = "";

                bool pingresult = true;
                bool ADtest = true;

                string lastnumberstring;

                var hostname_check = new List<Hostname_temp>();


            startover:

                do
                {
                    lastnumber++;

                    lastnumberstring = lastnumber.ToString().PadLeft(4, '0');

                    hostname = LocationCode + lastnumberstring;

                    pingresult = pingtest.IsPingable(hostname);

                    ADtest = AD.checkinAD(hostname);



                } while (pingresult == true || ADtest == true);

                hostname_check = _Hostname.GetHostnames_temp().Where(h => h.Hostname == hostname).ToList();

                if (hostname_check.Count > 0)
                {
                    goto startover;
                }


                // Adds hostname to temporary table which gets deleted daily to avoid duplicate entries when two people requesting hostname

                var tempmodel = new Hostname_temp();
                tempmodel.Hostname = hostname;
                _Hostname.AddHostname_temp(tempmodel);
                

                var model = new Hostname();

                int TypeId;
               
                if (Type == "Laptop")
                    TypeId = 1;
                else
                    TypeId = 2;

                model.Location = Location;
                model.ProductTypeId = TypeId;
                model.StaffId = Staff;
                model.LocationCode = LocationCode;
                model.FullHostname = hostname;
                model.Number = lastnumberstring;
                
                _Hostname.AddHostname(model);
                
                var hostnamemodel = new Hostname();

                hostnamemodel.Id = model.Id;
                hostnamemodel.FullHostname = model.FullHostname;

                return hostnamemodel;

            }




            ////////////////////////////////////////////////////////  XAFINITY //////////////////////////////////////////////////////////////////////////////////////




            else if (Domain == "Regent")
            {
                // Xafinity Hostname creation

                var Location = _Location.GetLocationById(LocationId).Town;
                List<string> filter;
                int lastnumber = 0;

                var hostname = "";
                bool pingresult = true;
                bool ADtest = true;

                string lastnumberstring;

                var hostname_check = new List<Hostname_temp>();

                int ProductTypeId;

                if (Type == "Laptop")
                { ProductTypeId = 1; }
                else { ProductTypeId = 2; }

                filter = _Hostname.GetHostnames().Where(x => x.ProductTypeId == ProductTypeId).OrderBy(n => n.Number).Select(n => n.Number).ToList();
                lastnumber = (Convert.ToInt32(filter[filter.Count - 1]));

            startoverxafinity:

                do
                {
                    lastnumber++;
                    lastnumberstring = lastnumber.ToString().PadLeft(6, '0');

                    if (Type == "Laptop")
                    {
                        hostname = "IT" + lastnumberstring + "L";
                    }
                    else if (Type == "Desktop")
                    {
                        hostname = "IT" + lastnumberstring;
                    }

                    pingresult = pingtest.IsPingable(hostname);
                    ADtest = AD.checkinAD(hostname);
                } while (pingresult == true || ADtest == true);


                hostname_check = _Hostname.GetHostnames_temp().Where(h => h.Hostname == hostname).ToList();

                if (hostname_check.Count > 0)
                {
                    goto startoverxafinity;
                }

                var tempmodel = new Hostname_temp();
                tempmodel.Hostname = hostname;
                _Hostname.AddHostname_temp(tempmodel);
                
                var model = new Hostname();


                model.Location = Location;
                model.ProductTypeId = ProductTypeId;
                model.StaffId = Staff;
                model.FullHostname = hostname;
                model.Number = lastnumberstring;


                _Hostname.AddHostname(model);
                

                var hostnamemodel = new Hostname();

                hostnamemodel.Id = model.Id;
                hostnamemodel.FullHostname = model.FullHostname;

                return hostnamemodel;
            }

            return null;


        }
    }
}
