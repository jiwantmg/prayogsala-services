namespace PragyoSala.Services.Dtos
{
    public class TopicDto
    {
        public int TopicId { get; set; }
        public int Order { get; set; }
        public string Topic { get; set; }
        public string Video { get; set; }
        public double Length { get; set; }
        public bool IsWatched { get; set; }      
        public int  ChapterId {get; set; }        
        public int CourseId { get; set; }        
    }
}