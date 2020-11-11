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
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackendiService
{
    public partial class BackendiService : ServiceBase
    {

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

        }

        protected override void OnStop()
        {

        }

        protected override void OnContinue()
        {

        }


    }
}
