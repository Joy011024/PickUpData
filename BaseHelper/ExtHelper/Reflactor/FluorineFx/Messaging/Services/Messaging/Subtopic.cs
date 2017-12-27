namespace FluorineFx.Messaging.Services.Messaging
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using System;
    using System.Text.RegularExpressions;

    public class Subtopic
    {
        private static Regex _regex = new Regex(@"^([\w][\w\-]*)(\.(([\w][\w\-]*)|\*))*$|^\*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private string _subtopic;
        private string[] _subtopicItems;
        private const string SubtopicCheckExpression = @"^([\w][\w\-]*)(\.(([\w][\w\-]*)|\*))*$|^\*$";
        public const string SubtopicSeparator = ".";
        public const string SubtopicWildcard = "*";

        public Subtopic(string subtopic) : this(subtopic, ".")
        {
        }

        private Subtopic(string subtopic, string separator)
        {
            if ((subtopic == null) || (subtopic.Length == 0))
            {
                throw new FluorineException(__Res.GetString("Subtopic_Invalid", new object[] { string.Empty }));
            }
            if (!_regex.IsMatch(subtopic))
            {
                throw new FluorineException(__Res.GetString("Subtopic_Invalid", new object[] { subtopic }));
            }
            this._subtopic = subtopic;
            this._subtopicItems = subtopic.Split(new char[] { separator[0] });
        }

        public bool Matches(Subtopic subtopic)
        {
            if (this.Value != subtopic.Value)
            {
                string[] subtopicItems = this.SubtopicItems;
                string[] strArray2 = subtopic.SubtopicItems;
                for (int i = 0; i < subtopicItems.Length; i++)
                {
                    string str = subtopicItems[i];
                    if (str != "*")
                    {
                        if (i >= strArray2.Length)
                        {
                            return true;
                        }
                        string str2 = strArray2[i];
                        if (str != str2)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool IsHierarchical
        {
            get
            {
                return ((this._subtopicItems != null) && (this._subtopicItems.Length > 1));
            }
        }

        public string Separator
        {
            get
            {
                return ".";
            }
        }

        internal string[] SubtopicItems
        {
            get
            {
                return this._subtopicItems;
            }
        }

        public string Value
        {
            get
            {
                return this._subtopic;
            }
        }
    }
}

