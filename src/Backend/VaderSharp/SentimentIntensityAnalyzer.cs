using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Double;

namespace VaderSharp;

public class SentimentIntensityAnalyzer
{
    private const double ExclIncr = 0.292;
    private const double QuesIncrSmall = 0.18;
    private const double QuesIncrLarge = 0.96;

    static SentimentIntensityAnalyzer()
    {
        var assembly = typeof(SentimentIntensityAnalyzer).GetTypeInfo().Assembly;
        using var stream = assembly.GetManifestResourceStream("VaderSharp.vader_lexicon.txt");
        using var reader = new StreamReader(stream);
        LexiconFullFile = reader.ReadToEnd().Split('\n');
        Lexicon = MakeLexDic();
    }

    private static Dictionary<string, double> Lexicon { get; }
    private static string[] LexiconFullFile { get; }

    private static Dictionary<string, double> MakeLexDic()
    {
        return LexiconFullFile.Select(line => line.Trim().Split('\t'))
            .ToDictionary(lineArray => lineArray[0], lineArray => Parse(lineArray[1]));
    }
    public SentimentAnalysisResults PolarityScores(string input)
    {
        var sentiText = new SentiText(input);
        var sentiments = new List<double>();
        var wordsAndEmoticons = sentiText.WordsAndEmoticons;

        for (var i = 0; i < wordsAndEmoticons.Count; i++)
        {
            var item = wordsAndEmoticons[i];
            const double valence = 0;
            if ((i < wordsAndEmoticons.Count - 1 && item.ToLower() == "kind" && wordsAndEmoticons[i + 1] == "of")
                || SentimentUtils.BoosterDict.ContainsKey(item.ToLower()))
            {
                sentiments?.Add(valence);
                continue;
            }

            sentiments = SentimentValence(valence, sentiText, item, i, sentiments) as List<double>;
        }

        sentiments = ButCheck(wordsAndEmoticons, sentiments) as List<double>;

        return ScoreValence(sentiments, input);
    }

    private IEnumerable<double> SentimentValence(double valence, SentiText sentiText, string item, int i,
        ICollection<double> sentiments)
    {
        var itemLowerCase = item.ToLower();
        if (!Lexicon.ContainsKey(itemLowerCase))
        {
            sentiments.Add(valence);
            return sentiments;
        }

        var isCapDiff = sentiText.IsCapDifferential;
        var wordsAndEmoticons = sentiText.WordsAndEmoticons;
        valence = Lexicon[itemLowerCase];
        if (isCapDiff && item.IsUpper())
        {
            if (valence > 0)
                valence += SentimentUtils.CIncr;
            else
                valence -= SentimentUtils.CIncr;
        }

        for (var startI = 0; startI < 3; startI++)
            if (i > startI && !Lexicon.ContainsKey(wordsAndEmoticons[i - (startI + 1)].ToLower()))
            {
                var s = SentimentUtils.ScalarIncDec(wordsAndEmoticons[i - (startI + 1)], valence, isCapDiff);
                switch (startI)
                {
                    case 1 when s != 0:
                        s = s * 0.95;
                        break;
                    case 2 when s != 0:
                        s = s * 0.9;
                        break;
                }

                valence = valence + s;

                valence = NeverCheck(valence, wordsAndEmoticons, startI, i);

                if (startI == 2) valence = IdiomsCheck(valence, wordsAndEmoticons, i);
            }

        valence = LeastCheck(valence, wordsAndEmoticons, i);
        sentiments.Add(valence);
        return sentiments;
    }

    private static IEnumerable<double> ButCheck(IList<string> wordsAndEmoticons, IList<double> sentiments)
    {
        var containsBut = wordsAndEmoticons.Contains("BUT");
        var containsbut = wordsAndEmoticons.Contains("but");
        if (!containsBut && !containsbut)
            return sentiments;

        var butIndex = containsBut
            ? wordsAndEmoticons.IndexOf("BUT")
            : wordsAndEmoticons.IndexOf("but");

        for (var i = 0; i < sentiments.Count; i++)
        {
            var sentiment = sentiments[i];
            if (i < butIndex)
            {
                sentiments.RemoveAt(i);
                sentiments.Insert(i, sentiment * 0.5);
            }
            else if (i > butIndex)
            {
                sentiments.RemoveAt(i);
                sentiments.Insert(i, sentiment * 1.5);
            }
        }

        return sentiments;
    }

    private static double LeastCheck(double valence, IList<string> wordsAndEmoticons, int i)
    {
        switch (i)
        {
            case > 1 when !Lexicon.ContainsKey(wordsAndEmoticons[i - 1].ToLower()) && wordsAndEmoticons[i - 1].ToLower() == "least":
            {
                if (wordsAndEmoticons[i - 2].ToLower() != "at" && wordsAndEmoticons[i - 2].ToLower() != "very")
                    valence = valence * SentimentUtils.NScalar;
                break;
            }
            case > 0 when !Lexicon.ContainsKey(wordsAndEmoticons[i - 1].ToLower()) && wordsAndEmoticons[i - 1].ToLower() == "least":
                valence = valence * SentimentUtils.NScalar;
                break;
        }

        return valence;
    }

