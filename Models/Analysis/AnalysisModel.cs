using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analysis.Models.Analysis
{
    public class AnalysisModel
    {
        public AnalysisModel()
        {
            _hashTags = new ConcurrentDictionary<string, int>();
            _intProperties = new ConcurrentDictionary<string, int>();
            _intProperties["TotalAnalyzed"] = 0;
            _dateProperties = new ConcurrentDictionary<string, DateTime>();
            _dateProperties["LastAnalyzed"] = DateTime.MinValue;
        }


        public int TotalAnalyzed { 
           get
            {
                return _intProperties["TotalAnalyzed"];
            }
           set
            {
                _intProperties["TotalAnalyzed"]=value;
            } 
        }

        public DateTime LastAnalyzed
        {
            get
            {
                return _dateProperties["LastAnalyzed"];
            }
            set
            {
                _dateProperties["LastAnalyzed"] = value;
            }
        }

        public AnalysisHashTag[] TopTenHashTags { 
           get
            {
                return _hashTags?.OrderByDescending(ht => ht.Value).Take(10).
                    Select(ht => new AnalysisHashTag { HashTag = ht.Key, Mentions = ht.Value }).ToArray();
            }
        }

        private ConcurrentDictionary<string, int> _hashTags;

        private ConcurrentDictionary<string, int> _intProperties;
        private ConcurrentDictionary<string, DateTime> _dateProperties;
        
        public async Task Add(string message,List<string> hashTags)
        {
            //todo: add asynchronous processing.
            hashTags?.ForEach(tag =>
            {
                if (_hashTags.ContainsKey(tag))
                    _hashTags[tag]++;
                else
                    _hashTags[tag] = 1;
            });

            TotalAnalyzed++;
            LastAnalyzed = DateTime.UtcNow;
        }
    }

    public class AnalysisHashTag
    {
        public string HashTag { get; set; }

        public int Mentions { get; set; }
    }
}
