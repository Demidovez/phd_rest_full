using System;
using System.Data;

namespace phd_api.Models
{
    public class TagValue
    {
        public DateTime date;
        public string val;
        public short conf;
        public string units;

        public TagValue(DateTime date, string value, short confidence, string units)
        {
            this.date = date;
            this.val = value;
            this.conf = confidence;
            this.units = units;
        }

        public DateTime getDate() { return date; }
        public string getValue() { return val; }
        public short getConfidence() { return conf; }
        public void setDate(DateTime date) { 
            this.date = date; 
        }
        public void setValue(string value) { this.val = value;}
        public void setConfidence(short confidence) { this.conf = confidence; }
    }
}
