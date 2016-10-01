//-----------------------------------------------------------------------
// <copyright file="JobTests.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder.Test
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Xunit;

    /// <summary>
    /// Job tests.
    /// </summary>
    public class JobTests : TestBase
    {
        #region Jobs Response JSON

        /// <summary>
        /// Test JSON for job details response deserialization.
        /// </summary>
        private const string JobDetailsResponseJson =
            @"{
              ""job"": {
                ""created_at"": ""2010-01-01T00:00:00Z"",
                ""finished_at"": ""2010-01-01T00:00:00Z"",
                ""updated_at"": ""2010-01-01T00:00:00Z"",
                ""submitted_at"": ""2010-01-01T00:00:00Z"",
                ""pass_through"": null,
                ""id"": 1,
                ""input_media_file"": {
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""frame_rate"": 29,
                  ""finished_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""duration_in_ms"": 24883,
                  ""audio_sample_rate"": 48000,
                  ""url"": ""s3://bucket/test.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 95,
                  ""audio_codec"": ""aac"",
                  ""height"": 352,
                  ""file_size_bytes"": 1862748,
                  ""video_codec"": ""h264"",
                  ""test"": false,
                  ""channels"": ""2"",
                  ""width"": 624,
                  ""video_bitrate_in_kbps"": 498,
                  ""state"": ""finished""
                },
                ""test"": false,
                ""output_media_files"": [{
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""frame_rate"": 29,
                  ""finished_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""duration_in_ms"": 24883,
                  ""audio_sample_rate"": 44100,
                  ""url"": ""http://s3.amazonaws.com/bucket/video.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 92,
                  ""audio_codec"": ""aac"",
                  ""height"": 352,
                  ""file_size_bytes"": 1386663,
                  ""video_codec"": ""h264"",
                  ""test"": false,
                  ""channels"": ""2"",
                  ""width"": 624,
                  ""video_bitrate_in_kbps"": 351,
                  ""state"": ""finished"",
                  ""label"": ""Web""
                }],
                ""thumbnails"": [{
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""url"": ""http://s3.amazonaws.com/bucket/video/frame_0000.png"",
                  ""id"": 1
                }],
                ""state"": ""finished""
              }
            }";

        /// <summary>
        /// Test JSON for job details response deserialization.
        /// Note additional new test values including: total_bitrate_in_kbps,
        /// non-integer framerate, thumbnail group label, thumbnail format
        /// </summary>
        private const string JobDetailsResponseTestSetTwoJson =
            @"{
              ""job"": {
              ""created_at"": ""2011-04-04T11:21:14-05:00"",
              ""finished_at"": ""2011-04-04T11:22:16-05:00"",
              ""updated_at"": ""2011-04-04T11:22:16-05:00"",
              ""submitted_at"": ""2011-04-04T11:21:14-05:00"",
                ""pass_through"": null,
                ""id"": 1,
                ""input_media_file"": {
                  ""total_bitrate_in_kbps"": 6524,
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2011-04-04T18:21:14+02:00"",
                  ""frame_rate"": 25.05,
                  ""finished_at"": ""2011-04-04T18:21:32+02:00"",
                  ""updated_at"": ""2011-04-04T18:21:32+02:00"",
                  ""duration_in_ms"": 122000,
                  ""audio_sample_rate"": 32000,
                  ""url"": ""http://example.com/test.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 1024,
                  ""audio_codec"": ""pcm_s16le"",
                  ""height"": 576,
                  ""file_size_bytes"": 100299080,
                  ""video_codec"": ""h264"",
                  ""test"": true,
                  ""channels"": ""2"",
                  ""width"": 720,
                  ""video_bitrate_in_kbps"": 5500,
                  ""state"": ""finished""
                },
                ""test"": false,
                ""output_media_files"": [{
                  ""total_bitrate_in_kbps"": 586,
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""frame_rate"": 29,
                  ""finished_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""duration_in_ms"": 5080,
                  ""audio_sample_rate"": 32000,
                  ""url"": ""http://s3.amazonaws.com/bucket/video.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 60,
                  ""audio_codec"": ""aac"",
                  ""height"": 360,
                  ""file_size_bytes"": 375236,
                  ""video_codec"": ""h264"",
                  ""test"": false,
                  ""channels"": ""2"",
                  ""width"": 640,
                  ""video_bitrate_in_kbps"": 526,
                  ""state"": ""finished"",
                  ""label"": ""Web""
                }],
                ""thumbnails"": [{
                  ""group_label"": ""group-label-value-1"",
                  ""format"": ""png"",
                  ""created_at"": ""2011-04-04T11:22:16-05:00"",
                  ""updated_at"": ""2011-04-04T11:22:16-05:00"",
                  ""url"": ""http://s3.amazonaws.com/bucket/video/frame_0000.png"",
                  ""id"": 1,
                  ""height"": 360,
                  ""file_size_bytes"": 417387,
                  ""width"": 640
                  },
                  {
                  ""group_label"": ""group-label-value-1"",
                  ""format"": ""png"",
                  ""created_at"": ""2011-04-04T11:22:16-05:00"",
                  ""updated_at"": ""2011-04-04T11:22:16-05:00"",
                  ""url"": ""http://s3.amazonaws.com/bucket/video/frame_0001.png"",
                  ""id"": 5829389,
                  ""height"": 360,
                  ""file_size_bytes"": 382938,
                  ""width"": 640
                }],
                ""state"": ""finished""
              }
            }";

        /// <summary>
        /// Test JSON for list jobs response deserialization.
        /// </summary>
        private const string ListJobsResponseJson =
            @"[{
              ""job"": {
                ""created_at"": ""2010-01-01T00:00:00Z"",
                ""finished_at"": ""2010-01-01T00:00:00Z"",
                ""updated_at"": ""2010-01-01T00:00:00Z"",
                ""submitted_at"": ""2010-01-01T00:00:00Z"",
                ""pass_through"": null,
                ""id"": 1,
                ""input_media_file"": {
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""frame_rate"": 29,
                  ""finished_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""duration_in_ms"": 24883,
                  ""audio_sample_rate"": 48000,
                  ""url"": ""s3://bucket/test.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 95,
                  ""audio_codec"": ""aac"",
                  ""height"": 352,
                  ""file_size_bytes"": 1862748,
                  ""video_codec"": ""h264"",
                  ""test"": false,
                  ""channels"": ""2"",
                  ""width"": 624,
                  ""video_bitrate_in_kbps"": 498,
                  ""state"": ""finished""
                },
                ""test"": false,
                ""output_media_files"": [{
                  ""format"": ""mpeg4"",
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""frame_rate"": 29,
                  ""finished_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""duration_in_ms"": 24883,
                  ""audio_sample_rate"": 44100,
                  ""url"": ""http://s3.amazonaws.com/bucket/video.mp4"",
                  ""id"": 1,
                  ""error_message"": null,
                  ""error_class"": null,
                  ""audio_bitrate_in_kbps"": 92,
                  ""audio_codec"": ""aac"",
                  ""height"": 352,
                  ""file_size_bytes"": 1386663,
                  ""video_codec"": ""h264"",
                  ""test"": false,
                  ""channels"": ""2"",
                  ""width"": 624,
                  ""video_bitrate_in_kbps"": 351,
                  ""state"": ""finished"",
                  ""label"": ""Web""
                }],
                ""thumbnails"": [{
                  ""created_at"": ""2010-01-01T00:00:00Z"",
                  ""updated_at"": ""2010-01-01T00:00:00Z"",
                  ""url"": ""http://s3.amazonaws.com/bucket/video/frame_0000.png"",
                  ""id"": 1
                }],
                ""state"": ""finished""
              }
            }]";

        #endregion

        /// <summary>
        /// Cancel job request tests.
        /// </summary>
        [Fact]
        public void JobCancelJobRequest()
        {
            CreateJobResponse createResponse = Zencoder.CreateJob("s3://bucket-name/file-name.avi", null, null, null, true, false);
            Assert.True(createResponse.Success);

            CancelJobResponse cancelResponse = Zencoder.CancelJob(createResponse.Id);
            Assert.True(cancelResponse.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.CancelJob(
                createResponse.Id, 
                r =>
                {
                    Assert.True(r.InConflict);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Create job request tests.
        /// </summary>
        [Fact]
        public void JobCreateJobRequest()
        {
            Output[] outputs = new Output[]
            {
                new Output()
                {
                    Label = "iPhone",
                    Url = "s3://output-bucket/output-file-1-name.mp4",
                    Width = 480,
                    Height = 320
                },
                new Output() 
                {
                    Label = "WebHD",
                    Url = "s3://output-bucket/output-file-2-name.mp4",
                    Width = 1280,
                    Height = 720
                }
            };

            CreateJobResponse response = Zencoder.CreateJob("s3://bucket-name/file-name.avi", outputs, null, null, true, true);
            Assert.True(response.Success);
            Assert.Equal(outputs.Count(), response.Outputs.Count());
            
            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.CreateJob(
                "s3://bucket-name/file-name.avi",
                null,
                3,
                "asia",
                true,
                true,
                r =>
                {
                    Assert.True(r.Success);
                    Assert.True(r.Outputs.Count() > 0);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Create job request to JSON tests.
        /// </summary>
        [Fact]
        public void JobCreateJobRequestToJson()
        {
            const string One = @"{{""input"":""s3://bucket-name/file-name.avi"",""api_key"":""{0}""}}";
            const string Two = @"{{""download_connections"":3,""input"":""s3://bucket-name/file-name.avi"",""region"":""asia"",""api_key"":""{0}""}}";

            CreateJobRequest request = new CreateJobRequest(Zencoder)
            {
                Input = "s3://bucket-name/file-name.avi"
            };

            Assert.Equal(string.Format(CultureInfo.InvariantCulture, One, ApiKey), request.ToJson());

            request = new CreateJobRequest(Zencoder)
            {
                DownloadConnections = 3,
                Input = "s3://bucket-name/file-name.avi",
                Region = "asia"
            };

            Assert.Equal(string.Format(CultureInfo.InvariantCulture, Two, ApiKey), request.ToJson());
        }

        /// <summary>
        /// Create job response from JSON tests.
        /// </summary>
        [Fact]
        public void JobCreateJobResponseFromJson()
        {
            CreateJobResponse response = CreateJobResponse.FromJson(@"{""id"":""1234"",""outputs"":[{""id"":""4321""}]}");
            Assert.Equal(1234, response.Id);
            Assert.Equal(1, response.Outputs.Length);
            Assert.Equal(4321, response.Outputs.First().Id);
        }

        /// <summary>
        /// Delete job request tests.
        /// </summary>
        [Fact]
        public void JobDeleteJobRequest()
        {
            CreateJobResponse createResponse = Zencoder.CreateJob("s3://bucket-name/file-name.avi", null, null, null, true, false);
            Assert.True(createResponse.Success);

            // TODO: Investigate whether Zencoder has truly deprecated this API operation.
            // For now, just test for an InConflict status, because that's what it seems
            // we should expect.
            DeleteJobResponse deleteResponse = Zencoder.DeleteJob(createResponse.Id);
            Assert.True(deleteResponse.InConflict);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.DeleteJob(
                createResponse.Id, 
                r =>
                {
                    Assert.True(r.InConflict);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Job details from JSON tests.
        /// </summary>
        [Fact]
        public void JobJobDetailsFromJson()
        {
            JobDetailsResponse response = JobDetailsResponse.FromJson(JobDetailsResponseJson);
            Assert.Equal(new DateTime(2010, 1, 1), response.Job.FinishedAt);
            Assert.Equal(1, response.Job.Id);
            Assert.Equal(JobState.Finished, response.Job.State);

            Assert.Equal("mpeg4", response.Job.InputMediaFile.Format);
            Assert.Equal(24883, response.Job.InputMediaFile.DurationInMiliseconds);
            Assert.Equal(2, response.Job.InputMediaFile.Channels);
            Assert.Equal("h264", response.Job.InputMediaFile.VideoCodec);
            Assert.Equal(1, response.Job.OutputMediaFiles.Length);
        }

        /// <summary>
        /// Job details from JSON tests.
        /// </summary>
        [Fact]
        public void JobJobDetailsTestSetTwoFromJson()
        {
            JobDetailsResponse response = JobDetailsResponse.FromJson(JobDetailsResponseTestSetTwoJson);
            Assert.Equal(new DateTimeOffset(2011, 4, 4, 11, 22, 16, TimeSpan.FromHours(-5)).ToUniversalTime(), response.Job.FinishedAt.Value.ToUniversalTime());
            Assert.Equal(1, response.Job.Id);
            Assert.Equal(JobState.Finished, response.Job.State);

            Assert.Equal("mpeg4", response.Job.InputMediaFile.Format);
            Assert.Equal(122000, response.Job.InputMediaFile.DurationInMiliseconds);
            Assert.Equal(2, response.Job.InputMediaFile.Channels);
            Assert.Equal("h264", response.Job.InputMediaFile.VideoCodec);
            Assert.Equal(1, response.Job.OutputMediaFiles.Length);

            Assert.Equal("pcm_s16le", response.Job.InputMediaFile.AudioCodec);
            Assert.Equal(25.05f, response.Job.InputMediaFile.FrameRate);
            Assert.Equal(6524, response.Job.InputMediaFile.TotalBitrateInKbps);
            Assert.Equal(586, response.Job.OutputMediaFiles[0].TotalBitrateInKbps);
            
            // TODO: implement ability to get thumbnail element of response.
            // Assert.Equal("group-label-value-1", response.Job.
        }

        /// <summary>
        /// Job details request tests.
        /// </summary>
        [Fact]
        public void JobJobDetailsRequest()
        {
            CreateJobResponse createResponse = Zencoder.CreateJob("s3://bucket-name/file-name.avi", null, null, null, true, false);
            Assert.True(createResponse.Success);

            JobDetailsResponse detailsResponse = Zencoder.JobDetails(createResponse.Id);
            Assert.True(detailsResponse.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.JobDetails(
                createResponse.Id, 
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Job progress request tests.
        /// </summary>
        [Fact]
        public void JobJobProgressRequest()
        {
            Output output = new Output()
            {
                Label = "iPhone",
                Url = "s3://output-bucket/output-file-1-name.mp4",
                Width = 480,
                Height = 320
            };

            CreateJobResponse createResponse = Zencoder.CreateJob("s3://bucket-name/file-name.avi", new Output[] { output });
            Assert.True(createResponse.Success);

            JobProgressResponse progressResponse = Zencoder.JobProgress(createResponse.Outputs.First().Id);
            Assert.True(progressResponse.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.JobProgress(
                createResponse.Outputs.First().Id,
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Job progress response from JSON tests.
        /// </summary>
        [Fact]
        public void JobJobProgressResponseFromJson()
        {
            JobProgressResponse response = JobProgressResponse.FromJson(@"{""state"":""processing"",""current_event"":""Transcoding"",""progress"":""32.34567345""}");
            Assert.Equal(OutputState.Processing, response.State);
            Assert.Equal(OutputEvent.Transcoding, response.CurrentEvent);
            Assert.Equal(32.34567345, response.Progress);
        }

        /// <summary>
        /// List jobs request tests.
        /// </summary>
        [Fact]
        public void JobListJobsRequest()
        {
            ListJobsResponse response = Zencoder.ListJobs();
            Assert.True(response.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.ListJobs(
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// List jobs from JSON tests.
        /// </summary>
        [Fact]
        public void JobListJobsFromJson()
        {
            ListJobsResponse response = ListJobsResponse.FromJson(ListJobsResponseJson);
            Assert.Equal(1, response.Jobs.Length);

            Job first = response.Jobs.First();
            Assert.Equal(new DateTime(2010, 1, 1), first.FinishedAt);
            Assert.Equal(1, first.Id);
            Assert.Equal(JobState.Finished, first.State);

            Assert.Equal("mpeg4", first.InputMediaFile.Format);
            Assert.Equal(24883, first.InputMediaFile.DurationInMiliseconds);
            Assert.Equal(2, first.InputMediaFile.Channels);
            Assert.Equal("h264", first.InputMediaFile.VideoCodec);
            Assert.Equal(1, first.OutputMediaFiles.Length);

            OutputMediaFile output = first.OutputMediaFiles.First();
            Assert.Equal(AudioCodec.Aac, output.AudioCodec);
            Assert.Equal(false, output.Test);
        }

        /// <summary>
        /// Nested async job request tests.
        /// </summary>
        [Fact]
        public void JobNestedAsyncRequests()
        {
            ManualResetEvent[] handles = new ManualResetEvent[] 
            { 
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };

            // Nested async calls.
            Zencoder.CreateJob(
                "s3://bucket-name/file-name.avi",
                null,
                3,
                "asia",
                true,
                false,
                r =>
                {
                    Assert.True(r.Success);

                    Zencoder.JobDetails(
                        r.Id,
                        dr =>
                        {
                            Assert.True(dr.Success);
                            handles[0].Set();
                        });
                });

            // Async call then a sync call.
            Zencoder.CreateJob(
                "s3://bucket-name/file-name.avi",
                null,
                3,
                "asia",
                true,
                false,
                r =>
                {
                    Assert.True(r.Success);
                    Assert.True(Zencoder.JobDetails(r.Id).Success);
                    handles[1].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Resubmit job request tests.
        /// </summary>
        [Fact]
        public void JobResubmitJobRequest()
        {
            CreateJobResponse createResponse = Zencoder.CreateJob("s3://bucket-name/file-name.avi", null, null, null, true, false);
            Assert.True(createResponse.Success);

            ResubmitJobResponse resubmitResponse = Zencoder.ResubmitJob(createResponse.Id);
            Assert.True(resubmitResponse.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.ResubmitJob(
                createResponse.Id, 
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }
    }
}
