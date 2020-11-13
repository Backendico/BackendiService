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

        public async Task<List<Action>> ReciveTasks()
        {
            var Tasks = new List<Action>();

            List<BsonDocument> Users = await Client.GetDatabase("Users").GetCollection<BsonDocument>("Users").FindAsync("{}").Result.ToListAsync();

            foreach (var InfoUser in Users)
            {
                for (int i = 0; i < InfoUser["Games"].AsBsonArray.Count; i++)
                {
                    BsonDocument SettingStudio = await Client.GetDatabase(InfoUser["Games"][i].AsString).GetCollection<BsonDocument>("Setting").FindAsync(new BsonDocument { { "_id", "Setting" } }).Result.SingleAsync();

                    foreach (BsonElement LeaderboardSetting in SettingStudio["Leaderboards"]["List"].AsBsonDocument)
                    {
                        switch (LeaderboardSetting.Value["Reset"].ToInt32())
                        {
                            case 1:
                                {
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddHours(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await Client.GetDatabase("").GetCollection<BsonDocument>("").FindAsync("");
                                        });
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddDays(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await Client.GetDatabase("").GetCollection<BsonDocument>("").FindAsync("");
                                        });
                                    }
                                }
                                break;
                            case 3:
                                {
                                    var Week = LeaderboardSetting.Value["Amount"].ToInt32() * 7;
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddDays(Week) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await Client.GetDatabase("").GetCollection<BsonDocument>("").FindAsync("");
                                        });
                                    }
                                }
                                break;
                            case 4:
                                {
                                if (LeaderboardSetting.Value["Start"].ToLocalTime().AddMonths(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                {
                                    Tasks.Add(async () =>
                                    {
                                        await Client.GetDatabase("").GetCollection<BsonDocument>("").FindAsync("");
                                    });
                                }

                                }
                                break;
                        }

                    }
                }

            }

            return Tasks;

        }
    }
}
