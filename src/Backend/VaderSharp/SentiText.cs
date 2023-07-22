using System.Collections.Generic;
using System.Linq;

namespace VaderSharp
{
    internal class SentiText
    {
        private string Text { get; }
        public IList<string> WordsAndEmoticons { get; }
        public bool IsCapDifferential { get; }

        public SentiText(string text)
        {
            Text = text;
            WordsAndEmoticons = GetWordsAndEmoticons();
            IsCapDifferential = SentimentUtils.AllCapDifferential(WordsAndEmoticons);
        }
        private Dictionary<string, string> WordsPlusPunc()
        {
            var noPuncText = Text.RemovePunctuation();
            var wordsOnly = noPuncText.Split().Where(x=>x.Length > 1);
            var puncDic = new Dictionary<string, string>();
            foreach (var word in wordsOnly)
            {
                foreach (var punc in SentimentUtils.PuncList)
                {
                    if (puncDic.ContainsKey(word + punc))
                        continue;

                    puncDic.Add(word + punc, word);
                    puncDic.Add(punc + word, word);
                }
            }
            return puncDic;
        }
        private IList<string> GetWordsAndEmoticons()
        {
            var wes = Text.Split().Where(x=> x.Length > 1).ToList();
            var wordsPuncDic = WordsPlusPunc();
            for (var i = 0; i < wes.Count; i++)
            {
                if (wordsPuncDic.ContainsKey(wes[i]))
                    wes[i] = wordsPuncDic[wes[i]];
            }
            return wes;
        }

    }
}
