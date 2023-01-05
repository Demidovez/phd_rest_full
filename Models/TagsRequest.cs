using System;

namespace phd_api.Models
{
    public class TagsRequest
    {
        public string[] tags;

        public TagsRequest(string[] tags)
        {
            this.tags = tags;
        }
    }
}
