namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;

    public sealed class LoginCommandCollection : CollectionBase<LoginCommandSettings>
    {
        public string GetLoginCommand(string server)
        {
            foreach (LoginCommandSettings settings in this)
            {
                if (settings.Server == server)
                {
                    return settings.Type;
                }
            }
            return null;
        }
    }
}

