namespace FluorineFx.Diagnostic
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    internal class DescribeService
    {
        private AMFBody _amfBody;

        public DescribeService(AMFBody amfBody)
        {
            this._amfBody = amfBody;
        }

        public Hashtable GetDescription()
        {
            Hashtable hashtable = new Hashtable();
            if (this._amfBody != null)
            {
                hashtable["version"] = "1.0";
                hashtable["address"] = this._amfBody.TypeName;
                Type type = TypeHelper.Locate(this._amfBody.TypeName);
                hashtable["description"] = "Service description not found.";
                ArrayList list = new ArrayList();
                if (type != null)
                {
                    object[] customAttributes = type.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        DescriptionAttribute attribute = customAttributes[0] as DescriptionAttribute;
                        hashtable["description"] = attribute.Description;
                    }
                    foreach (MethodInfo info in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (!TypeHelper.SkipMethod(info))
                        {
                            string description = TypeHelper.GetDescription(info);
                            Hashtable hashtable2 = new Hashtable();
                            hashtable2["name"] = info.Name;
                            hashtable2["version"] = "1.0";
                            hashtable2["description"] = description;
                            if (info.ReturnType.Name == "Void")
                            {
                                hashtable2["returns"] = "undefined";
                            }
                            else
                            {
                                hashtable2["returns"] = info.ReturnType.Name;
                            }
                            ArrayList list2 = new ArrayList();
                            hashtable2["arguments"] = list2;
                            if ((info.GetParameters() != null) && (info.GetParameters().Length > 0))
                            {
                                foreach (ParameterInfo info2 in info.GetParameters())
                                {
                                    Hashtable hashtable3 = new Hashtable();
                                    hashtable3["name"] = info2.Name;
                                    hashtable3["required"] = true;
                                    if (info2.ParameterType.IsArray)
                                    {
                                        hashtable3["type"] = "Array";
                                    }
                                    else
                                    {
                                        hashtable3["type"] = info2.ParameterType.Name;
                                    }
                                    list2.Add(hashtable3);
                                }
                            }
                            list.Add(hashtable2);
                        }
                    }
                }
                hashtable["functions"] = list;
            }
            return hashtable;
        }
    }
}

