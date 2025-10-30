

using AutoMapper;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.ImportNoteServices
{
    public class ImportNoteService : IImportNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ImportNoteService> _logger;
        public ImportNoteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ImportNoteService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
