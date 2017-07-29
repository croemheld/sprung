using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprung
{
    class WindowMatcher
    {
        private WindowManager windowManager;

        public WindowMatcher(WindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public List<Window> match(String pattern)
        {
            return match(pattern, windowManager.getProcesses());
        }

<<<<<<< Updated upstream
=======
        private int levenshteinDistance(String s, String t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;
            
            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        private int valuePhrase(String s1, String s2)
        {
            return levenshteinDistance(s1, s2);
        }

        private int valueWords(String s1, String s2)
        {
            // All accepted delimeters
            Char[] delimeters = new Char[] {' ', '-', '_'};
            String[] w1 = s1.Split(delimeters);
            String[] w2 = s2.Split(delimeters);

            int distance, best, total = 0;

            foreach(String word1 in w1)
            {
                best = s2.Length;

                foreach(String word2 in w2)
                {
                    distance = levenshteinDistance(word1, word2);

                    if (distance < best) best = distance;
                    if (distance == 0) break;
                }

                total += best;
            }

            return total;
        }

>>>>>>> Stashed changes
        public List<Window> match(String pattern, List<Window> windows)
        {
            if (pattern.Length == 0) return windows;
            foreach (Window window in windows)
            {
                String title = window.getTitle().ToLower();
                pattern = pattern.ToLower();
<<<<<<< Updated upstream
                int matchingGroups = 0;
                int matchingChars = 0;
                int titleCharPos = 0;
                int textCharPos = 0;
                bool lastMatched = false;

                while (titleCharPos < title.Length && textCharPos < pattern.Length)
                {
                    char titleChar = title[titleCharPos];
                    char textChar = pattern[textCharPos];
                    if (titleChar == textChar)
                    {
                        titleCharPos++;
                        textCharPos++;
                        matchingChars++;
                        if (!lastMatched) matchingGroups++;
                        lastMatched = true;
                    }
                    else
                    {
                        titleCharPos++;
                        lastMatched = false;
                    }
                }

                window.setMatchingPriority(matchingChars);
                window.setMatchingGroups(matchingGroups);
=======

                double phraseweight = 0.5;
                double wordweight = 1.0;
                int minweight = 10;
                int maxweight = 1;
                double lengthweight = -0.3;

                //String[] words = title.Split(' ');

                // Matched substrings in title
                //matchingGroups = Regex.Matches(Regex.Escape(title), pattern).Count;

                // To calculate the Hamming distance for every tag
                //Dictionary<String, Tuple<int, int>> dictionary = new Dictionary<String, Tuple<int, int>>();

                //foreach (String word in words)
                //{
                //    if (dictionary.ContainsKey(word))
                //    {
                //        // Since tuples are immutable, we need to create a new tuple
                //        dictionary[word] = new Tuple<int, int>(dictionary[word].Item1 + 1, 0);
                //    }
                //    else
                //    {
                //        dictionary.Add(word, new Tuple<int, int>(1, 0));
                //    }
                //}

                // To avoid InvalidOperationException
                //String[] keys = dictionary.Keys.ToArray();

                // Levenshtein distance for each title word and the given pattern
                //foreach(String item in keys)
                //{
                //    Tuple<int, int> value = dictionary[item];
                //    dictionary[item] = new Tuple<int, int>(value.Item1, levenshteinDistance(item, pattern) / item.Length);
                //}

                //matchingChars = dictionary.Select(entry => entry.Value.Item2).Sum();

                double vp = valuePhrase(pattern, title) - 0.8 * Math.Abs(title.Length - pattern.Length);
                double vw = valueWords(pattern, title);
                double sv = Math.Min(vp, vw) * 0.8 + Math.Max(vp, vw) * 0.2;

                double value = Math.Min(phraseweight * valuePhrase(pattern, title), wordweight * valueWords(pattern, title)) * minweight
                             + Math.Max(phraseweight * valuePhrase(pattern, title), wordweight * valueWords(pattern, title)) * maxweight
                             + lengthweight * pattern.Length;

                window.setMatchingPriority(value);
                //window.setMatchingGroups(matchingGroups);
>>>>>>> Stashed changes
            }

            windows.Sort();
            return windows;
        }

    }
}
