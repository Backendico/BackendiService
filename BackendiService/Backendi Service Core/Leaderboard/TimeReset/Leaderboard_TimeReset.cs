using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackendiService.Backendi_Service_Core.Leaderboard.TimeReset
{
    class Leaderboard_TimeReset
    {

        MongoClient Client = new MongoClient();

        public async Task<TaskJob[]> ReciveTasks()
        {
            var Tasks = new TaskJob[] { };

            List<BsonDocument> Users = await Client.GetDatabase("Users").GetCollection<BsonDocument>("Users").FindAsync("{}").Result.ToListAsync();

            foreach (var item in Users)
            {


            }

            return Tasks;

        }

    }
}
