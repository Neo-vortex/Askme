using Jflutter.Entities;
using Jflutter.Utilities;

var x =  new FeedbackReports();
x.Reports.Add(new FeedbackReport()
{
    TeacherName = "Ali",
    InvalidFeedbacks = 5,
    TotalFeedbacks = 10 ,
    ValidFeedbacks = 20,
    NegativeValidFeedbacks = 6,
    NeutralValidFeedbacks = 2,
    PositiveValidFeedbacks = 6,
    FeedbacksWithBenefits = new List<string>(){"hello world", "bye world"},
    NegativeTotalFeedbacks = 5,
    NeutralTotalFeedbacks = 6,
    PositiveTotalFeedbacks = 4
    
});

Console.WriteLine(x.AsJson());