namespace ScheduledCrawlerProject.Models
{
    public class SecondaryTag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class TemplateInfo
    {
        public string template_type { get; set; }
        public string template_title { get; set; }
        public List<string> template_cover { get; set; }
        public object template_extra { get; set; }
    }

    public class Feed
    {
        public int id { get; set; }
        public int project_id { get; set; }
        public int feed_id { get; set; }
        public string entity_type { get; set; }
        public int entity_id { get; set; }
        public string state { get; set; }
        public int is_hot { get; set; }
        public int is_pin { get; set; }
        public string template { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string web_cover { get; set; }
        public long order_num { get; set; }
        public string published_at { get; set; }
        public List<SecondaryTag> secondary_tag { get; set; }
        public List<string> images { get; set; }
        public int is_top { get; set; }
        public object extra { get; set; }
        public object extraction_tags { get; set; }
        public TemplateInfo template_info { get; set; }
        public string cover { get; set; }
        public object viewpoint { get; set; }
        public int favourite_num { get; set; }
    }
    public class Feeds
    {
        public List<Feed> feedList { get; set; }
    }

    public class RootObject
    {
        public Feeds feeds { get; set; }
    }
}
