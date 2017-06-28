using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITAssetsDatabase.DataAccess.Repositorys.Assets;
using System.IO;   // Allows Path.Combine(path, fileName); to work
using System.DirectoryServices;                     // Active directory chats 
using ITAssetsDatabase.Web.ViewModels.Assets;
using ITAssetsDatabase.BusinessDomain;
using ITAssetsDatabase.Helpers;
using ITAssetsDatabase.Web.Helpers;

namespace ITAssetsDatabase.Web.Controllers
{
    public class AssetsController : Controller
    {
        private IAssetRepository _assetRepository;
        private IDomainRepository _domainRepository;
        private IAssetActionsRepository _assetActionsRepository;
        private IEmployeeRepository _employeeRepository;
        private IAssetAssigneeRepository _assetAssigneeRepository;
        private IAssetAssignedRepository _assetAssignedRepository;
        private IAssetSignOffRepository _assetSignOffRepository;
        private ILocationRepository _locationRepository;
        private IDeviceRepository _deviceRepository;
        private IBuildsRepository _buildsRepository;
        private IHostnameRepository _hostnameRepository;




        public AssetsController(IAssetRepository assetRepository, 
                                IDomainRepository domainRepository,
                                IAssetActionsRepository assetActionsRepository,
                                IEmployeeRepository employeeRepository,
                                IAssetAssigneeRepository assetAssigneeRepository,
                                IAssetAssignedRepository assetAssignedRepository,
                                IAssetSignOffRepository assetSignOffRepository,
                                ILocationRepository locationRepository,
                                IDeviceRepository deviceRepository,
                                IBuildsRepository buildsRepository,
                                IHostnameRepository hostnameRepository)
        {
            _assetRepository = assetRepository;
            _domainRepository = domainRepository;
            _assetActionsRepository = assetActionsRepository;
            _employeeRepository = employeeRepository;
            _assetAssigneeRepository = assetAssigneeRepository;
            _assetAssignedRepository = assetAssignedRepository;
            _assetSignOffRepository = assetSignOffRepository;
            _locationRepository = locationRepository;
            _deviceRepository = deviceRepository;
            _buildsRepository = buildsRepository;
            _hostnameRepository = hostnameRepository;
        }



        //string CurrentlyLoggedin = System.Web.HttpContext.Current.User.Identity.Name.ToLower();

        public ActionResult Index()
        {
            var model = new List<AssetsViewModel>();

            var query = _assetRepository.GetAllAssets();

            string EndUserName = null;
            string HelpdeskRef = null;


            foreach (var Assets in query)
            {
                var assetquery = Assets.AssetAssigned.OrderByDescending(d => d.CreateDate).FirstOrDefault();
                //var Status = assetquery.AssetStatus.Id;
                var Status = assetquery.AssetStatusID;
                var StatusLabel = assetquery.AssetStatus.State;

                if (Status == 1 || Status == 2)   // NEW or ASSIGNED
                {
                    EndUserName = assetquery.AssetAssignee.Assignee.FirstName;
                    HelpdeskRef = assetquery.AssetAssignee.HelpdeskRef;
                }
                else if (Status == 3)  // IN STOCK
                {
                    EndUserName = "NOT ASSIGNED";
                    HelpdeskRef = "n/a";
                }

                else if (Status == 4)  // FAULTY
                {
                    EndUserName = "IN REPAIR";
                    HelpdeskRef = "n/a";
                }
                else if (Status == 5)  // RETIRED
                {
                    EndUserName = "n/a";
                    HelpdeskRef = "n/a";
                }

                var viewModel = new AssetsViewModel

                {
                    Id = Assets.Id,
                    CreateDate = Assets.CreateDate,
                    Type = Assets.Device.Type,
                    Make = Assets.Device.Make,
                    Model = Assets.Device.Model,
                    Status = StatusLabel,
                    EndUserName = EndUserName,
                    HelpdeskRef = HelpdeskRef
                };

                model.Add(viewModel);
                
            }

            return View(model);
        }

