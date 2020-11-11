using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace BackendiService.Backendi_Service_Core
{
    class TaskJob
    {
        internal DateTime StartAt { get; set; }
        internal bool IsRun { get; set; }
        internal ObjectId Token { get; set; }

        List<TaskJob> ListJobs;



        public TaskJob(Action JobTime, DateTime StartAt ,List<TaskJob> ListJobs)
        {
            this.JobTime += JobTime;
            this.StartAt = StartAt;
            this.ListJobs = ListJobs;

        }

        void RemoveJob()
        {
            ListJobs.Remove(this);
        }

        internal void Job() => JobTime();

        event Action JobTime;
    }
}
