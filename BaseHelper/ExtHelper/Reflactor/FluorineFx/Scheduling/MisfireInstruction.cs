namespace FluorineFx.Scheduling
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct MisfireInstruction
    {
        public const int InstructionNotSet = 0;
        public const int SmartPolicy = 0;
        public const int FireNow = 1;
        public const int RescheduleNowWithExistingRepeatCount = 2;
        public const int RescheduleNowWithRemainingRepeatCount = 3;
        public const int RescheduleNextWithRemainingCount = 4;
        public const int RescheduleNextWithExistingCount = 5;
    }
}