        public ActionResult Register()
        {

            var model = new RegisterAssetViewModel();

            // This snippet of code was originally used within a domain infrastructure environment and would look at who was logged on and update a new asset record using their Staff ID
            // for the purposes of placing the asset register in a public setting I will use one guest id to register asset additions
            //model.StaffId = db.Staff.Select(o => new { StaffFullName = o.Domain + @"\" + o.DomainLogon, o.Id }).Where(r => r.StaffFullName == CurrentlyLoggedin).Select(t => t.Id).First();

            // StaffID of 1 is set as a guest Id
            model.StaffId = 1;
            model.Domain = new SelectList(_domainRepository.GetDomains(), "Id", "DomainName");
            
            return View(model); 
        }      

        [HttpPost]
        public ActionResult Register(RegisterAssetViewModel Asset)
        {

            // Add End User to AssetAssignee entity

            var newAsset = new Asset();

            newAsset.PRRef = Asset.PRRef;
            newAsset.PORef = Asset.PORef;
            newAsset.AssetNo = Asset.AssetNo;
            newAsset.SerialNo = Asset.SerialNo;
            newAsset.MAC_Address = Asset.MAC_Address;
            newAsset.DomainNameId = Asset.DomainId;
            newAsset.HostnameId = Asset.HostnameId;
            newAsset.BuildId = Asset.BuildId;
            newAsset.DeviceId = Asset.DeviceId;
            newAsset.StaffId = Asset.StaffId;


            _assetRepository.AddNewAsset(newAsset);

            // Update AssetAssignee entity

            var newAssetAssignee = new AssetAssignee();

            newAssetAssignee.HelpdeskRef = Asset.HelpdeskRef;


            // Update Assignee Details

            // Check if user already in Database

            var EmployeeSIDLookup_Id = _employeeRepository.EmployeeExistsLookup(Asset.AssigneeSID);
                
                //db.Employees.Where(e => e.SID == Asset.AssigneeSID).Select(u => u.Id).FirstOrDefault();

            if (EmployeeSIDLookup_Id != 0)
            {
                newAssetAssignee.AssigneeId = EmployeeSIDLookup_Id;
            }
            else
            {
                // Update Employee entity with newly aquired SID details

                var employee = new Employee();

                employee.FirstName = Asset.AssigneeFirstName;
                employee.MiddleName = Asset.AssigneeMiddleName;
                employee.SecondName = Asset.AssigneeSurname;
                employee.Email = Asset.AssigneeEmail;
                employee.Domain = Asset.AssigneeDomain;
                employee.DomainLogon = Asset.AssigneeDomainLogon;
                employee.SID = Asset.AssigneeSID;

                // Adds the new employee and returns the Id of the newly added employee
                newAssetAssignee.AssigneeId = _employeeRepository.AddEmployee(employee);
                
                }
            // Update Requester Details


            var RequesterSIDLookup_Id = _employeeRepository.EmployeeExistsLookup(Asset.RequesterSID);
            //SIDLookup = db.Employees.Where(e => e.SID == Asset.RequesterSID).Select(u => u.Id).FirstOrDefault();

            if (RequesterSIDLookup_Id != 0)
            {
                newAssetAssignee.RequesterId = RequesterSIDLookup_Id;
            }
            else
            {
                // Update Employee entity with newly aquired SID details

                var employee = new Employee();

                employee.FirstName = Asset.RequesterFirstName;
                employee.MiddleName = Asset.RequesterMiddleName;
                employee.SecondName = Asset.RequesterSurname;
                employee.Email = Asset.RequesterEmail;
                employee.Domain = Asset.RequesterDomain;
                employee.DomainLogon = Asset.RequesterDomainLogon;
                employee.SID = Asset.RequesterSID;

                // Adds the new employee and returns the Id of the newly added employee
                newAssetAssignee.RequesterId = _employeeRepository.AddEmployee(employee);

            }

            // Add new record to the AssetAssignee entity

            _assetAssigneeRepository.AddNewAssetAssigneedRecord(newAssetAssignee);


            // Update AssetAssigned entity

            var newAssetAssigned = new AssetAssigned();

            newAssetAssigned.AssetStatusID = 1;   // To represent NEW status
            newAssetAssigned.AssetId = newAsset.Id;  // Adds newly created ID from newAsset record to AssetAssigned entity
            newAssetAssigned.LocationId = Asset.LocationId;
            newAssetAssigned.StaffId = Asset.StaffId;
            newAssetAssigned.Notes = Asset.Notes;
            newAssetAssigned.AssetAssigneeID = newAssetAssignee.Id;

            _assetAssignedRepository.AddAssetAssignedRecord(newAssetAssigned);


            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var model = _assetRepository.GetAssetById(id);
            
            ViewBag.Hostname = model.Hostname.FullHostname;        
            
            ViewBag.status = model.AssetAssigned.Where(x => x.AssetId == id).OrderByDescending(x => x.CreateDate).Select(x => x.AssetStatus.State).First();

            var newmodel = new DetailsViewModel();

            newmodel.Asset = model;

            var dbrecordset = _assetAssignedRepository.GetAssetAssignedRecordsByAssetId(id);
            
           var ListOfObjects = new List<AuditClass>();
            
            foreach (var Item in dbrecordset)
            {

                var eachAudit = new AuditClass();
                {
                    eachAudit.CreatedDate = Item.CreateDate;

                    if (Item.AssetAssigneeID != null)
                    {
                        eachAudit.HelpdeskRef = Item.AssetAssignee.HelpdeskRef;
                        eachAudit.RequesterFullName = Item.AssetAssignee.Requester.FirstName;
                        eachAudit.EndUserFullName = Item.AssetAssignee.Assignee.FirstName;
                        eachAudit.SignOffSheetFileName = Item.AssetAssignee.SignOffSheetFileName;
                    }
                    else
                    {
                        eachAudit.HelpdeskRef = "n/a";
                        eachAudit.RequesterFullName = "n/a";
                        eachAudit.EndUserFullName = "n/a";
                        eachAudit.SignOffSheetFileName = "";
                    }

                    
                    eachAudit.AssetStatus = Item.AssetStatus.State;
                    eachAudit.Notes = Item.Notes;
                    eachAudit.Staff = Item.Staff.SecondName;

                    ListOfObjects.Add(eachAudit);
       
                }
                
            }

            newmodel.Audit = ListOfObjects;


            return View(newmodel);

        }

