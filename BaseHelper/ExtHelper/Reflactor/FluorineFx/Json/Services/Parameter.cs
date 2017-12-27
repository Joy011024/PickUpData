namespace FluorineFx.Json.Services
{
    using System;
    using System.Reflection;

    public sealed class Parameter
    {
        private bool _isParamArray;
        private string _name;
        private Type _parameterType;
        private int _position;

        internal Parameter()
        {
        }

        public static Parameter FromParameterInfo(ParameterInfo parameterInfo)
        {
            return new Parameter { _name = parameterInfo.Name, _position = parameterInfo.Position, _parameterType = parameterInfo.ParameterType, _isParamArray = parameterInfo.IsDefined(typeof(ParamArrayAttribute), true) };
        }

        public bool IsParamArray
        {
            get
            {
                return this._isParamArray;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public Type ParameterType
        {
            get
            {
                return this._parameterType;
            }
        }

        public int Position
        {
            get
            {
                return this._position;
            }
        }
    }
}

