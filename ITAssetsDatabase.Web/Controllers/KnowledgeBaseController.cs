﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITAssetsDatabase.DataAccess;

using System.IO;   // Allows Path.Combine(path, fileName); to work
using ITAssetsDatabase.Web.ViewModels.KnowledgeBase;
using ITAssetsDatabase.BusinessDomain;
using ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase;



namespace ITAssetsDatabase.Web.Controllers
{
    public class KnowledgeBaseController : Controller

    {

        private IApplicationRepository _applicationRepository;
        private IArticleCategoryRepository _articleCategoryRepository;
        private IArticleRepository _articleRepository;
        private IArticleTypeRepository _articleTypeRepository;
        private IUploadedFileKBRepository _uploadedFileKBRepository;


        public KnowledgeBaseController(IApplicationRepository applicationRepository,
                                       IArticleCategoryRepository articleCategoryRepository,
                                       IArticleRepository articleRepository,
                                       IArticleTypeRepository articleTypeRepository,
                                       IUploadedFileKBRepository uploadedFileKBRepository)

             {
                 _applicationRepository = applicationRepository;
                 _articleCategoryRepository = articleCategoryRepository;
                 _articleRepository = articleRepository;
                 _articleTypeRepository = articleTypeRepository;
                 _uploadedFileKBRepository = uploadedFileKBRepository;   }


        //string CurrentlyLoggedin = System.Web.HttpContext.Current.User.Identity.Name.ToLower();


        public ActionResult Index()
        {
            var model = new KBViewModel();

            model.ArticleTypeId = new SelectList(_articleTypeRepository.GetArticleTypes(), "Id", "Name", 0);

            return View(model);
        }



