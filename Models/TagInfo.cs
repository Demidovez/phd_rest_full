namespace phd_api.Models
{
    public class TagInfo
    {
        public string tagname;
        public string tagnumber;
        public string desc;
        public string unit;

        public TagInfo(string tagname, string tagnumber, string desc, string unit)
        {
            this.tagname = tagname;
            this.tagnumber = tagnumber;
            this.desc = desc;
            this.unit = unit;
        }
    }
}
