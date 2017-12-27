namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.Context;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Security;

    internal class MessageFilter : AbstractFilter
    {
        private EndpointBase _endpoint;
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageFilter));

        public MessageFilter(EndpointBase endpoint)
        {
            this._endpoint = endpoint;
        }

        public override void Invoke(AMFContext context)
        {
            MessageOutput messageOutput = context.MessageOutput;
            for (int i = 0; i < context.AMFMessage.BodyCount; i++)
            {
                ResponseBody body2;
                AMFBody bodyAt = context.AMFMessage.GetBodyAt(i);
                if (!bodyAt.IsEmptyTarget)
                {
                    continue;
                }
                object content = bodyAt.Content;
                if (content is IList)
                {
                    content = (content as IList)[0];
                }
                IMessage message = content as IMessage;
                if (message == null)
                {
                    continue;
                }
                if (FluorineContext.Current.Client == null)
                {
                    FluorineContext.Current.SetCurrentClient(this._endpoint.GetMessageBroker().ClientRegistry.GetClient(message));
                }
                if (message.clientId == null)
                {
                    message.clientId = Guid.NewGuid().ToString("D");
                }
                if (messageOutput.GetResponse(bodyAt) != null)
                {
                    continue;
                }
                try
                {
                    if (context.AMFMessage.BodyCount > 1)
                    {
                        CommandMessage message2 = message as CommandMessage;
                        if ((message2 != null) && (message2.operation == 2))
                        {
                            message2.SetHeader(CommandMessage.FluorineSuppressPollWaitHeader, null);
                        }
                    }
                    IMessage message3 = this._endpoint.ServiceMessage(message);
                    if (message3 is ErrorMessage)
                    {
                        ErrorMessage message4 = message3 as ErrorMessage;
                        body2 = new ErrorResponseBody(bodyAt, message, message3 as ErrorMessage);
                        if (!(message4.faultCode == "Client.Authentication"))
                        {
                            goto Label_0291;
                        }
                        messageOutput.AddBody(body2);
                        for (int j = i + 1; j < context.AMFMessage.BodyCount; j++)
                        {
                            bodyAt = context.AMFMessage.GetBodyAt(j);
                            if (bodyAt.IsEmptyTarget)
                            {
                                content = bodyAt.Content;
                                if (content is IList)
                                {
                                    content = (content as IList)[0];
                                }
                                message = content as IMessage;
                                if (message != null)
                                {
                                    body2 = new ErrorResponseBody(bodyAt, message, new SecurityException(message4.faultString));
                                    messageOutput.AddBody(body2);
                                }
                            }
                        }
                        break;
                    }
                    body2 = new ResponseBody(bodyAt, message3);
                }
                catch (Exception exception)
                {
                    if ((log != null) && log.get_IsErrorEnabled())
                    {
                        log.Error(exception.Message, exception);
                    }
                    body2 = new ErrorResponseBody(bodyAt, message, exception);
                }
            Label_0291:
                messageOutput.AddBody(body2);
            }
        }
    }
}

