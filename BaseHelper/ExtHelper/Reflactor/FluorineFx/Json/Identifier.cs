namespace FluorineFx.Json
{
    using System;

    public class Identifier
    {
        private string _name;

        public Identifier(string name)
        {
            this._name = name;
        }

        public bool Equals(Identifier function)
        {
            return (this._name == function.Name);
        }

        public override bool Equals(object obj)
        {
            Identifier function = obj as Identifier;
            return this.Equals(function);
        }

        public static bool Equals(Identifier a, Identifier b)
        {
            return ((a == b) || (((a != null) && (b != null)) && a.Equals(b)));
        }

        public override int GetHashCode()
        {
            return this._name.GetHashCode();
        }

        private static bool IsAsciiLetter(char c)
        {
            return (((c >= 'A') && (c <= 'Z')) || ((c >= 'a') && (c <= 'z')));
        }

        public static bool operator ==(Identifier a, Identifier b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Identifier a, Identifier b)
        {
            return !Equals(a, b);
        }

        public override string ToString()
        {
            return this._name;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

