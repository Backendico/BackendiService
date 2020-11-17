using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendiService.Backendi_Service_Core.Leaderboard;

namespace BackendiService.Backendi_Service_Core
{
    class ServiceCore
    {


        ControllLeaderboard ControllLeaderboard = new ControllLeaderboard();


        internal async void Run()
        {
            while (true)
            {
                await Task.Delay(1000);

                //reset Time Leaderboards
                try
                {
                    List<Action> ResetTimes = await ControllLeaderboard.ReciveTasksResetTime();

                    foreach (var item in ResetTimes)
                    {
                        item();
                    }

                }
                catch (Exception)
                {

                }


                //control Backup leaderboard

                List<Action> RemoveBackup = await ControllLeaderboard.ReciveTaskRemoveBackups();

                foreach (var item in RemoveBackup)
                {
                    item();
                }


            }

        }



        internal void OnError()
        {

        }

    }


}
