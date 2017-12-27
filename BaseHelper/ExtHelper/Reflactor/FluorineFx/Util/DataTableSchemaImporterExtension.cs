namespace FluorineFx.Util
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Data;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using System.Xml.Serialization.Advanced;

    internal class DataTableSchemaImporterExtension : SchemaImporterExtension
    {
        private Hashtable importedTypes = new Hashtable();

        public override string ImportSchemaType(XmlSchemaType type, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
        {
            if (type != null)
            {
                if (this.importedTypes[type] != null)
                {
                    mainNamespace.Imports.Add(new CodeNamespaceImport(typeof(DataSet).Namespace));
                    compileUnit.ReferencedAssemblies.Add("System.Data.dll");
                    return (string) this.importedTypes[type];
                }
                if (!(context is XmlSchemaElement))
                {
                    return null;
                }
                if (type is XmlSchemaComplexType)
                {
                    XmlSchemaComplexType type2 = (XmlSchemaComplexType) type;
                    if (type2.Particle is XmlSchemaSequence)
                    {
                        XmlSchemaObjectCollection items = ((XmlSchemaSequence) type2.Particle).Items;
                        if (((items.Count == 2) && (items[0] is XmlSchemaAny)) && (items[1] is XmlSchemaAny))
                        {
                            XmlSchemaAny any = (XmlSchemaAny) items[0];
                            XmlSchemaAny any2 = (XmlSchemaAny) items[1];
                            if ((any.Namespace == "http://www.w3.org/2001/XMLSchema") && (any2.Namespace == "urn:schemas-microsoft-com:xml-diffgram-v1"))
                            {
                                string fullName = typeof(DataTable).FullName;
                                this.importedTypes.Add(type, fullName);
                                mainNamespace.Imports.Add(new CodeNamespaceImport(typeof(DataTable).Namespace));
                                compileUnit.ReferencedAssemblies.Add("System.Data.dll");
                                return fullName;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public override string ImportSchemaType(string name, string schemaNamespace, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
        {
            IList list = schemas.GetSchemas(schemaNamespace);
            if (list.Count != 1)
            {
                return null;
            }
            XmlSchema schema = list[0] as XmlSchema;
            if (schema == null)
            {
                return null;
            }
            XmlSchemaType type = (XmlSchemaType) schema.SchemaTypes[new XmlQualifiedName(name, schemaNamespace)];
            return this.ImportSchemaType(type, context, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
        }
    }
}

