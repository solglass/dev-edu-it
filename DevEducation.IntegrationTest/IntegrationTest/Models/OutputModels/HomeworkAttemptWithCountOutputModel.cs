namespace IntegrationTest.Models.OutputModels
{
  public class HomeworkAttemptWithCountOutputModel: HomeworkAttemptOutputModel
  {

    public int StatusId { get; set; }
    public int CountAttachments { get; set; }
    public int CountComments { get; set; }
    public HomeworkOutputModel Homework { get; set; }
  }
}
