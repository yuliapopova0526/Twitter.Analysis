using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Tweetinvi;
using Tweetinvi.Models.V2;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.V2;
using Twitter.Analysis.Models.Analysis;
using Twitter.Analysis.Models.Configuration;

namespace Twitter.Analysis.Services
{
    public class AnalysisService : IAnalysisService
    {
        private AppSettings _settings;
        private TwitterClient _client;
        private ISampleStreamV2 _sampleStream;
        private AnalysisModel _model;
        private StreamState _state;

        public AnalysisService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _client=new TwitterClient(_settings.TwitterApi.ApiKey, _settings.TwitterApi.ApiSecret);
            _model = new AnalysisModel();
            _state = StreamState.Stop;
        }

        private async void ProcessSampleTweet(TweetV2 tweet)
        {
            await _model.Add(tweet.Text, tweet.Entities?.Hashtags?.Select(ht => ht.Tag).ToList());
        }

        public async Task<bool> StartAnalysis()
        {
            try
            {
                if (_state == StreamState.Running)
                    return true;

                if (_sampleStream == null)
                    await InitClient();
    
                _sampleStream.StartAsync();
            }
            catch(Exception ex)
            {
                //todo:log
                //restart client
                await InitClient();
                return false;
            }

            return true;
        }

        private async Task InitClient()
        {
            _state = StreamState.Stop;

            await _client.Auth.InitializeClientBearerTokenAsync();
            _sampleStream = _client.StreamsV2.CreateSampleStream();

            _sampleStream.EventReceived += _sampleStream_EventReceived;

           _sampleStream.TweetReceived += (sender, args) =>
            {
                _state = StreamState.Running;
                ProcessSampleTweet(args.Tweet);
            };
        }

        private void _sampleStream_EventReceived(object sender, Tweetinvi.Events.StreamEventReceivedArgs e)
        {
            //todo: analyze events
           var v= e.Json;
        }

        public async Task<bool> StopAnalysis()
        {
            if (_sampleStream != null)
            {
                _sampleStream.StopStream();
                _state = StreamState.Stop;
            }

            return true;
        }

        public async Task<AnalysisModel> ReportCurrent()
        {
            return _model;
        }
    }
}
