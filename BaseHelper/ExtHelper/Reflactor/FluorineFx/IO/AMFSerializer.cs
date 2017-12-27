namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;

    public class AMFSerializer : AMFWriter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AMFSerializer));

        public AMFSerializer(Stream stream) : base(stream)
        {
        }

        internal AMFSerializer(AMFWriter writer, Stream stream) : base(writer, stream)
        {
        }

        private void WriteHeader(AMFHeader header, ObjectEncoding objectEncoding)
        {
            base.Reset();
            base.WriteUTF(header.Name);
            base.WriteBoolean(header.MustUnderstand);
            base.WriteInt32(-1);
            base.WriteData(objectEncoding, header.Content);
        }

        [CLSCompliant(false)]
        public void WriteMessage(AMFMessage amfMessage)
        {
            Exception exception;
            try
            {
                int num2;
                base.WriteShort(amfMessage.Version);
                int headerCount = amfMessage.HeaderCount;
                base.WriteShort(headerCount);
                for (num2 = 0; num2 < headerCount; num2++)
                {
                    this.WriteHeader(amfMessage.GetHeaderAt(num2), ObjectEncoding.AMF0);
                }
                int bodyCount = amfMessage.BodyCount;
                base.WriteShort(bodyCount);
                for (num2 = 0; num2 < bodyCount; num2++)
                {
                    ResponseBody bodyAt = amfMessage.GetBodyAt(num2) as ResponseBody;
                    if ((bodyAt != null) && !bodyAt.IgnoreResults)
                    {
                        if (this.BaseStream.CanSeek)
                        {
                            long position = this.BaseStream.Position;
                            try
                            {
                                bodyAt.WriteBody(amfMessage.ObjectEncoding, this);
                            }
                            catch (Exception exception1)
                            {
                                ErrorResponseBody body2;
                                exception = exception1;
                                this.BaseStream.Seek(position, SeekOrigin.Begin);
                                if (log.get_IsFatalEnabled())
                                {
                                    log.Fatal(__Res.GetString("Amf_SerializationFail"), exception);
                                }
                                if (bodyAt.RequestBody.IsEmptyTarget)
                                {
                                    object content = bodyAt.RequestBody.Content;
                                    if (content is IList)
                                    {
                                        content = (content as IList)[0];
                                    }
                                    IMessage message = content as IMessage;
                                    MessageException exception2 = new MessageException(exception) {
                                        FaultCode = __Res.GetString("Amf_SerializationFail")
                                    };
                                    body2 = new ErrorResponseBody(bodyAt.RequestBody, message, exception2);
                                }
                                else
                                {
                                    body2 = new ErrorResponseBody(bodyAt.RequestBody, exception);
                                }
                                try
                                {
                                    body2.WriteBody(amfMessage.ObjectEncoding, this);
                                }
                                catch (Exception exception3)
                                {
                                    if (log.get_IsFatalEnabled())
                                    {
                                        log.Fatal(__Res.GetString("Amf_ResponseFail"), exception3);
                                    }
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            bodyAt.WriteBody(amfMessage.ObjectEncoding, this);
                        }
                    }
                    else
                    {
                        AMFBody body3 = amfMessage.GetBodyAt(num2);
                        ValidationUtils.ObjectNotNull(body3, "amfBody");
                        body3.WriteBody(amfMessage.ObjectEncoding, this);
                    }
                }
            }
            catch (Exception exception5)
            {
                exception = exception5;
                if (log.get_IsFatalEnabled())
                {
                    log.Fatal(__Res.GetString("Amf_SerializationFail"), exception);
                }
                throw;
            }
        }
    }
}

