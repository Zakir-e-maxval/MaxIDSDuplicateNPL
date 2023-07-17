using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuplicateNPL_Common
{
    public static class UtilityConstants
    {

        /// <summary>
        /// Represents Regex patterns
        /// </summary>
        public struct RegexPatterns
        {
            public const string ExactlyFourDigits = @"^\d{4}$";
            public const string TwoDigits = @"^\d{2}$";
            public const string OnlyAlphabetDigits = @"[^\w\d/,.]";
            public const string AlphabetDigitsWithHyphen = @"[^\w\d-]";
            public const string DigitsOnly = @"[^\d+]";
            public const string DigitsWithHyphen = @"^\d+-\d+$";
            public const string AlphaNumericRegex = @"^[a-zA-Z][a-zA-Z0-9]*$";
            public const string DoubleQuotes = "\"([^\"]*)\"";
            public const string MultipleSpace = @"\s{2,}";
            public const string Newlines = @"\n{1,}";
            public const string DOIRegex = @"^[a-zA-Z0-9.//]*$";
        }

        /// <summary>
        /// Prepresents search keys
        /// </summary>
        public struct SearchKeys
        {
            public const string EtAl = "et al";
            public const string Pages = "pages";
            public const string Pgs = "pgs";
            public const string Page = "page";
        }

        /// <summary>
        /// Represents search collections
        /// </summary>
        public struct Collections
        {
            /**
             * Keep search key list to execute algorithm perfectly
             * Keep all special character search keys first to avoid misses during search
             * Keep non special character strings as last search keys
             * **/
            public static readonly IList<string> Months = new ReadOnlyCollection<string>
                            (new List<string>
                            { "january", "february", "march", "april", "may", "june",
                                "july", "august", "september", "october", "november", "december"
                                , "jan", "feb", "mar", "apr", "jun", "jul", "aug", "sep", "oct", "nov", "dec"
                            });
            public static readonly IList<string> ApplicationNoSearches = new ReadOnlyCollection<string>
                            (new List<string>
                            { "application no.", "application no", "appl. no.", "appl. no", "application. no.", "application. no"
                                , "appl no.", "appl no", "app no.", "app. no.", "app no"
                                , "serial. no.", "serial no.", "serial. no", "serial no","patentability for"
                                , "publication no.", "publication no", "publication number.", "publication number"
                            });
            public static readonly IList<string> MailRoomDateSearches = new ReadOnlyCollection<string>
                            (new List<string>
                            { "mailed on", "mailed. ", "mailed ", "mailed"});
            public static readonly IList<string> FilingDateSearches = new ReadOnlyCollection<string>
                            (new List<string>
                            { "filed on.", "filed. on.", "filed. on", "filed on", "filed. ", "filed "
                            });
            public static readonly IList<string> PageSearchKeys = new ReadOnlyCollection<string>
                            (new List<string>
                            { "Pages,","Pages.", "Pages", "Page.", "Page", "Pgs.", "Pg",
                              "pages,","pages.", "pages", "page.", "page", "pgs.", "pg",
                            });
            /// <summary>
            /// Journal searches
            /// </summary>
            public static readonly IList<SearchKey> JournalSearcheKeys = new ReadOnlyCollection<SearchKey>(
                new List<SearchKey>
                {
                    new SearchKey(){Key="journal of"},
                    new SearchKey(){Key="j. of"}
                });
            /// <summary>
            /// Represents title search keys
            /// </summary>
            public static readonly IList<SearchKey> TitleSearchKeys = new ReadOnlyCollection<SearchKey>(
                new List<SearchKey>
                {
                    new SearchKey(){Key = "final office action", ReplaceValue="FOA"},
                    new SearchKey(){Key = "office action", ReplaceValue="OA"},
                    new SearchKey(){Key = "european search report", ReplaceValue="ESR"},
                    new SearchKey(){Key="partial international search report", ReplaceValue = "PISR"}
                });
            /// <summary>
            /// Represents excluded title search words
            /// </summary>
            public static readonly IList<string> ExcludedTitleSearchWords = new ReadOnlyCollection<string>
                            (new List<string>
                            { "on","et", "al",
                                "the.", "the,", "the",
                                "to.", "to,", "to",
                                "in.", "in,", "in",
                                "of.", "of,", "of",
                                "by,", "by.", "by",
                                "for.", "for,", "for",
                                "with.","with,", "with" ,
                                "and.", "and,", "and",
                                "as.", "as,", "be",
                                "before", "after",
                                "use", "used",
                                "present","concept", "concepts", "relative"
                                , "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"
                            });

            /// <summary>
            /// Month replace values based on month search keys
            /// Ex. Replaces Jul. to Jul - This conversion required to change the user opted date format
            /// </summary>
            public static readonly IList<SearchKey> MonthSearchKeys = new ReadOnlyCollection<SearchKey>(
                new List<SearchKey>
                {
                    new SearchKey(){Key = "Jan.", ReplaceValue="Jan"},
                    new SearchKey(){Key = "Feb.", ReplaceValue="Feb"},
                    new SearchKey(){Key = "Mar.", ReplaceValue="Mar"},
                    new SearchKey(){Key = "Apr.", ReplaceValue="Apr"},
                    new SearchKey(){Key = "May.", ReplaceValue="May"},
                    new SearchKey(){Key = "Jun.", ReplaceValue="Jun"},
                    new SearchKey(){Key = "Jul.", ReplaceValue="Jul"},
                    new SearchKey(){Key = "Aug.", ReplaceValue="Aug"},
                    new SearchKey(){Key = "Sep.", ReplaceValue="Sep"},
                    new SearchKey(){Key = "Oct.", ReplaceValue="Oct"},
                    new SearchKey(){Key = "Nov.", ReplaceValue="Nov"},
                    new SearchKey(){Key = "Dec.", ReplaceValue="Dec"},
                    new SearchKey(){Key = "Jan,", ReplaceValue="Jan"},
                    new SearchKey(){Key = "Feb,", ReplaceValue="Feb"},
                    new SearchKey(){Key = "Mar,", ReplaceValue="Mar"},
                    new SearchKey(){Key = "Apr,", ReplaceValue="Apr"},
                    new SearchKey(){Key = "May,", ReplaceValue="May"},
                    new SearchKey(){Key = "Jun,", ReplaceValue="Jun"},
                    new SearchKey(){Key = "Jul,", ReplaceValue="Jul"},
                    new SearchKey(){Key = "Aug,", ReplaceValue="Aug"},
                    new SearchKey(){Key = "Sep,", ReplaceValue="Sep"},
                    new SearchKey(){Key = "Oct,", ReplaceValue="Oct"},
                    new SearchKey(){Key = "Nov,", ReplaceValue="Nov"},
                    new SearchKey(){Key = "Dec,", ReplaceValue="Dec"},

                });
        }

    }

    /// <summary>
    /// Represents for search keys
    /// </summary>
    public class SearchKey
    {
        public string Key { get; set; }
        public string ReplaceValue { get; set; }
    }
}
