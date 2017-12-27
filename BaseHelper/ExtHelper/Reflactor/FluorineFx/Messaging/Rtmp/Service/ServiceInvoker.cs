namespace FluorineFx.Messaging.Rtmp.Service
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Exceptions;
    using FluorineFx.Invocation;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using log4net;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Security;

    internal class ServiceInvoker : IServiceInvoker
    {
        private ILog _log;
        private ArrayList _serviceResolvers = new ArrayList();
        public static string SERVICE_NAME = "serviceInvoker";

        public ServiceInvoker()
        {
            try
            {
                this._log = LogManager.GetLogger(typeof(ServiceInvoker));
            }
            catch
            {
            }
        }

        private object GetServiceHandler(IScope scope, string serviceName)
        {
            object handler = scope.Handler;
            if ((serviceName == null) || (serviceName == string.Empty))
            {
                return handler;
            }
            if (this._serviceResolvers != null)
            {
                foreach (IServiceResolver resolver in this._serviceResolvers)
                {
                    handler = resolver.ResolveService(scope, serviceName);
                    if (handler != null)
                    {
                        return handler;
                    }
                }
            }
            return null;
        }

        public bool Invoke(IServiceCall call, IScope scope)
        {
            string serviceName = call.ServiceName;
            this._log.Debug("Service name " + serviceName);
            object serviceHandler = this.GetServiceHandler(scope, serviceName);
            if (serviceHandler == null)
            {
                call.Exception = new ServiceNotFoundException(serviceName);
                call.Status = 0x10;
                this._log.Warn("Service not found: " + serviceName);
                return false;
            }
            this._log.Debug("Service found: " + serviceName);
            return this.Invoke(call, serviceHandler);
        }

        public bool Invoke(IServiceCall call, object service)
        {
            IConnection connection = FluorineContext.Current.Connection;
            string serviceMethodName = call.ServiceMethodName;
            object[] arguments = call.Arguments;
            object[] destinationArray = null;
            if (arguments != null)
            {
                destinationArray = new object[arguments.Length + 1];
                destinationArray[0] = connection;
                Array.Copy(arguments, 0, destinationArray, 1, arguments.Length);
            }
            else
            {
                destinationArray = new object[] { connection };
            }
            object[] objArray3 = null;
            MethodInfo methodInfo = MethodHandler.GetMethod(service.GetType(), serviceMethodName, destinationArray, true, false, false);
            if (methodInfo == null)
            {
                methodInfo = MethodHandler.GetMethod(service.GetType(), serviceMethodName, arguments, true, false, false);
                if (methodInfo == null)
                {
                    methodInfo = MethodHandler.GetMethod(service.GetType(), serviceMethodName, destinationArray, false, false, false);
                    if (methodInfo == null)
                    {
                        methodInfo = MethodHandler.GetMethod(service.GetType(), serviceMethodName, arguments, false, false, false);
                        if (methodInfo == null)
                        {
                            string message = __Res.GetString("Invocation_NoSuitableMethod", new object[] { serviceMethodName });
                            call.Status = 0x11;
                            call.Exception = new FluorineException(message);
                            this._log.Error(message, call.Exception);
                            return false;
                        }
                        objArray3 = arguments;
                    }
                    else
                    {
                        objArray3 = destinationArray;
                    }
                }
                else
                {
                    objArray3 = arguments;
                }
            }
            else
            {
                objArray3 = destinationArray;
            }
            try
            {
                this._log.Debug("Invoking method: " + methodInfo.Name);
                ParameterInfo[] parameters = methodInfo.GetParameters();
                object[] array = new object[parameters.Length];
                objArray3.CopyTo(array, 0);
                TypeHelper.NarrowValues(array, parameters);
                object obj2 = null;
                if (methodInfo.ReturnType == typeof(void))
                {
                    InvocationHandler handler = new InvocationHandler(methodInfo);
                    handler.Invoke(service, array);
                    call.Status = 4;
                }
                else
                {
                    obj2 = new InvocationHandler(methodInfo).Invoke(service, array);
                    call.Status = (obj2 == null) ? ((byte) 3) : ((byte) 2);
                }
                if (call is IPendingServiceCall)
                {
                    (call as IPendingServiceCall).Result = obj2;
                }
            }
            catch (SecurityException exception)
            {
                call.Exception = exception;
                call.Status = 0x12;
                this._log.Error("Error executing call: " + call);
                this._log.Error("Service invocation error", exception);
                return false;
            }
            catch (TargetInvocationException exception2)
            {
                call.Exception = exception2;
                call.Status = 0x13;
                this._log.Error("Error executing call: " + call);
                this._log.Error("Service invocation error", exception2);
                return false;
            }
            catch (Exception exception3)
            {
                call.Exception = exception3;
                call.Status = 20;
                this._log.Error("Error executing call: " + call);
                this._log.Error("Service invocation error", exception3);
                return false;
            }
            return true;
        }

        public void SetServiceResolvers(ArrayList resolvers)
        {
            this._serviceResolvers = resolvers;
        }
    }
}

