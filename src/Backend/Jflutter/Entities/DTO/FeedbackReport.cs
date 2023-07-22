
namespace Jflutter.Entities;

public class FeedbackReport
{
    public  string TeacherName { get; set; }
    public int TotalFeedbacks { get; set; }
    public  int ValidFeedbacks { get; set; }
    public int InvalidFeedbacks { get; set; }
    public int PositiveValidFeedbacks { get; set; }
    public int NegativeValidFeedbacks { get; set; }
    public  int NeutralValidFeedbacks { get; set; }
    
    public  int PositiveTotalFeedbacks { get; set; }
    public  int NegativeTotalFeedbacks { get; set; }
    public  int NeutralTotalFeedbacks { get; set; }
    public List<string> FeedbacksWithBenefits { get; set; } = new List<string>();
}