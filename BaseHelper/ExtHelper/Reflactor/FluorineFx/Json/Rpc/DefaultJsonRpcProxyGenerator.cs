﻿namespace FluorineFx.Json.Rpc
{
    using FluorineFx.Json;
    using FluorineFx.Json.Services;
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Web;

    internal class DefaultJsonRpcProxyGenerator : IJsonRpcProxyGenerator
    {
        public void WriteProxy(ServiceClass service, IndentedTextWriter writer, HttpRequest request)
        {
            ValidationUtils.ArgumentNotNull(writer, "writer");
            writer.WriteLine("// This JavaScript was automatically generated by");
            writer.Write("// ");
            writer.WriteLine(base.GetType().AssemblyQualifiedName);
            writer.Write("// on ");
            DateTime now = DateTime.Now;
            TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
            writer.Write(now.ToLongDateString());
            writer.Write(" at ");
            writer.Write(now.ToLongTimeString());
            writer.Write(" (");
            writer.Write(currentTimeZone.IsDaylightSavingTime(now) ? currentTimeZone.DaylightName : currentTimeZone.StandardName);
            writer.WriteLine(")");
            writer.WriteLine();
            Uri url = request.Url;
            Debug.Assert(service != null);
            Debug.Assert(url != null);
            Debug.Assert(!url.IsFile);
            Debug.Assert(writer != null);
            writer.WriteLine("// Default Proxy");
            writer.WriteLine();
            writer.Write("function ");
            writer.Write(service.Name);
            writer.WriteLine("(url)");
            writer.WriteLine("{");
            writer.Indent++;
            ICollection methods = service.GetMethods();
            string[] values = new string[methods.Count];
            int num = 0;
            foreach (Method method in methods)
            {
                values[num++] = method.Name;
                writer.Write("this[\"");
                writer.Write(method.Name);
                writer.Write("\"] = function(");
                Parameter[] parameters = method.GetParameters();
                foreach (Parameter parameter in parameters)
                {
                    writer.Write(parameter.Name);
                    writer.Write(", ");
                }
                writer.WriteLine("callback)");
                writer.WriteLine("{");
                writer.Indent++;
                writer.Write("return call(\"");
                writer.Write(method.Name);
                writer.Write("\", [");
                foreach (Parameter parameter in parameters)
                {
                    if (parameter.Position > 0)
                    {
                        writer.Write(',');
                    }
                    writer.Write(' ');
                    writer.Write(parameter.Name);
                }
                writer.WriteLine(" ], callback);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
            }
            writer.Write("var url = typeof(url) === 'string' ? url : '");
            writer.Write(url);
            writer.WriteLine("';");
            writer.WriteLine("var self = this;\r\n    var nextId = 0;\r\n    var credentials;\r\n\r\n    this['setCredentials'] = function(userid, password)\r\n    {\r\n        if( userid == null && password == null )\r\n            this.credentials = null;\r\n        else\r\n            this.credentials = Base64.encode(userid + ':' + password);\r\n    }\r\n\r\n    function call(method, params, callback)\r\n    {\r\n        var request = { id : nextId++, method : method, params : params, credentials:self.credentials };\r\n        return callback == null ? \r\n            callSync(method, request) : callAsync(method, request, callback);\r\n    }\r\n\r\n    function callSync(method, request)\r\n    {\r\n        var http = newHTTP();\r\n        http.open('POST', url, false, self.httpUserName, self.httpPassword);\r\n        setupHeaders(http, method);\r\n        http.send(JSON.stringify(request));\r\n        if (http.status != 200)\r\n            throw { message : http.status + ' ' + http.statusText, toString : function() { return message; } };\r\n        var response = JSON.parse(http.responseText);\r\n        if (response.error != null) throw response.error;\r\n        return response.result;\r\n    }\r\n\r\n    function callAsync(method, request, callback)\r\n    {\r\n        var http = newHTTP();\r\n        http.open('POST', url, true, self.httpUserName, self.httpPassword);\r\n        setupHeaders(http, method);\r\n        http.onreadystatechange = function() { http_onreadystatechange(http, callback); }\r\n        http.send(JSON.stringify(request));\r\n        return request.id;\r\n    }\r\n\r\n    function setupHeaders(http, method)\r\n    {\r\n        http.setRequestHeader('Content-Type', 'text/plain; charset=utf-8');\r\n        http.setRequestHeader('X-JSON-RPC', method);\r\n    }\r\n\r\n    function http_onreadystatechange(sender, callback)\r\n    {\r\n        if (sender.readyState == /* complete */ 4)\r\n        {\r\n            var response = sender.status == 200 ? \r\n                JSON.parse(sender.responseText) : {};\r\n            \r\n            response.xmlHTTP = sender;\r\n                \r\n            callback(response);\r\n        }\r\n    }\r\n\r\n    function newHTTP()\r\n    {\r\n        if (typeof(window) != 'undefined' && window.XMLHttpRequest)\r\n            return new XMLHttpRequest(); /* IE7, Safari 1.2, Mozilla 1.0/Firefox, and Netscape 7 */\r\n        else\r\n            return new ActiveXObject('Microsoft.XMLHTTP'); /* WSH and IE 5 to IE 6 */\r\n    }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.Write(service.Name);
            writer.Write(".rpcMethods = ");
            new JsonWriter(writer).WriteStringArray(values);
            writer.WriteLine(";");
        }
    }
}

