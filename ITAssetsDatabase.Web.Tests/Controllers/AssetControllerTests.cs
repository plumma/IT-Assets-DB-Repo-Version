using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITAssetsDatabase.Web.Controllers;
using ITAssetsDatabase.DataAccess.Repositorys.Assets;
using Moq;
using ITAssetsDatabase.BusinessDomain;
using System.Collections.Generic;
using System.Web.Mvc;
using ITAssetsDatabase.Web.ViewModels.Assets;

namespace ITAssetsDatabase.Web.Tests.Controllers
{
    [TestClass]
    public class AssetControllerTests
    {
        private AssetsController _assetsController;
        private string _domain = "MyTestDomain";
        private string _type = "MyTestType";
        private string _locationCode = "MyTestLocationCode";
        private string _town = "MyTesttown";
        private string _number = "1";





        [TestInitialize]
        public void InitializeTest()
        {
            var assetRepository = new Mock<IAssetRepository>();
            var domainRepository = new Mock<IDomainRepository>();
            var assetActionsRepository = new Mock<IAssetActionsRepository>();
            var employeeRepository = new Mock<IEmployeeRepository>();
            var assetAssigneeRepository = new Mock<IAssetAssigneeRepository>();
            var assetAssignedRepository = new Mock<IAssetAssignedRepository>();
            var assetSignOffRepository = new Mock<IAssetSignOffRepository>();
            var locationRepository = new Mock<ILocationRepository>();
            var deviceRepository = new Mock<IDeviceRepository>();
            var buildsRepository = new Mock<IBuildsRepository>();
            var hostnameRepository = new Mock<IHostnameRepository>();

            buildsRepository.Setup(r => r.GetBuildByID(It.IsAny<int>())).Returns(new BusinessDomain.Build { Domain = _domain });
            deviceRepository.Setup(r => r.GetDeviceById(It.IsAny<int>())).Returns(new BusinessDomain.Device { Type = _type });
            locationRepository.Setup(r => r.GetLocationById(It.IsAny<int>())).Returns(new Location { LocationCode = _locationCode, Town = _town });
            hostnameRepository.Setup(r => r.GetHostnames()).Returns(new List<Hostname> { new Hostname { LocationCode = _locationCode, Number = _number } });


            //// Setup for the Index controller method
            
            var assetassignedlist = new List<AssetAssigned>();

            assetassignedlist.Add(new AssetAssigned { AssetId = 1, CreateDate = Convert.ToDateTime("01/01/2017 10:10:10"),
                                                      AssetStatusID = 1, AssetStatus = new AssetStatus { State = "NEW" },
                                                      AssetAssignee = new AssetAssignee { Assignee = new Employee { FirstName = "TestFirstName" },
                                                      HelpdeskRef = "TestHelpdeskRef"}  });
            assetassignedlist.Add(new AssetAssigned { AssetId = 2, CreateDate = Convert.ToDateTime("01/01/2017 10:10:10"),
                                                      AssetStatusID = 2, AssetStatus = new AssetStatus { State = "ASSIGNED" },
                                                      AssetAssignee = new AssetAssignee { Assignee = new Employee { FirstName = "TestFirstName" },
                                                      HelpdeskRef = "TestHelpdeskRef" }  });        
            assetassignedlist.Add(new AssetAssigned { AssetId = 3, CreateDate = Convert.ToDateTime("01/01/2017 10:10:10"),
                                                    AssetStatusID = 3, AssetStatus = new AssetStatus { State = "NOT ASSIGNED" }});
            assetassignedlist.Add(new AssetAssigned { AssetId = 4, CreateDate = Convert.ToDateTime("01/01/2017 10:10:10"),
                                                    AssetStatusID = 4, AssetStatus = new AssetStatus { State = "IN REPAIR" }});
            assetassignedlist.Add(new AssetAssigned { AssetId = 5, CreateDate = Convert.ToDateTime("01/01/2017 10:10:10"),
                                                    AssetStatusID = 5, AssetStatus = new AssetStatus { State = "RETIRED" }});

            var assetlist = new List<Asset>();
            assetlist.Add(new Asset

            {   Id = 1,
                CreateDate = new DateTime(2015,1,1,10,10,10),
                Device =  new Device { Type = "TestDevice", Make = "TestMake", Model = "TestModel"  },
                AssetAssigned = assetassignedlist, });

            assetlist.Add(new Asset
            {
                Id = 2,
                CreateDate = new DateTime(2015, 1, 1, 10, 10, 10),
                Device = new Device { Type = "TestDevice", Make = "TestMake", Model = "TestModel" },
                AssetAssigned = assetassignedlist,
            });

            assetlist.Add(new Asset
            {
                Id = 3,
                CreateDate = new DateTime(2015, 1, 1, 10, 10, 10),
                Device = new Device { Type = "TestDevice", Make = "TestMake", Model = "TestModel" },
                AssetAssigned = assetassignedlist,
            });

            assetlist.Add(new Asset
            {
                Id = 4,
                CreateDate = new DateTime(2015, 1, 1, 10, 10, 10),
                Device = new Device { Type = "TestDevice", Make = "TestMake", Model = "TestModel" },
                AssetAssigned = assetassignedlist,
            });

            assetlist.Add(new Asset
            {
                Id = 5,
                CreateDate = new DateTime(2015, 1, 1, 10, 10, 10),
                Device = new Device { Type = "TestDevice", Make = "TestMake", Model = "TestModel" },
                AssetAssigned = assetassignedlist,
            });

            
            assetRepository.Setup(r => r.GetAllAssets()).Returns(assetlist);


            _assetsController = new AssetsController(assetRepository.Object, domainRepository.Object, assetActionsRepository.Object, employeeRepository.Object, assetAssigneeRepository.Object, assetAssignedRepository.Object, assetSignOffRepository.Object, locationRepository.Object, deviceRepository.Object, buildsRepository.Object, hostnameRepository.Object);
        }


        //[TestMethod]
        //public void Should_Index()
        //{
        //    //Arrange


        //    //Act
        //    var result = _assetsController.Index();

        //    //Assert            

        //    Assert.IsInstanceOfType(result, typeof(ViewResult));
        //}


        [TestMethod]
        public void Should_FetchHostname()
        {
            //AAA convention
            //Arrange
            int buildId = 0;
            int localId = 0;
            int DeviceId = 0;
            int staffId = 0;
            //Act
            var result = _assetsController.FetchHostname(localId, DeviceId, staffId, buildId);
            //Assert
            Assert.IsTrue(result.Data is Hostname);
            Assert.IsTrue((result.Data as Hostname).FullHostname == "MYTESTTYPEMYTESTLOCATIONCODE0002");
        }
    }
}
