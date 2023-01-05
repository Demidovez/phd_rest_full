using System;

namespace phd_api.Models
{
    public class TagsPeriodRequest
    {
        public string[] tags;
        public DateTime starttime;
        public DateTime endtime;

        public TagsPeriodRequest(string[] tags, string starttime = null, string endtime = null)
        {
            this.tags = tags;
            this.starttime = (starttime != null && starttime != "") ? DateTime.Parse(starttime) : DateTime.Now.AddSeconds(-5);
            this.endtime = (endtime != null && endtime != "") ? DateTime.Parse(endtime) : DateTime.Now;
        }
    }
}
