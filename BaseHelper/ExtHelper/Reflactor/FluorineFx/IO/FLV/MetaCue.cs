namespace FluorineFx.IO.FLV
{
    using System;
    using System.Collections;

    public class MetaCue : Hashtable, IComparable
    {
        public const string EVENT = "event";
        public const string NAVIGATION = "navigation";

        public int CompareTo(object obj)
        {
            MetaCue cue = obj as MetaCue;
            double time = cue.Time;
            double num2 = this.Time;
            if (time > num2)
            {
                return -1;
            }
            if (time < num2)
            {
                return 1;
            }
            return 0;
        }

        public string Name
        {
            get
            {
                return (this["name"] as string);
            }
            set
            {
                this["name"] = value;
            }
        }

        public double Time
        {
            get
            {
                return (double) this["time"];
            }
            set
            {
                this["time"] = value;
            }
        }

        public string Type
        {
            get
            {
                return (this["type"] as string);
            }
            set
            {
                this["type"] = value;
            }
        }
    }
}

