using eMototCare.BLL.Services.ImportNoteServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/import-notes")]
    [ApiController]
    public class ImportNoteController : ControllerBase
    {
        private readonly IImportNoteService _importNoteService;

        public ImportNoteController(IImportNoteService importNoteService)
        {
            _importNoteService = importNoteService;
        }
    }
}