        public PartialViewResult SearchKB(string SearchString, int ArticleTypeId)
        {

            int PagesizeCount = 9;
            ViewBag.PagesizeCount = PagesizeCount;
            

            if (ArticleTypeId != 0)
            {
                if (ArticleTypeId == 5)
                {
                    int articleid = Convert.ToInt32(SearchString.Trim());

                    var model = _articleRepository.GetArticleById_select(articleid);
                    
                    return this.PartialView(model);
                }

                else
                {
                    var q = _articleRepository.SearchKnowledgeBase(SearchString);
                    var p = q.Where(e => e.ArticleTypeId == ArticleTypeId);

                    int TotalResultsCount = p.Count();

                  
                    ViewBag.TotalResultsCount = TotalResultsCount;
                    
                    // Corrects Display of Number of results if less then PageSizeCount

                    if (TotalResultsCount < PagesizeCount)
                    {
                        ViewBag.DisplayedResultsCount = TotalResultsCount;
                        ViewBag.RemainingResultsCount = 0;
                    }
                    else {

                        ViewBag.DisplayedResultsCount = PagesizeCount;
                        ViewBag.RemainingResultsCount = TotalResultsCount - PagesizeCount;                    
                    }
                                        
                    var model = p.Take(PagesizeCount).ToList();                  
                    
                    return this.PartialView(model);
                }
            }

            else
            {
                var q = _articleRepository.SearchKnowledgeBase(SearchString);

                int TotalResultsCount = q.Count();

                ViewBag.TotalResultsCount = TotalResultsCount;
                
                // Corrects Display of Number of results if less then PageSizeCount

                if (TotalResultsCount < PagesizeCount)
                {
                    ViewBag.DisplayedResultsCount = TotalResultsCount;
                    ViewBag.RemainingResultsCount = 0;
                }
                else
                {

                    ViewBag.DisplayedResultsCount = PagesizeCount;
                    ViewBag.RemainingResultsCount = TotalResultsCount - PagesizeCount;
                }
                                
                
                var model = q.Take(PagesizeCount).ToList();                               
                
                return this.PartialView(model);
            }



        }

     
        public JsonResult LoadMoreResults(int ArticleTypeId, string SearchString, int PagesizeCount, int DisplayedResultsCount, int RemainingResultsCount)
         {

             int newDisplayedResultsCount;
             int newRemainingResultsCount;


             if ((DisplayedResultsCount + PagesizeCount) > (DisplayedResultsCount + RemainingResultsCount))
             {
                 newDisplayedResultsCount = DisplayedResultsCount + RemainingResultsCount;
                 newRemainingResultsCount = 0;
             }
             else
             {
                 newDisplayedResultsCount = DisplayedResultsCount + PagesizeCount;
                 newRemainingResultsCount = RemainingResultsCount - PagesizeCount;
             }

            
             
            var q = _articleRepository.SearchKnowledgeBase(SearchString);


            if (ArticleTypeId != 0)
            {

                var p = q.Where(e => e.ArticleTypeId == ArticleTypeId);
                var model = p.Skip(DisplayedResultsCount).Take(PagesizeCount).ToList();

                var collection = model.Select(x => new
                {
                    Id = x.id,
                    ArticleTypeImage = x.ArticleType.ArticleTypeImage,   // JSON seralisation can't handle this
                    Summary = x.Summary,
                    Details = x.Details,
                    AppName = x.Application.AppName,  // JSON seralisation can't handle this
                    ArticleCategory = x.ArticleCategory.Category,  // JSON seralisation can't handle this
                    ArticleType = x.ArticleType.Name,  // JSON seralisation can't handle this
                    Staff = x.Staff.SecondName,  // JSON seralisation can't handle this
                    CreatedDate = x.CreatedDate.ToString("dd/MM/yy"),
                    RemainingResultsCount = newRemainingResultsCount,
                    DisplayedResultsCount = newDisplayedResultsCount 
                });
                
                return Json(collection, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var model = q.Skip(DisplayedResultsCount).Take(PagesizeCount).ToList();

                var collection = model.Select(x => new
                {
                    Id = x.id,
                    ArticleTypeImage = x.ArticleType.ArticleTypeImage,   // JSON seralisation can't handle this
                    Summary = x.Summary,
                    Details = x.Details,
                    AppName = x.Application.AppName,  // JSON seralisation can't handle this
                    ArticleCategory = x.ArticleCategory.Category,  // JSON seralisation can't handle this
                    ArticleType = x.ArticleType.Name,  // JSON seralisation can't handle this
                    Staff = x.Staff.SecondName,  // JSON seralisation can't handle this                    
                    CreatedDate = x.CreatedDate.ToString("dd/MM/yy"),
                    RemainingResultsCount = newRemainingResultsCount,
                    DisplayedResultsCount = newDisplayedResultsCount 
                });
                
                return Json(collection, JsonRequestBehavior.AllowGet);
            }
            

            //  See Above 
            // Returning a multileveled object back to JSON, EF mapping doesn't automatically work so to prevent this we have to 
            // explicitly specify the field names, using mapping.
            // Server Error in '/' Application. A circular reference was detected while serializing an object of type
                                        
        }
        
        
        public PartialViewResult retrieveselectedID(int Id)
        {

                var model = _articleRepository.GetArticleById(Id);                
                return this.PartialView(model);
           
        }


        public JsonResult Autocomplete(string term, int? ArticleTypeId)
        {

            if (ArticleTypeId != null)
            {

                if (ArticleTypeId == 5)
                {
                                          
                    var result = new string(term.Where(c => char.IsDigit(c)).ToArray());

                    if (result != "")
                    {

                        int id = Convert.ToInt32(result.Trim());

                        var query = _articleRepository.GetArticleById_select(id).Select(x => new { value = x.id, label = x.Summary });
                        

                        return Json(query, JsonRequestBehavior.AllowGet);
                    }
                    else 
                    
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    
                    }


                } 

                else {
                    var query = _articleRepository.SearchSummaryWithFilter(term, ArticleTypeId).Select(x => new { value = x.id, label = x.Summary });

                    return Json(query, JsonRequestBehavior.AllowGet);
                    
                }

            }

            else {

                var query = _articleRepository.SearchSummary(term).Select(x => new { value = x.id, label = x.Summary });
                return Json(query, JsonRequestBehavior.AllowGet); 
            }

       }


