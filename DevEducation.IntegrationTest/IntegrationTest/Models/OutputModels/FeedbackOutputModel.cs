namespace IntegrationTest.Models.OutputModels
{
    public class FeedbackOutputModel
    {
        public int Id { get; set; }
        public AuthorOutputModel User { get; set; }
        public string Message { get; set; }
        public LessonOutputModel Lesson { get; set; }
        public string UnderstandingLevel { get; set; }
       
    }
}
