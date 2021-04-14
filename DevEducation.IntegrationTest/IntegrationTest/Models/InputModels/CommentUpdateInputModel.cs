using System.Collections.Generic;

namespace IntegrationTest.Models.InputModels
{
    public class CommentUpdateInputModel
    {
        public string Message { get; set; }
        public List<AttachmentInputModel> Attachments { get; set; }
    }
}
