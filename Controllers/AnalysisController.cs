using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitter.Analysis.Models.Analysis;
using Twitter.Analysis.Services;

namespace Twitter.Analysis.Controllers
{
    [ApiController]
    [Route("analysis")]
    public class AnalysisController : ControllerBase
    {
       

        private readonly ILogger<AnalysisController> _logger;
        private readonly IAnalysisService _service;

        public AnalysisController(ILogger<AnalysisController> logger,IAnalysisService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("start")]
        public async Task<bool> StartAnalysis()
        {
            bool res = await _service.StartAnalysis();
            return res;
        }


        [HttpGet]
        [Route("report")]
        public async Task<AnalysisModel> ReportAnalysis()
        {
            var res = await _service.ReportCurrent();
            return res;
        }

        [HttpGet]
        [Route("stop")]
        public async Task<bool> StopAnalysis()
        {
            bool res = await _service.StopAnalysis();
            return res;
        }
    }
}
