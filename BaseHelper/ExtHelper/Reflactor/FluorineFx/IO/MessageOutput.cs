namespace FluorineFx.IO
{
    using System;

    [CLSCompliant(false)]
    public class MessageOutput : AMFMessage
    {
        public MessageOutput() : this(0)
        {
        }

        public MessageOutput(ushort version) : base(version)
        {
        }

        [CLSCompliant(false)]
        public bool ContainsResponse(AMFBody requestBody)
        {
            return (this.GetResponse(requestBody) != null);
        }

        [CLSCompliant(false)]
        public ResponseBody GetResponse(AMFBody requestBody)
        {
            for (int i = 0; i < base._bodies.Count; i++)
            {
                ResponseBody body = base._bodies[i] as ResponseBody;
                if (body.RequestBody == requestBody)
                {
                    return body;
                }
            }
            return null;
        }
    }
}

