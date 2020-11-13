﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;

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
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddMinutes(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(() =>
                                       {
                                           ResetLeaderboard(InfoUser["AccountSetting"]["Token"].ToString(), InfoUser["Games"][i].AsString, LeaderboardSetting.Name);
                                       });
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddDays(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add( () =>
                                        {
                                            ResetLeaderboard(InfoUser["AccountSetting"]["Token"].ToString(), InfoUser["Games"][i].AsString, LeaderboardSetting.Name);
                                        });
                                    }
                                }
                                break;
                            case 3:
                                {
                                    var Week = LeaderboardSetting.Value["Amount"].ToInt32() * 7;
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddDays(Week) <= DateTime.Now)
                                    {
                                        Tasks.Add(() =>
                                       {
                                           ResetLeaderboard(InfoUser["AccountSetting"]["Token"].ToString(), InfoUser["Games"][i].AsString, LeaderboardSetting.Name);
                                       });
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if (LeaderboardSetting.Value["Start"].ToLocalTime().AddMonths(LeaderboardSetting.Value["Amount"].ToInt32()) <= DateTime.Now)
                                    {
                                        Tasks.Add(() =>
                                        {
                                            ResetLeaderboard(InfoUser["AccountSetting"]["Token"].ToString(), InfoUser["Games"][i].AsString, LeaderboardSetting.Name);
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

        public async void ResetLeaderboard(string Token, string Studio, string NameLeaderboard)
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
    }
}
