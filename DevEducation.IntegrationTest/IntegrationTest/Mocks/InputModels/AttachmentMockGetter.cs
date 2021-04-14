namespace IntegrationTest.Models.InputModels
{
    public static class AttachmentInputModelMockGetter
    {
        public static AttachmentInputModel GetAttachmentInputModel(int id)
        {
            return id switch
            {
                1 => new AttachmentInputModel
                {
                    Description = "Test attach 1",
                    Path=@"C:\Example\Path",
                    AttachmentTypeId = 1
                },
                2 => new AttachmentInputModel
                {
                    Description = "Test attach 2",
                    Path = @"D:\Example\Path",
                    AttachmentTypeId = 2
                },
                3 => new AttachmentInputModel(),
                4 => new AttachmentInputModel
                {
                    Description = "Test attach 4",
                    Path = @"D:\Example\Path",
                },
                _ => null
            };
        }
    }
}
