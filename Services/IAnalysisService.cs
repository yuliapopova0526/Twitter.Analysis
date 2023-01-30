using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Twitter.Analysis.Models.Analysis;

namespace Twitter.Analysis.Services
{
    public interface IAnalysisService
    {
        Task<bool> StartAnalysis();

        Task<bool> StopAnalysis();

        Task<AnalysisModel> ReportCurrent();
    }
}
