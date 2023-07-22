namespace Jflutter.Entities;

public class FeedbackReports
{
    public ICollection<FeedbackReport> Reports { get; set; } = new List<FeedbackReport>();
}