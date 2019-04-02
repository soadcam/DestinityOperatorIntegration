using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using OperadorDestinoCarpetaMock.DAO;

namespace OperadorDestinoCarpetaMock.Controllers
{
    public class HomeController : Controller
    {
        private DapperDAO _dao;
        private readonly string _destinyFolder;

        public HomeController()
        {
            _dao = new DapperDAO();
            _destinyFolder = ConfigurationManager.AppSettings["DestinyFolder"];
        }

        public ActionResult Index() =>
            View();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult ValidateDocuments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateDocuments(string idNumberDocument)
        {
            var documents = new List<DocumentService.document>();
            if (!String.IsNullOrWhiteSpace(idNumberDocument))
            {
                documents = _dao.GetList<DocumentService.document>("SELECT * FROM Document WHERE idNumberClient = @IdNumberClient", new
                {
                    IdNumberClient = idNumberDocument
                }).ToList();
            }
            return View(documents.Select(x => new DocumentService.document { fullPath = Server.UrlEncode(x.fullPath), name = x.name, modifiedDate = x.modifiedDate }));
        }

        public FileResult Detail(string fullPath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.UrlDecode(fullPath));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(fullPath)); 
        }

        [HttpPost]
        public ActionResult Migrate(string fullPath)
        {
            fullPath = Server.UrlDecode(fullPath);
            string fileToMove = Path.Combine(_destinyFolder, Path.GetFileName(fullPath));
            if (System.IO.File.Exists(fileToMove))
                System.IO.File.Delete(fileToMove);
            System.IO.File.Move(fullPath, fileToMove);
            var partsFileName = Path.GetFileName(fullPath).Split('_');
            _dao.ExecQuery("DELETE FROM Document WHERE idNumberClient = @IdNumberClient AND Name LIKE CONCAT('%',@DocumentType,'%')", new
            {
                IdNumberClient = partsFileName[0],
                DocumentType = partsFileName[1]
            });
            ValidateDocuments(Path.GetFileNameWithoutExtension(fullPath).Split('_')[0]);
            return null;
        }
    }
}