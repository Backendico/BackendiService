using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BackendiService.Backendi_Service_Core.Leaderboard.TimeReset;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackendiService.Backendi_Service_Core
{
    class ServiceCore
    {


        Leaderboard_TimeReset Leaderboard_TimeReset = new Leaderboard_TimeReset();

        List<TaskJob> JobsLeaderboard = new List<TaskJob>();

        internal async void Run()
        {
            while (true)
            {
                await Task.Delay(1);

                var LeaderboardTask = await Leaderboard_TimeReset.ReciveTasks();

            }
        }



        internal void OnError()
        {

        }

    }


}
