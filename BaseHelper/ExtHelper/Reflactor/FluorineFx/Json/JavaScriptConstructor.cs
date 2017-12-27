namespace FluorineFx.Json
{
    using System;
    using System.Text;

    public class JavaScriptConstructor
    {
        private string _name;
        private JavaScriptParameters _parameters;

        public JavaScriptConstructor(string name, JavaScriptParameters parameters)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException("Constructor name cannot be empty.", "name");
            }
            this._name = name;
            this._parameters = (parameters != null) ? parameters : JavaScriptParameters.Empty;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("new ");
            builder.Append(this._name);
            builder.Append("(");
            if (this._parameters != null)
            {
                for (int i = 0; i < this._parameters.Count; i++)
                {
                    builder.Append(this._parameters[i]);
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public JavaScriptParameters Parameters
        {
            get
            {
                return this._parameters;
            }
        }
    }
}

