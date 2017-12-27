namespace FluorineFx.Messaging.Config
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class SecuritySettings : Hashtable
    {
        private DestinationSettings _destinationSettings;
        private LoginCommandCollection _loginCommands;
        private object _objLock;
        private FluorineFx.Messaging.Config.SecurityConstraintRef _securityConstraintRef;
        private Hashtable _securityConstraints;

        private SecuritySettings()
        {
            this._objLock = new object();
        }

        internal SecuritySettings(DestinationSettings destinationSettings)
        {
            this._objLock = new object();
            this._destinationSettings = destinationSettings;
            this._securityConstraints = new Hashtable();
            this._loginCommands = new LoginCommandCollection();
        }

        internal SecuritySettings(DestinationSettings destinationSettings, XmlNode securityNode)
        {
            this._objLock = new object();
            this._destinationSettings = destinationSettings;
            this._securityConstraints = new Hashtable();
            this._loginCommands = new LoginCommandCollection();
            foreach (XmlNode node in securityNode.SelectNodes("*"))
            {
                if (node.Name == "security-constraint")
                {
                    if (node.Attributes["ref"] != null)
                    {
                        this._securityConstraintRef = new FluorineFx.Messaging.Config.SecurityConstraintRef(node.Attributes["ref"].Value);
                        continue;
                    }
                    string id = null;
                    if (node.Attributes["id"] != null)
                    {
                        id = node.Attributes["id"].Value;
                    }
                    else
                    {
                        id = Guid.NewGuid().ToString("N");
                    }
                    string authMethod = "Custom";
                    string[] roles = null;
                    foreach (XmlNode node2 in node.SelectNodes("*"))
                    {
                        if (node2.Name == "auth-method")
                        {
                            authMethod = node2.InnerXml;
                        }
                        if (node2.Name == "roles")
                        {
                            ArrayList list = new ArrayList();
                            foreach (XmlNode node3 in node2.SelectNodes("*"))
                            {
                                if (node3.Name == "role")
                                {
                                    list.Add(node3.InnerXml);
                                }
                            }
                            roles = list.ToArray(typeof(string)) as string[];
                        }
                    }
                    this.CreateSecurityConstraint(id, authMethod, roles);
                }
                if (node.Name == "login-command")
                {
                    LoginCommandSettings item = new LoginCommandSettings {
                        Server = node.Attributes["server"].Value,
                        Type = node.Attributes["class"].Value
                    };
                    this._loginCommands.Add(item);
                }
            }
        }

        internal SecurityConstraint CreateSecurityConstraint(string id, string authMethod, string[] roles)
        {
            lock (this._objLock)
            {
                if (!this._securityConstraints.ContainsKey(id))
                {
                    SecurityConstraint constraint = new SecurityConstraint(id, authMethod, roles);
                    this._securityConstraints[id] = constraint;
                    return constraint;
                }
                return (this._securityConstraints[id] as SecurityConstraint);
            }
        }

        public string[] GetRoles()
        {
            lock (this._objLock)
            {
                SecurityConstraint constraint;
                if ((this.SecurityConstraintRef != null) && (this._destinationSettings != null))
                {
                    if ((this._destinationSettings.ServiceSettings.ServiceConfigSettings.SecuritySettings == null) || (this._destinationSettings.ServiceSettings.ServiceConfigSettings.SecuritySettings.SecurityConstraints == null))
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_ConstraintRefNotFound"));
                    }
                    constraint = this._destinationSettings.ServiceSettings.ServiceConfigSettings.SecuritySettings.SecurityConstraints[this.SecurityConstraintRef.Reference] as SecurityConstraint;
                    if (constraint == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_ConstraintRefNotFound", new object[] { this.SecurityConstraintRef.Reference }));
                    }
                    return constraint.Roles;
                }
                ArrayList list = new ArrayList();
                foreach (DictionaryEntry entry in this.SecurityConstraints)
                {
                    constraint = entry.Value as SecurityConstraint;
                    list.AddRange(constraint.Roles);
                }
                return (list.ToArray(typeof(string)) as string[]);
            }
        }

        public LoginCommandCollection LoginCommands
        {
            get
            {
                return this._loginCommands;
            }
        }

        public FluorineFx.Messaging.Config.SecurityConstraintRef SecurityConstraintRef
        {
            get
            {
                return this._securityConstraintRef;
            }
        }

        public Hashtable SecurityConstraints
        {
            get
            {
                return this._securityConstraints;
            }
        }
    }
}

