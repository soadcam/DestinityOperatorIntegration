using OperadorDestinoCarpetaMock.DAO;
using System;
using System.Web.Http;
using System.Linq;

namespace OperadorDestinoCarpetaMock.Controllers
{
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        private DapperDAO _dao;

        public ClientController() =>
            _dao = new DapperDAO();

        [HttpGet]
        [Route("{idNumberClient}/{documentType}")]
        public IHttpActionResult GetIdentificationClient(string idNumberClient, string documentType)
        {
            if (String.IsNullOrWhiteSpace(idNumberClient) 
                || String.IsNullOrWhiteSpace(documentType))
                return BadRequest();
            var documentClient = _dao.GetList<DocumentService.document>("SELECT * FROM Document WHERE idNumberClient = @IdNumberClient AND Name LIKE CONCAT('%',@DocumentType,'%')", new
            {
                IdNumberClient = idNumberClient,
                DocumentType = documentType
            }).FirstOrDefault();
            if (documentClient == null)
                return Ok(documentClient);
            return Ok(new
            {
                fullPath = documentClient.fullPath,
                idNumberClient = documentClient.idNumberClient,
                modifiedDate = documentClient.modifiedDate,
                name = documentClient.name,
            });
        }
    }
}