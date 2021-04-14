using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class HomeworkAttemptOutputModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public AuthorOutputModel Author { get; set; }
        public string HomeworkAttemptStatus { get; set; }
        public List<CommentOutputModel> Comments { get; set; }
        public List<AttachmentOutputModel> Attachments { get; set; }
    }
}
