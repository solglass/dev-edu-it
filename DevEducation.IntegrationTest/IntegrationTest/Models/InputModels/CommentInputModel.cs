using System.Collections.Generic;

namespace IntegrationTest.Models.InputModels
{
    public class CommentInputModel
    {
        public int AuthorId { get; set; }
        public string Message { get; set; }
        public List<AttachmentInputModel> Attachments { get; set; }
    }
}
