namespace PragyoSala.Services.Dtos
{
    public class CommentRequest
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int ChapterId { get; set; }
        public int TopicId { get; set; }
    }
}