    private static double NeverCheck(double valence, IList<string> wordsAndEmoticons, int startI, int i)
    {
        switch (startI)
        {
            case 0:
            {
                if (SentimentUtils.Negated(new List<string> { wordsAndEmoticons[i - 1] }))
                    valence = valence * SentimentUtils.NScalar;
                break;
            }
            case 1 when wordsAndEmoticons[i - 2] == "never" &&
                        (wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this"):
                valence = valence * 1.5;
                break;
            case 1:
            {
                if (SentimentUtils.Negated(new List<string> { wordsAndEmoticons[i - (startI + 1)] }))
                    valence = valence * SentimentUtils.NScalar;
                break;
            }
            case 2 when (wordsAndEmoticons[i - 3] == "never"
                         && (wordsAndEmoticons[i - 2] == "so" || wordsAndEmoticons[i - 2] == "this")) ||
                        wordsAndEmoticons[i - 1] == "so" || wordsAndEmoticons[i - 1] == "this":
                valence = valence * 1.25;
                break;
            case 2:
            {
                if (SentimentUtils.Negated(new List<string> { wordsAndEmoticons[i - (startI + 1)] }))
                    valence = valence * SentimentUtils.NScalar;
                break;
            }
        }

        return valence;
    }

    private static double IdiomsCheck(double valence, IList<string> wordsAndEmoticons, int i)
    {
        var oneZero = string.Concat(wordsAndEmoticons[i - 1], " ", wordsAndEmoticons[i]);
        var twoOneZero = string.Concat(wordsAndEmoticons[i - 2], " ", wordsAndEmoticons[i - 1], " ",
            wordsAndEmoticons[i]);
        var twoOne = string.Concat(wordsAndEmoticons[i - 2], " ", wordsAndEmoticons[i - 1]);
        var threeTwoOne = string.Concat(wordsAndEmoticons[i - 3], " ", wordsAndEmoticons[i - 2], " ",
            wordsAndEmoticons[i - 1]);
        var threeTwo = string.Concat(wordsAndEmoticons[i - 3], " ", wordsAndEmoticons[i - 2]);

        string[] sequences = { oneZero, twoOneZero, twoOne, threeTwoOne, threeTwo };

        foreach (var seq in sequences)
            if (SentimentUtils.SpecialCaseIdioms.ContainsKey(seq))
            {
                valence = SentimentUtils.SpecialCaseIdioms[seq];
                break;
            }

        if (wordsAndEmoticons.Count - 1 > i)
        {
            var zeroOne = string.Concat(wordsAndEmoticons[i], " ", wordsAndEmoticons[i + 1]);
            if (SentimentUtils.SpecialCaseIdioms.ContainsKey(zeroOne))
                valence = SentimentUtils.SpecialCaseIdioms[zeroOne];
        }

        if (wordsAndEmoticons.Count - 1 > i + 1)
        {
            var zeroOneTwo = string.Concat(wordsAndEmoticons[i], " ", wordsAndEmoticons[i + 1], " ",
                wordsAndEmoticons[i + 2]);
            if (SentimentUtils.SpecialCaseIdioms.ContainsKey(zeroOneTwo))
                valence = SentimentUtils.SpecialCaseIdioms[zeroOneTwo];
        }

        if (SentimentUtils.BoosterDict.ContainsKey(threeTwo) || SentimentUtils.BoosterDict.ContainsKey(twoOne))
            valence += SentimentUtils.BDecr;
        return valence;
    }

    private double PunctuationEmphasis(string text)
    {
        return AmplifyExclamation(text) + AmplifyQuestion(text);
    }

    private static double AmplifyExclamation(string text)
    {
        var epCount = text.Count(x => x == '!');

        if (epCount > 4)
            epCount = 4;

        return epCount * ExclIncr;
    }

    private static double AmplifyQuestion(string text)
    {
        var qmCount = text.Count(x => x == '?');

        return qmCount switch
        {
            < 1 => 0,
            <= 3 => qmCount * QuesIncrSmall,
            _ => QuesIncrLarge
        };
    }

    private static SiftSentiments SiftSentimentScores(IList<double> sentiments)
    {
        var siftSentiments = new SiftSentiments();

        foreach (var sentiment in sentiments)
        {
            switch (sentiment)
            {
                case > 0:
                    siftSentiments.PosSum += sentiment + 1; //1 compensates for neutrals
                    break;
                case < 0:
                    siftSentiments.NegSum += sentiment - 1;
                    break;
                case 0:
                    siftSentiments.NeuCount++;
                    break;
            }
        }

        return siftSentiments;
    }

    private SentimentAnalysisResults ScoreValence(IList<double> sentiments, string text)
    {
        if (sentiments.Count == 0)
            return new SentimentAnalysisResults(); //will return with all 0

        var sum = sentiments.Sum();
        var puncAmplifier = PunctuationEmphasis(text);

        sum += Math.Sign(sum) * puncAmplifier;

        var compound = SentimentUtils.Normalize(sum);
        var sifted = SiftSentimentScores(sentiments);

        if (sifted.PosSum > Math.Abs(sifted.NegSum))
            sifted.PosSum += puncAmplifier;
        else if (sifted.PosSum < Math.Abs(sifted.NegSum)) sifted.NegSum -= puncAmplifier;

        var total = sifted.PosSum + Math.Abs(sifted.NegSum) + sifted.NeuCount;
        return new SentimentAnalysisResults
        {
            Compound = Math.Round(compound, 4),
            Positive = Math.Round(Math.Abs(sifted.PosSum / total), 3),
            Negative = Math.Round(Math.Abs(sifted.NegSum / total), 3),
            Neutral = Math.Round(Math.Abs(sifted.NeuCount / total), 3)
        };
    }

    private class SiftSentiments
    {
        public double PosSum { get; set; }
        public double NegSum { get; set; }
        public int NeuCount { get; set; }
    }
}