        public JsonResult AutocompleteApplication(string term)
        {
            var query = _applicationRepository.SearchApplications(term).Select(o => new { id = o.Id, value = o.AppName });

            return Json(query, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Details(int id)
        {
            var model = _articleRepository.GetArticleById(id);

            return View(model);
        }


        public ActionResult AddArticle()
        {
            var model = new AddArticleViewModel();

            model.ApplicationList = new SelectList(_applicationRepository.GetApplications(), "Id", "AppName");
            model.ArticleCategoryList = new SelectList(_articleCategoryRepository.GetArticleCategorys(), "Id", "Category");
            model.ArticleTypeList = new SelectList(_articleTypeRepository.GetArticleTypes(), "Id ", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddArticle(AddArticleViewModel model, List<HttpPostedFileBase> fileupload_object)
        {
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

                        serverFilepath = AppDomain.CurrentDomain.BaseDirectory + "File Uploads"; //here give the directory where you want to save your file

                        if (!System.IO.Directory.Exists(serverFilepath))  //if path do not exit
                        {
                            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "File Uploads");//if given directory dont exist, it creates with give directory name
                        }

                        //string filePath = "/File Uploads";

                        
                         //In IE this gives full path and file not just file fix shown below

                        //string serverFilePath = Server.MapPath(filePath);
                        
                        fileName = Path.GetFileName(file.FileName);   // IE Fix to ensure path is stripped from uploaded file
                        fullPath = Path.Combine(serverFilepath, fileName);

                        if (!System.IO.File.Exists(fullPath))
                        {

                            if (fileName != null && fileName.Trim().Length > 0)
                            {
                                file.SaveAs(fullPath);
                            }
                        }
                    }
                }
            }


            // Add Article info to DB

            var thisArticle = new Article();

            thisArticle.Summary = model.Summary;
            thisArticle.Details = model.Details;
            thisArticle.Resolution = model.Resolution;
            thisArticle.Version = model.Version;
            thisArticle.ApplicationId = model.ApplicationId;
            thisArticle.ArticleTypeId = model.ArticleTypeId;
            thisArticle.ArticleCategoryId = model.ArticleCategoryId;

            //var staff = db.Staff.ToList();

            //var staff1 = staff.Where(s => s.LoggedInName == CurrentlyLoggedin).FirstOrDefault();

            //thisArticle.StaffId = staff1.Id;

            thisArticle.StaffId = 1;
            _articleRepository.AddArticle(thisArticle);
          
            var uploadedKB = new UploadedFileKB();

            foreach (HttpPostedFileBase file in fileupload_object)
            {

                if (file != null)
                {
                    uploadedKB.FileName = Path.GetFileName(file.FileName);
                    uploadedKB.ArticleId = thisArticle.id;

                    _uploadedFileKBRepository.UploadFile(uploadedKB);
                    
                }
            }
            
            return RedirectToAction("Index");
        }


        public FilePathResult GetFileFromDisk(int Id)
        {
            string filename = _uploadedFileKBRepository.GetUploadedFileNameByID(Id);

            string path = AppDomain.CurrentDomain.BaseDirectory + "File Uploads/";

            return File(path + filename, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", filename);
        }


        public PartialViewResult AddApp()
        {

            return this.PartialView();

        }


        [HttpPost]
        public JsonResult AddApp(Application NewApp)
        {

            var model = new Application();

           
                model.AppName = NewApp.AppName;
                model.Company = NewApp.Company;
            _applicationRepository.AddApp(model);
                

            return Json(model, JsonRequestBehavior.AllowGet);
        }




        /* -------------------------------------------------------------------------------------  */


        public ActionResult TestUpload()
        {
            return View();
        }


          [HttpPost]
        public ActionResult TestUpload(HttpPostedFileBase fileupload_object)
        {

                        string sourcedirectory = string.Empty;
                        string serverFilepath = string.Empty;
                        string fileName = string.Empty;
                        string fullPath = string.Empty;

                        serverFilepath = AppDomain.CurrentDomain.BaseDirectory + "File Uploads"; //here give the directory where you want to save your file

                        if (!System.IO.Directory.Exists(serverFilepath))  //if path do not exit
                        {
                            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "File Uploads");//if given directory dont exist, it creates with give directory name
                        }

                        //string filePath = "/File Uploads";

                        //string serverFilePath = Server.MapPath(filePath);
                        //string serverFilePath = "C:/inetpub/wwwroot/File Uploads";

                        
                        //fileName = fileupload_object.FileName;   In IE this gives full path and file not just file fix shown below

                        fileName = Path.GetFileName(fileupload_object.FileName);
                        fullPath = Path.Combine(serverFilepath, fileName);                

                        var model = new TestUploadViewModel();

                        model.filename = fileName;
                        model.fullpath = fullPath;
                        model.serverFilepath = serverFilepath;


               return View(model);
            }

        public ActionResult ReadMe() {

            return View();

        } 
        
    
    
    }

}