using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;

namespace BackendiService.Backendi_Service_Core.Leaderboard
{
    class ControllLeaderboard
    {

        MongoClient Client = new MongoClient();

        public async Task<List<Action>> ReciveTasksResetTime()
        {
            var Tasks = new List<Action>();

            List<BsonDocument> Users = await Client.GetDatabase("Users").GetCollection<BsonDocument>("Users").FindAsync("{}").Result.ToListAsync();

            //find Users
            foreach (BsonDocument InfoUser in Users)
            {
                //find User Studios
                foreach (var Studio in InfoUser["Games"].AsBsonArray)
                {

                    BsonDocument SettingStudio = await Client.GetDatabase(Studio.AsString).GetCollection<BsonDocument>("Setting").FindAsync(new BsonDocument { { "_id", "Setting" } }).Result.SingleAsync();

                    //Find List Leaderboards
                    foreach (BsonElement LeaderboardSetting in SettingStudio["Leaderboards"]["List"].AsBsonDocument)
                    {

                        switch (LeaderboardSetting.Value["Reset"].ToInt32())
                        {
                            case 1:
                                {
                                    if (DateTime.Parse(LeaderboardSetting.Value["Start"].ToString()).AddHours(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await ResetLeaderboard(InfoUser["AccountSetting"]["Token"].AsString, Studio.AsString, LeaderboardSetting.Name);
                                        });
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (DateTime.Parse(LeaderboardSetting.Value["Start"].ToString()).AddDays(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await ResetLeaderboard(InfoUser["AccountSetting"]["Token"].AsString, Studio.AsString, LeaderboardSetting.Name);
                                        });
                                    }
                                }
                                break;
                            case 3:
                                {
                                    var Week = LeaderboardSetting.Value["Amount"].ToInt32() * 7;
                                    if (DateTime.Parse(LeaderboardSetting.Value["Start"].ToString()).AddDays(Week) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await ResetLeaderboard(InfoUser["AccountSetting"]["Token"].AsString, Studio.AsString, LeaderboardSetting.Name);
                                        });
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if (DateTime.Parse(LeaderboardSetting.Value["Start"].ToString()).AddMonths(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(async () =>
                                        {
                                            await ResetLeaderboard(InfoUser["AccountSetting"]["Token"].AsString, Studio.AsString, LeaderboardSetting.Name);
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


        public async Task<List<Action>> ReciveTaskRemoveBackups()
        {
            var Tasks = new List<Action>();

            List<BsonDocument> Users = await Client.GetDatabase("Users").GetCollection<BsonDocument>("Users").FindAsync("{}").Result.ToListAsync();

            //findUsers
            foreach (var InfoUser in Users)
            {
                //find Studios
                try
                {

                    foreach (var Studios in InfoUser["Games"].AsBsonArray)
                    {
                        var StudioSetting = await Client.GetDatabase(Studios.AsString).GetCollection<BsonDocument>("Setting").FindAsync(new BsonDocument { { "_id", "Setting" } }).Result.SingleAsync();
                        //find all Leaderboard
                        foreach (var Leaderboards in StudioSetting["Leaderboards"]["List"].AsBsonDocument)
                        {
                            //singel Leaderboard
                            foreach (var Leaderboard in Leaderboards.Value.AsBsonDocument)
                            {
                                if (Leaderboard.Name == "Backups")
                                {
                                    foreach (var Backup in Leaderboard.Value.AsBsonDocument)
                                    {
                                        if (Backup.Value.AsBsonDocument["List"].AsBsonDocument.ElementCount >= 1)
                                        {
                                            if (Backup.Value.AsBsonDocument["Detail"].AsBsonDocument["End"].ToLocalTime().AddMonths(1) <= DateTime.Now)
                                            {
                                                DeleteLeaderboard(StudioSetting["Setting"].AsBsonDocument["Creator"].ToString(), Studios.AsString, Leaderboards.Name, Backup.Name);
                                            }
                                        }
                                        else
                                        {
                                            DeleteLeaderboard(StudioSetting["Setting"].AsBsonDocument["Creator"].ToString(), Studios.AsString, Leaderboards.Name, Backup.Name);
                                        }

                                    }
                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return Tasks;

        }

        public async Task ResetLeaderboard(string Token, string Studio, string NameLeaderboard)
        {
            //send data
            var client = new RestClient("http://193.141.64.203/PageLeaderBoard/Reset");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("Token", Token);
            request.AddParameter("Studio", Studio);
            request.AddParameter("NameLeaderboard", NameLeaderboard);
            await client.ExecuteAsync(request);
        }

        public async void DeleteLeaderboard(string Token, string Studio, string NameLeaderboard, string Version)
        {
            var client = new RestClient("http://193.141.64.203/PageLeaderBoard/RemoveBackup");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddParameter("Token", Token);
            request.AddParameter("Studio", Studio);
            request.AddParameter("NameLeaderboard", NameLeaderboard);
            request.AddParameter("Version", Version);
            var Result = await client.ExecuteAsync(request);
            Debug.WriteLine(Result.StatusCode);
        }
    }
}
