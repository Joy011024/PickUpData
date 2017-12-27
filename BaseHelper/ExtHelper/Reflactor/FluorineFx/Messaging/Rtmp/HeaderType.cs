namespace FluorineFx.Messaging.Rtmp
{
    using System;

    public enum HeaderType : byte
    {
        HeaderContinue = 3,
        HeaderNew = 0,
        HeaderSameSource = 1,
        HeaderTimerChange = 2
    }
}