        public JsonResult ConfirmDelete(int id)
        {
            _assetAssignedRepository.DeleteAssetAssignedRecordsById(id);

            // Remove main asset record.

            _assetRepository.DeleteAssetById(id);
            
            return null;

        }

        public ActionResult PrintSignoff_display(int AssetId)
        {

            var model = new PrintSignoff();

            // Find newest assigned end user

            //var newestassignedenduser = db.AssetAssigned.Where(x => x.AssetId == AssetId && x.AssetStatusID == 2)
            //                .OrderByDescending(s => s.CreateDate)
            //                .Select(y => new { Fullname = y.AssetAssignee.Assignee.FirstName + " " + y.AssetAssignee.Assignee.SecondName }).FirstOrDefault();


            var newestassignedenduser = _assetAssignedRepository.CurrentAssignee(AssetId);

            model.EndUser = newestassignedenduser.AssetAssignee.Assignee.FirstName;
            model.Email = newestassignedenduser.AssetAssignee.Assignee.Email;
            model.Number = newestassignedenduser.AssetAssignee.Assignee.PhoneNum;
            model.UserDomain = newestassignedenduser.AssetAssignee.Assignee.Domain;

            var query = _assetRepository.GetAssetById(AssetId);

                
            model.HelpdeskRef = newestassignedenduser.AssetAssignee.HelpdeskRef;
            model.PORef = query.PORef;
            model.PRRef = query.PRRef;

            model.Type = query.Device.Type;
            model.Make = query.Device.Make;
            model.Model = query.Device.Model;
            model.SerialNo = query.SerialNo;
            model.Asset = query.AssetNo;
            model.MACAddress = query.MAC_Address;
           
            model.Hostname = query.Hostname.FullHostname;
            model.Build = query.Build.BuildName;
            model.DeviceDomain = query.DomainName.DomainName;

            //var staffid = 1;

            //var staffid = db.Staff.Select(o => new { StaffFullName = o.Domain + @"\" + o.DomainLogon, o.Id }).Where(r => r.StaffFullName == CurrentlyLoggedin).Select(t => t.Id).First();

            //model.Engineer = db.Staff.Find(staffid).FullName;
            model.Engineer = "Matt Plumley";

            return View(model);
        
        }
   
