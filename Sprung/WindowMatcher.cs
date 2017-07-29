using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        private int LevenshteinDistance(String s, String t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }
            
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

        public List<Window> match(String pattern, List<Window> windows)
        {
            if (pattern.Length == 0) return windows;

            foreach (Window window in windows)
            {
                String title = window.getTitle().ToLower();
                pattern = pattern.ToLower();
                int matchingGroups = 0;
                int matchingChars = 0;

                String[] words = title.Split(' ');

                // Matched substrings in title
                matchingGroups = Regex.Matches(Regex.Escape(title), pattern).Count;

                // To calculate the Hamming distance for every tag
                Dictionary<String, Tuple<int, int>> dictionary = new Dictionary<String, Tuple<int, int>>();

                foreach (String word in words)
                {
                    if (dictionary.ContainsKey(word))
                    {
                        // Since tuples are immutable, we need to create a new tuple
                        dictionary[word] = new Tuple<int, int>(dictionary[word].Item1 + 1, 0);
                    }
                    else
                    {
                        dictionary.Add(word, new Tuple<int, int>(1, 0));
                    }
                }

                // To avoid InvalidOperationException
                String[] keys = dictionary.Keys.ToArray();

                // Levenshten distance for each title word and the given pattern
                foreach(String item in keys)
                {
                    Tuple<int, int> value = dictionary[item];
                    dictionary[item] = new Tuple<int, int>(value.Item1, LevenshteinDistance(item, pattern) / item.Length);
                }

                matchingChars = dictionary.Select(entry => entry.Value.Item2).Sum();

                window.setMatchingPriority(matchingChars);
                window.setMatchingGroups(matchingGroups);
            }

            windows.Sort();
            return windows;
        }

    }
}
