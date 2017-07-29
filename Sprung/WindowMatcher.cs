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

        public List<Window> match(String pattern, List<Window> windows)
        {
            if (pattern.Length == 0) return windows;
            foreach (Window window in windows)
            {
                String title = window.getTitle().ToLower();
                pattern = pattern.ToLower();

                int count, value = 0;

                count = Regex.Matches(Regex.Escape(title), pattern).Count;
                value = pattern.Length * count;

                window.setMatchingPriority(value);
            }

            windows.Sort();
            return windows;
        }

    }
}
