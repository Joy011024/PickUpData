namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class AMFDeserializer : AMFReader
    {
        private List<AMFBody> _failedAMFBodies;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMFDeserializer));

        public AMFDeserializer(Stream stream) : base(stream)
        {
            this._failedAMFBodies = new List<AMFBody>(1);
            base.FaultTolerancy = true;
        }

        private ErrorResponseBody GetErrorBody(AMFBody amfBody, Exception exception)
        {
            try
            {
                object content = amfBody.Content;
                if (content is IList)
                {
                    content = (content as IList)[0];
                }
                if (content is IMessage)
                {
                    return new ErrorResponseBody(amfBody, content as IMessage, exception);
                }
                return new ErrorResponseBody(amfBody, exception);
            }
            catch
            {
                return new ErrorResponseBody(amfBody, exception);
            }
        }

        [CLSCompliant(false)]
        public AMFMessage ReadAMFMessage()
        {
            int num3;
            AMFMessage message = new AMFMessage(base.ReadUInt16());
            int num2 = base.ReadUInt16();
            for (num3 = 0; num3 < num2; num3++)
            {
                message.AddHeader(this.ReadHeader());
            }
            int num4 = base.ReadUInt16();
            for (num3 = 0; num3 < num4; num3++)
            {
                AMFBody body = this.ReadBody();
                if (body != null)
                {
                    message.AddBody(body);
                }
            }
            return message;
        }

        private AMFBody ReadBody()
        {
            object obj2;
            AMFBody body;
            Exception lastError;
            ErrorResponseBody errorBody;
            AMFBody body3;
            base.Reset();
            string target = base.ReadString();
            string response = base.ReadString();
            int num = base.ReadInt32();
            if (base.BaseStream.CanSeek)
            {
                long position = base.BaseStream.Position;
                try
                {
                    obj2 = base.ReadData();
                    body = new AMFBody(target, response, obj2);
                    lastError = base.LastError;
                    if (lastError != null)
                    {
                        errorBody = this.GetErrorBody(body, lastError);
                        this._failedAMFBodies.Add(errorBody);
                        if (log.get_IsFatalEnabled())
                        {
                            log.Fatal(__Res.GetString("Amf_ReadBodyFail"), lastError);
                        }
                        return null;
                    }
                    return body;
                }
                catch (Exception exception1)
                {
                    lastError = exception1;
                    base.BaseStream.Position = position + num;
                    body = new AMFBody(target, response, null);
                    errorBody = this.GetErrorBody(body, lastError);
                    this._failedAMFBodies.Add(errorBody);
                    if (log.get_IsFatalEnabled())
                    {
                        log.Fatal(__Res.GetString("Amf_ReadBodyFail"), lastError);
                    }
                    return null;
                }
            }
            try
            {
                obj2 = base.ReadData();
                body = new AMFBody(target, response, obj2);
                lastError = base.LastError;
                if (lastError != null)
                {
                    errorBody = this.GetErrorBody(body, lastError);
                    this._failedAMFBodies.Add(errorBody);
                    if (log.get_IsFatalEnabled())
                    {
                        log.Fatal(__Res.GetString("Amf_ReadBodyFail"), lastError);
                    }
                    return null;
                }
                body3 = body;
            }
            catch (Exception exception2)
            {
                lastError = exception2;
                body = new AMFBody(target, response, null);
                errorBody = this.GetErrorBody(body, lastError);
                this._failedAMFBodies.Add(errorBody);
                if (log.get_IsFatalEnabled())
                {
                    log.Fatal(__Res.GetString("Amf_ReadBodyFail"), lastError);
                }
                throw;
            }
            return body3;
        }

        private AMFHeader ReadHeader()
        {
            base.Reset();
            string name = base.ReadString();
            bool mustUnderstand = base.ReadBoolean();
            int num = base.ReadInt32();
            return new AMFHeader(name, mustUnderstand, base.ReadData());
        }

        [CLSCompliant(false)]
        public AMFBody[] FailedAMFBodies
        {
            get
            {
                return this._failedAMFBodies.ToArray();
            }
        }
    }
}

