namespace phd_api.Models
{
    public class TagItem
    {
        public string name;
        public TagValue[] data;

        public TagItem(string name, TagValue[] data)
        {
            this.name = name;
            this.data = data;
        }

        public string getName() { return name; }
        public TagValue[] getData() { return data; }
        public void setName(string name) { this.name = name;}
        public void setData(TagValue[] data) { this.data = data; }
    }
}