        public FilePathResult GetFileFromDisk(int id)
        {

            string filename = _assetSignOffRepository.GetSignOffSheetFileName(id);

            string path = AppDomain.CurrentDomain.BaseDirectory + "SignOffSheets/";

            return File(path + filename, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", filename);
        }

        public ActionResult Edit(int id)
        {
            var query = _assetRepository.GetAssetById(id);

            string hostname = "";
            hostname = query.Hostname.FullHostname;

            int status = 0;

            if (query != null)
            {
                status = _assetAssignedRepository.GetAssetStatusId(id);
            }

            var model = new AssetActionViewModel();
            model.Assets = query;
            model.Hostname = hostname;



            if (status == 1)  // Status NEW

            {
                model.State = "NEW";

              //  Build one off Selectlist for a NEW asset only.

               model.AssetActions = new SelectList(_assetActionsRepository.GetAssetActions(model.State), "ID", "Action");
                
            }



            if (status == 2)   // Status ASSIGNED
            {
                model.State = "ASSIGNED";
                model.AssetActions = new SelectList(_assetActionsRepository.GetAssetActions(model.State), "ID", "Action", 1);
           

            }

            if (status == 3)  // Status IN STOCK
            {
                model.State = "IN STOCK";
            //    Remove the Return to Stock Option
                var ammendedlist = _assetActionsRepository.GetAssetActions(model.State).Where(x => x.ID != 1);
                model.AssetActions = new SelectList(ammendedlist, "ID", "Action", 2);

            }

            if (status == 4)  // Status FAULTY
            {

                model.State = "FAULTY";

                //  Remove the DEVICE IN REPAIR option from selectlist
                var ammendedlist = _assetActionsRepository.GetAssetActions(model.State).Where(x => x.ID != 4);

                model.AssetActions = new SelectList(ammendedlist, "ID", "Action", 2);

                
            }

            if (status == 5)  // Status RETIRED
            {

                model.State = "RETIRED";

           //     Remove the DEVICE IN REPAIR option
                var ammendedlist = _assetActionsRepository.GetAssetActions(model.State).Where(x => x.ID != 3);

                model.AssetActions = new SelectList(ammendedlist, "ID", "Action", 2);
            
            }

            return View(model);
        }



        [HttpPost]
        public ActionResult Edit(string submitbuttonflag, int id, int Action, List<HttpPostedFileBase> fileupload_object)
        {

            if (submitbuttonflag == "action")
            {

                // For the purposes of this public website I've set the Staff ID to 1 which represents myself only
                var StaffId = 1;
                // db.Staff.Select(o => new { StaffFullName = o.Domain + @"\" + o.DomainLogon, o.Id }).Where(r => r.StaffFullName == CurrentlyLoggedin).Select(t => t.Id).First();


                var model = new AssetAssigned();
                model.StaffId = StaffId;
                model.AssetId = id;
                model.AssetAssigneeID = null;


                if (Action == 5)  // ASSIGN

                {
                    model.AssetStatusID = 2;

                    //Retrieve the record of the Assignment when first registered and copy that to the ASSIGNED Record 
                    model.AssetAssigneeID = _assetAssignedRepository.GetAssetAssignedRecordsByAssetId(id).Select(x => x.AssetAssigneeID).FirstOrDefault();

                }

                if (Action == 1)  // Return to stock

                {
                    model.AssetStatusID = 3;
                }

                if (Action == 2)  // Re-deploy to different user
                {
                    return RedirectToAction("ReDeploy", new { id = id });

                }

                if (Action == 3)  // Retire the device
                {
                    model.AssetStatusID = 5;
                }

                if (Action == 4)  // Mark device as in repair
                {
                    model.AssetStatusID = 4;
                }

                _assetAssignedRepository.AddAssetAssignedRecord(model);

                return RedirectToAction("Index");
            }


            else if (submitbuttonflag == "upload")
            {



                // Determine Latest User.

                var newestassignedenduser = _assetAssignedRepository.CurrentAssignee(id);

                var EmployeeID = newestassignedenduser.AssetAssignee.Assignee.Id;
                var EmployeeFullName = newestassignedenduser.AssetAssignee.Assignee.FirstName;

                string newfilename = "";


                //   Save attached files to disk

                if (fileupload_object.Count > 0)
                {
                    foreach (HttpPostedFileBase file in fileupload_object)
                    {
                        string pathFile = string.Empty;
                        if (file != null)
                        {
                            string serverFilepath = string.Empty;
                            string fileName = string.Empty;
                            string fullPath = string.Empty;

                            serverFilepath = AppDomain.CurrentDomain.BaseDirectory + "SignOffSheets"; //here give the directory where you want to save your file

                            if (!System.IO.Directory.Exists(serverFilepath))  //if path do not exit
                            {
                                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "SignOffSheets");//if given directory dont exist, it creates with give directory name
                            }


                            string date = DateTime.Now.ToString("dd-MM-yy");

                            fileName = Path.GetFileName(file.FileName);   // IE Fix to ensure path is stripped from uploaded file

                            //string name = Path.GetFileNameWithoutExtension(fileName);

                            string[] name = EmployeeFullName.Split(' ');


                            string extension = Path.GetExtension(fileName);

                            newfilename = name[1].Trim() + "_" + name[0].Trim() + "_" + date + extension;

                            fullPath = Path.Combine(serverFilepath, newfilename);

                            if (!System.IO.File.Exists(fullPath))
                            {

                                if (fileName != null && fileName.Trim().Length > 0)
                                {
                                    file.SaveAs(fullPath);
                                }
                            }

                            var model = new AssetSignoff();

                            model.AssetId = id;
                            model.EmployeeId = EmployeeID;
                            model.Uploadpath = newfilename;
                            _assetSignOffRepository.AddSignOffsheet(model);
                        }
                    }
                }

                return RedirectToAction("Edit", new { id = id });


            }

            return null;
        }



