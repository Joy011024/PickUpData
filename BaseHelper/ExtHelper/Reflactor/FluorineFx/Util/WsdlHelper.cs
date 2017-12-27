namespace FluorineFx.Util
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using log4net;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Web.Services.Description;
    using System.Web.Services.Discovery;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class WsdlHelper
    {
        private static object _objLock = new object();
        private static Hashtable _webserviceTypeCache = new Hashtable();

        private WsdlHelper()
        {
        }

        private static Assembly GenerateAssembly(string asmxFile)
        {
            StringReader input = new StringReader(WsdlFromUrl(GetApplicationPath() + "/" + asmxFile + "?wsdl"));
            XmlTextReader reader = new XmlTextReader(input);
            ServiceDescription description = ServiceDescription.Read(reader);
            reader.Close();
            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            CodeNamespace namespace2 = new CodeNamespace("FluorineFx");
            codeCompileUnit.Namespaces.Add(namespace2);
            CodeNamespace namespace3 = new CodeNamespace(FluorineConfiguration.Instance.WsdlProxyNamespace);
            codeCompileUnit.Namespaces.Add(namespace3);
            DiscoveryClientDocumentCollection documents = new DiscoveryClientDocumentCollection();
            documents.Add(asmxFile, description);
            WebReference webReference = new WebReference(documents, namespace3) {
                ProtocolName = "Soap12"
            };
            WebReferenceCollection webReferences = new WebReferenceCollection();
            webReferences.Add(webReference);
            WebReferenceOptions options = new WebReferenceOptions {
                Style = ServiceDescriptionImportStyle.Client,
                CodeGenerationOptions = CodeGenerationOptions.None
            };
            options.SchemaImporterExtensions.Add(typeof(DataTableSchemaImporterExtension).AssemblyQualifiedName);
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ServiceDescriptionImporter.GenerateWebReferences(webReferences, codeProvider, codeCompileUnit, options);
            if (!FluorineConfiguration.Instance.WsdlGenerateProxyClasses)
            {
                foreach (CodeNamespace namespace4 in codeCompileUnit.Namespaces)
                {
                    ArrayList list = new ArrayList();
                    foreach (CodeTypeDeclaration declaration in namespace4.Types)
                    {
                        bool flag = false;
                        foreach (CodeTypeReference reference2 in declaration.BaseTypes)
                        {
                            if (reference2.BaseType == "System.Web.Services.Protocols.SoapHttpClientProtocol")
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            list.Add(declaration);
                        }
                        else
                        {
                            CodeAttributeDeclaration declaration2 = new CodeAttributeDeclaration(typeof(RemotingServiceAttribute).FullName);
                            declaration.CustomAttributes.Add(declaration2);
                            foreach (CodeTypeMember member in declaration.Members)
                            {
                                CodeConstructor constructor = member as CodeConstructor;
                                if (constructor != null)
                                {
                                    CodeSnippetStatement statement = new CodeSnippetStatement("this.CookieContainer = new System.Net.CookieContainer(); //Session Cookie");
                                    constructor.Statements.Add(statement);
                                }
                            }
                        }
                    }
                    foreach (CodeTypeDeclaration declaration in list)
                    {
                        namespace3.Types.Remove(declaration);
                    }
                }
                if (FluorineConfiguration.Instance.ImportNamespaces != null)
                {
                    for (int i = 0; i < FluorineConfiguration.Instance.ImportNamespaces.Count; i++)
                    {
                        ImportNamespace namespace5 = FluorineConfiguration.Instance.ImportNamespaces[i];
                        namespace3.Imports.Add(new CodeNamespaceImport(namespace5.Namespace));
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, writer, null);
            string str2 = sb.ToString();
            writer.Close();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("System.Web.Services.dll");
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GlobalAssemblyCache)
                {
                    if (assembly.GetName().Name.StartsWith("System") && !parameters.ReferencedAssemblies.Contains(assembly.GetName().Name + ".dll"))
                    {
                        parameters.ReferencedAssemblies.Add(assembly.Location);
                    }
                }
                else if (!assembly.GetName().Name.StartsWith("mscorlib"))
                {
                    try
                    {
                        if ((assembly.Location != null) && (assembly.Location != string.Empty))
                        {
                            parameters.ReferencedAssemblies.Add(assembly.Location);
                        }
                    }
                    catch (NotSupportedException)
                    {
                    }
                }
            }
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = false;
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, new string[] { str2 });
            if (results.Errors.Count > 0)
            {
                ILog logger = null;
                try
                {
                    logger = LogManager.GetLogger(typeof(WsdlHelper));
                }
                catch
                {
                }
                StringBuilder builder2 = new StringBuilder();
                builder2.Append(string.Format("Build failed: {0} errors", results.Errors.Count));
                if (logger.get_IsErrorEnabled())
                {
                    logger.Error(__Res.GetString("Wsdl_ProxyGenFail"));
                }
                foreach (CompilerError error in results.Errors)
                {
                    logger.Error(__Res.GetString("Compiler_Error", new object[] { error.Line, error.Column, error.ErrorText }));
                    builder2.Append("\n");
                    builder2.Append(error.ErrorText);
                }
                throw new FluorineException(builder2.ToString());
            }
            return results.CompiledAssembly;
        }

        private static string GetApplicationPath()
        {
            string str = "";
            if (HttpContext.Current.Request.Url != null)
            {
                str = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.ApplicationPath.ToLower(), (int) (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.Url.Authority.ToLower()) + HttpContext.Current.Request.Url.Authority.Length)) + HttpContext.Current.Request.ApplicationPath.Length);
            }
            return str;
        }

        public static Assembly GetAssemblyFromAsmx(string asmxFile)
        {
            lock (_objLock)
            {
                Type type = null;
                if (!_webserviceTypeCache.Contains(asmxFile))
                {
                    Type[] types = GenerateAssembly(asmxFile).GetTypes();
                    if (types.Length > 0)
                    {
                        type = types[0];
                        _webserviceTypeCache[asmxFile] = type;
                        ObjectFactory.AddTypeToCache(type);
                    }
                }
                else
                {
                    type = _webserviceTypeCache[asmxFile] as Type;
                }
                if (type != null)
                {
                    return type.Assembly;
                }
            }
            return null;
        }

        private static string WsdlFromUrl(string url)
        {
            Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            StreamReader reader = new StreamReader(responseStream, encoding);
            return reader.ReadToEnd();
        }
    }
}

