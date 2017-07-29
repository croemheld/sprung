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
        
        public List<Window> match(String pattern, List<Window> windows)
        {
            if (pattern.Length == 0) return windows;
            foreach (Window window in windows)
            {
                String title = window.getTitle().ToLower();
                pattern = pattern.ToLower();

                double phraseweight = 0.5;
                double wordweight = 1.0;
                int minweight = 10;
                int maxweight = 1;
                double lengthweight = -0.3;

                double vp = valuePhrase(pattern, title) - 0.8 * Math.Abs(title.Length - pattern.Length);
                double vw = valueWords(pattern, title);
                double sv = Math.Min(vp, vw) * 0.8 + Math.Max(vp, vw) * 0.2;

                double value = Math.Min(phraseweight * valuePhrase(pattern, title), wordweight * valueWords(pattern, title)) * minweight
                             + Math.Max(phraseweight * valuePhrase(pattern, title), wordweight * valueWords(pattern, title)) * maxweight
                             + lengthweight * pattern.Length;

                window.setMatchingPriority(value);
            }

            windows.Sort();
            return windows;
        }

    }
}