        public ActionResult ReDeploy( int? id)

        {
            var model = _assetRepository.GetAssetById(id);

            var status = model.AssetAssigned.Where(x => x.AssetId == id).OrderByDescending(x => x.CreateDate).First();


            // var StaffId = db.Staff.Select(o => new { StaffFullName = o.Domain + @"\" + o.DomainLogon, o.Id }).Where(r => r.StaffFullName == CurrentlyLoggedin).Select(t => t.Id).First();
            var StaffId = 1;
            
            var ReDeployViewModel = new ReDeployViewModel();

            ReDeployViewModel.id = model.Id;

            ReDeployViewModel.StaffId = StaffId;

            ReDeployViewModel.AssetStatus = status.AssetStatus.State;

            ReDeployViewModel.DeviceDetails = model.Device.Type + "-> " + model.Device.Make + "-> " + model.Device.Model;

            ReDeployViewModel.DeviceId = model.DeviceId;

            ReDeployViewModel.ComputerDomain = model.DomainName.DomainName;

            ReDeployViewModel.Hostname = model.Hostname.FullHostname;  
          

            if (status.AssetAssigneeID != null)
            {

                ReDeployViewModel.AssignedTo = status.AssetAssignee.Requester.FullName;

            }

            else {

                ReDeployViewModel.AssignedTo = "Currently Not Assigned";
            }

            ReDeployViewModel.RedeployCheckBox = true;
            ReDeployViewModel.Domain = new SelectList(_domainRepository.GetDomains(), "Id", "DomainName");

            
            return View(ReDeployViewModel);

        }

