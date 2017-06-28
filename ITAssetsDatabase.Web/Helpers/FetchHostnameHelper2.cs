using System;
using System.Data;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using ITAssetsDatabase.DataAccess;
using ITAssetsDatabase.DataAccess.Repositorys.Assets;

namespace ITAssetsDatabase.Helpers
{
    public class FetchHostnameHelper2
    {
        private IHostnameRepository _hostnameRepository;
        private ILocationRepository _locationRepository;

        public FetchHostnameHelper2(IHostnameRepository hostnameRepository, ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
            _hostnameRepository = hostnameRepository;
        }

        public Hostname Fetch(int LocationId, string Type, int Staff, string Domain)
        {
            var LocationCode = _locationRepository.GetLocationById(LocationId).LocationCode;
            var Location = _locationRepository.GetLocationById(LocationId).Town;

            // Filters out all hostnames with relevant location codes then orders them by their number
            var filter = _hostnameRepository.GetHostnames().Where(x => x.LocationCode == LocationCode).OrderBy(n => n.Number).Select(n => n.Number).ToList();

            // Looks at last number i.e. filter[19] and converts it to an integer

            var lastnumber = (Convert.ToInt32(filter[filter.Count - 1]));

            var hostname = "";

            string lastnumberstring;

            lastnumber++;

            lastnumberstring = lastnumber.ToString().PadLeft(4, '0');

            hostname = (Type + LocationCode + lastnumberstring).ToUpper();
            
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


            _hostnameRepository.AddHostname(model);
            
            var hostnamemodel = new Hostname();

            hostnamemodel.Id = model.Id;
            hostnamemodel.FullHostname = model.FullHostname;

            return hostnamemodel;

        }
    }
}
       