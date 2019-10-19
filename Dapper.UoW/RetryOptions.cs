using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.UoW
{
    public class RetryOptions
    {
        public int MaxRetries { get; private set; }
        public int WaitMillis { get; private set; }
        //public bool Enabled { get; private set; }

        public RetryOptions()
        {
            this.MaxRetries = 3;
            this.WaitMillis = 100;
            //this.Enabled = true;
        }

        //public RetryOptions(bool Enabled)
        //{
        //    this.MaxRetries = 3;
        //    this.WaitMillis = 100;
        //    //this.Enabled = Enabled;
        //}

        public RetryOptions(int MaxRetries, int WaitMillis, bool Enabled)
        {
            this.MaxRetries = MaxRetries;
            this.WaitMillis = WaitMillis;
            //this.Enabled = Enabled;
        }


    }
}
