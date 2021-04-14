namespace IntegrationTest.Models.InputModels
{
    public static class CommentInputModelMockGetter
    {
        public static CommentInputModel GetCommentInputModelMock(int id)
        {
            return id switch
            {
                1 => new CommentInputModel
                {
                    Message = "Test 1 mock",
                },
                2 => new CommentInputModel
                {
                    Message = "Test 2 mock",
                },
                3 => new CommentInputModel
                {
                    Message = "Test 3 mock",
                },
                _ => null,
            };
        }
    }
}
