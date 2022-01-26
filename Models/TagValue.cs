using System;

namespace phd_api.Models
{
    public class TagValue
    {
        public long timestamp;
        public string date;
        public Double value;
        public short confidence;

        public TagValue(long timestamp, string date, Double value, short confidence)
        {
            this.timestamp = timestamp;
            this.date = date;
            this.value = value;
            this.confidence = confidence;
        }

        public long getTimestamp() { return timestamp; }
        public string getDate() { return date; }
        public Double getValue() { return value; }
        public short getConfidence() { return confidence; }
        public void setTimestamp(long timestamp) { this.timestamp = timestamp; }
        public void setDate(string date) { this.date = date; }
        public void setValue(Double value) { this.value = value;}
        public void setConfidence(short confidence) { this.confidence = confidence; }
    }
}
