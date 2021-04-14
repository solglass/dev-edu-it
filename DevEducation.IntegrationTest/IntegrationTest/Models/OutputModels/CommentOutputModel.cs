using System.Collections.Generic;
namespace IntegrationTest.Models.OutputModels
{
    public class CommentOutputModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public HomeworkAttemptOutputModel homeworkAttempt { get; set; }
        public UserOutputModel Author { get; set; }
        public List<AttachmentOutputModel> Attachments { get; set; }
    }
}
