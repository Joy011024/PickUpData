namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal class DescribeServiceFilter : AbstractFilter
    {
        public override void Invoke(AMFContext context)
        {
            AMFMessage aMFMessage = context.AMFMessage;
            if (aMFMessage.GetHeader("DescribeService") != null)
            {
                aMFMessage.GetBodyAt(0).IsDescribeService = true;
            }
        }
    }
}