        [HttpPost]
        public ActionResult ReDeploy(ReDeployViewModel model)
        {
            if (model.RedeployCheckBox == true)        // Redeploy asset to new user
            {
                
                // Update AssetAssignee entity

                var newAssetAssignee = new AssetAssignee();

                newAssetAssignee.HelpdeskRef = model.HelpdeskRef;


                // Update Assignee Details


                var SIDLookup = _employeeRepository.DoesEmployeeSIDExist(model.AssigneeSID);
                    
                if (SIDLookup != 0)
                {
                    newAssetAssignee.AssigneeId = SIDLookup;
                }
                else
                {

                    // Update Employee entity with newly aquired SID details

                    var employee = new Employee();

                    employee.FirstName = model.AssigneeFirstName;
                    employee.MiddleName = model.AssigneeMiddleName;
                    employee.SecondName = model.AssigneeSurname;
                    employee.Email = model.AssigneeEmail;
                    employee.Domain = model.AssigneeDomain;
                    employee.DomainLogon = model.AssigneeDomainLogon;
                    employee.SID = model.AssigneeSID;

                    newAssetAssignee.AssigneeId = _employeeRepository.AddEmployee(employee);
                    
                }
                // Update Requester Details

                SIDLookup = _employeeRepository.DoesEmployeeSIDExist(model.RequesterSID);
                    
                if (SIDLookup != 0)
                {
                    newAssetAssignee.RequesterId = SIDLookup;
                }
                else
                {
                    // Update Employee entity with newly aquired SID details

                    var employee = new Employee();

                    employee.FirstName = model.RequesterFirstName;
                    employee.MiddleName = model.RequesterMiddleName;
                    employee.SecondName = model.RequesterSurname;
                    employee.Email = model.RequesterEmail;
                    employee.Domain = model.RequesterDomain;
                    employee.DomainLogon = model.RequesterDomainLogon;
                    employee.SID = model.RequesterSID;


                    newAssetAssignee.RequesterId = _employeeRepository.AddEmployee(employee);
                    
                }

                // Add new AssetAssignee record then return Id of newly added record
                var newAssetAssigneeId = _assetAssigneeRepository.AddNewAssetAssigneedRecord(newAssetAssignee);
                                

                // Update AssetAssigned entity

                var newAssetAssigned = new AssetAssigned();

                newAssetAssigned.AssetStatusID = 2 ;   // Assigned to user
                newAssetAssigned.AssetId = model.id;  // Adds newly created ID from newAsset record to AssetAssigned entity
                newAssetAssigned.AssetAssigneeID = newAssetAssigneeId;
                newAssetAssigned.StaffId = model.StaffId;
                newAssetAssigned.Notes = model.Notes;

                _assetAssignedRepository.AddAssetAssignedRecord(newAssetAssigned);
                
            }


            if (model.ChangeBuildHostnameCheckBox == true)        // Redeploy asset to new user
            {

                // Update Asset with new hostname/build

                _assetRepository.UpdateAsset(model.id, model.HostnameId, 1, model.BuildId);
                                    
            }


            return RedirectToAction("Index");

        }

        public JsonResult LookupPersonAutocomplete(string term, int DomainId)

        {
            if (term != string.Empty)
            {
                
                var search = LDAPHelper.SearchUser(term);

                var listmodel = new List<EndUserViewModel>();

                foreach (SearchResult result in search)
                {
                    var model = new EndUserViewModel();

                    DirectoryEntry user = result.GetDirectoryEntry();
                    if (user != null && user.Name.Contains("uid"))
                    {

                        model.value = Convert.ToString(user.Properties["uid"].Value);
                        model.label = user.Properties["cn"].Value.ToString();
                        model.FirstName = user.Properties["cn"].Value.ToString();
                        model.Surname = user.Properties["sn"].Value.ToString();
                        model.Logon = user.Properties["uid"].Value.ToString();
                        model.Email = Convert.ToString(user.Properties["mail"].Value);
                        model.Domain = user.Path;

                        listmodel.Add(model);

                    }
                }


                return Json(listmodel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        //  LookupLocation
        public JsonResult LookupLocationAutocomplete(string term)
        {
            if (term != string.Empty)
            {

                var locations = _locationRepository.GetLocations(term).Select(o => new { value = o.Id, label = o.BuildingName + ", " + o.Town });
                                                
                return Json(locations, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        //  Lookup Device Model
        public JsonResult LookupDeviceModelAutocomplete(string term)
        {
            if (term != string.Empty)
            {
                var devices = _deviceRepository.GetDeviceList(term).Select(o => new { value = o.Id, label = o.Type + "->" + o.Make + "->" + o.Model });
                
                return Json(devices, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        //  Lookup Build
        public JsonResult LookupBuildAutocomplete(string term, int filter)
        {
            if (term != string.Empty)
            {

                var builds = _buildsRepository.GetBuildList(term).Select(o => new { value = o.Id, label = o.Domain + "->" + o.BuildName});
                 
                return Json(builds, JsonRequestBehavior.AllowGet);
                         
            } 
            else
            {
                return null;
            }

        }

        //  Fetch Hostname
        public JsonResult FetchHostname(int LocationId, int DeviceId, int StaffId, int BuildId)
        {

            var Domain = _buildsRepository.GetBuildByID(BuildId).Domain;
            var Type = _deviceRepository.GetDeviceById(DeviceId).Type;

            var helper = new FetchHostnameHelper2(_hostnameRepository, _locationRepository);
            var Hostname = helper.Fetch(LocationId, Type, StaffId, Domain);

            return Json(Hostname, JsonRequestBehavior.AllowGet);
        }
        
    }
}

    