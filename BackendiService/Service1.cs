using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BackendiService.Backendi_Service_Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackendiService
{
    public partial class BackendiService : ServiceBase
    {

        ServiceCore ServiceCore = new ServiceCore();


        public BackendiService()
        {
            InitializeComponent();
            this.ServiceName = "BackendiService";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            //start service
            ServiceCore.Run();

        }

        protected override void OnStop()
        {
            ServiceCore.OnError();
        }

        protected override void OnContinue()
        {

        }


    }
}
