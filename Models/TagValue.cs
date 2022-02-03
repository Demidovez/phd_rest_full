using System;

namespace phd_api.Models
{
    public class TagValue
    {
        public DateTime date;
        public Double val;
        public short conf;

        public TagValue(DateTime date, Double value, short confidence)
        {
            this.date = date;
            this.val = value;
            this.conf = confidence;
        }

        public DateTime getDate() { return date; }
        public Double getValue() { return val; }
        public short getConfidence() { return conf; }
        public void setDate(DateTime date) { 
            this.date = date; 
        }
        public void setValue(Double value) { this.val = value;}
        public void setConfidence(short confidence) { this.conf = confidence; }
    }
}
