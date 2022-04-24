using System;
using System.Collections.Generic;

namespace FileTask
{
    public static class InterLeaveWordsService 
    {
        /// <summary>
        /// This is a utility method to get the interleave words.
        /// </summary>
        /// <param name="file1Words"></param>
        /// <param name="file2Words"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<string> InterLeaveWords(List<string> file1Words, List<string> file2Words)
        {
            if (file1Words == null) { throw new ArgumentNullException(nameof(file1Words)); }
            if (file2Words == null) { throw new ArgumentNullException(nameof(file2Words)); }

            using (var file1 = file1Words.GetEnumerator())
            {
                using (var file2 = file2Words.GetEnumerator())
                {
                    var file1End = true;
                    var file2End = true;
                    do
                    {
                        if (file1End = file1.MoveNext())
                        {
                            yield return file1.Current;
                        }

                        if (file2End = file2.MoveNext())
                        {
                            yield return file2.Current;
                        }

                    }
                    while (file1End || file2End);
                }
            }
        }
    }
}
