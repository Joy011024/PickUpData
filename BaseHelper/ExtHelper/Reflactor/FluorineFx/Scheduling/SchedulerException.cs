namespace FluorineFx.Scheduling
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    public class SchedulerException : ApplicationException
    {
        private readonly Exception cause;
        public const int ErrorBadConfiguration = 50;
        public const int ErrorClientError = 100;
        private int errorCode;
        public const int ErrorJobExecutionThrewException = 800;
        public const int ErrorTriggerThrewException = 850;
        public const int ErrorUnspecified = 0;
        public const int ErrorUnsupportedFunctionInThisConfiguration = 210;

        public SchedulerException()
        {
            this.errorCode = 0;
        }

        public SchedulerException(Exception cause) : base(cause.ToString(), cause)
        {
            this.errorCode = 0;
            this.cause = cause;
        }

        public SchedulerException(string msg) : base(msg)
        {
            this.errorCode = 0;
        }

        public SchedulerException(SerializationInfo info, StreamingContext context) : base(info.GetString("Message"))
        {
            this.errorCode = 0;
        }

        public SchedulerException(string msg, Exception cause) : base(msg, cause)
        {
            this.errorCode = 0;
            this.cause = cause;
        }

        public SchedulerException(string msg, int errorCode) : base(msg)
        {
            this.errorCode = 0;
            this.ErrorCode = errorCode;
        }

        public SchedulerException(string msg, Exception cause, int errorCode) : base(msg, cause)
        {
            this.errorCode = 0;
            this.cause = cause;
            this.ErrorCode = errorCode;
        }

        public override string ToString()
        {
            if (this.cause == null)
            {
                return base.ToString();
            }
            return string.Format(CultureInfo.InvariantCulture, "{0} [See nested exception: {1}]", new object[] { base.ToString(), this.cause });
        }

        public virtual bool ClientError
        {
            get
            {
                return ((this.errorCode >= 100) && (this.errorCode <= 0xc7));
            }
        }

        public virtual bool ConfigurationError
        {
            get
            {
                return ((this.errorCode >= 50) && (this.errorCode <= 0x63));
            }
        }

        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }

        public virtual Exception UnderlyingException
        {
            get
            {
                return this.cause;
            }
        }
    }
}

