namespace VaderSharp
{
    public struct SentimentAnalysisResults
    {
        public double Negative { get; set; }
        public double Neutral { get; set; }
        public double Positive { get; set; }
        public double Compound { get; set; }
    }
}
