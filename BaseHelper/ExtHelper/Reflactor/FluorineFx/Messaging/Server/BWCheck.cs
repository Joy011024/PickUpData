namespace FluorineFx.Messaging.Server
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using System;
    using System.Collections;

    internal class BWCheck : IPendingServiceCallback
    {
        private ArrayList _beginningValues = new ArrayList();
        private IConnection _connection;
        private int _count = 0;
        private double _cumLatency = 1.0;
        private double _deltaDown = 0.0;
        private double _deltaTime = 0.0;
        private double _kbitDown = 0.0;
        private double _latency = 0.0;
        private ArrayList _pakRecv = new ArrayList();
        private ArrayList _pakSent = new ArrayList();
        private int _sent = 0;

        public BWCheck(IConnection connection)
        {
            this._connection = connection;
        }

        public void CalculateClientBw()
        {
            int num;
            Random random = new Random();
            double[] numArray = new double[0x4b0];
            for (num = 0; num < numArray.Length; num++)
            {
                numArray[num] = random.NextDouble();
            }
            this._connection.SetAttribute("payload", numArray);
            double[] numArray2 = new double[0x2ee0];
            for (num = 0; num < numArray2.Length; num++)
            {
                numArray2[num] = random.NextDouble();
            }
            this._connection.SetAttribute("payload1", numArray2);
            double[] numArray3 = new double[0x4b0];
            for (num = 0; num < numArray3.Length; num++)
            {
                numArray3[num] = random.NextDouble();
            }
            this._connection.SetAttribute("payload2", numArray3);
            long tickCount = Environment.TickCount;
            this._beginningValues.Add(this._connection.WrittenBytes);
            this._beginningValues.Add(this._connection.ReadBytes);
            this._beginningValues.Add(tickCount);
            this._pakSent.Add(tickCount);
            this._sent++;
            ServiceUtils.InvokeOnConnection(this._connection, "onBWCheck", new object[0], this);
        }

        public void ResultReceived(IPendingServiceCall call)
        {
            long tickCount = Environment.TickCount;
            this._pakRecv.Add(tickCount);
            long num2 = tickCount - ((long) this._beginningValues[2]);
            this._count++;
            if (this._count == 1)
            {
                this._latency = Math.Min(num2, 800L);
                this._latency = Math.Max(this._latency, 10.0);
                this._pakSent.Add(tickCount);
                this._sent++;
                ServiceUtils.InvokeOnConnection(this._connection, "onBWCheck", new object[] { this._connection.GetAttribute("payload") }, this);
            }
            else if (((this._count > 1) && (this._count < 3)) && (num2 < 0x3e8L))
            {
                this._pakSent.Add(tickCount);
                this._sent++;
                this._cumLatency++;
                ServiceUtils.InvokeOnConnection(this._connection, "onBWCheck", new object[] { this._connection.GetAttribute("payload") }, this);
            }
            else if (((this._count >= 3) && (this._count < 6)) && (num2 < 0x3e8L))
            {
                this._pakSent.Add(tickCount);
                this._sent++;
                this._cumLatency++;
                ServiceUtils.InvokeOnConnection(this._connection, "onBWCheck", new object[] { this._connection.GetAttribute("payload1") }, this);
            }
            else if ((this._count >= 6) && (num2 < 0x3e8L))
            {
                this._pakSent.Add(tickCount);
                this._sent++;
                this._cumLatency++;
                ServiceUtils.InvokeOnConnection(this._connection, "onBWCheck", new object[] { this._connection.GetAttribute("payload2") }, this);
            }
            else if (this._sent == this._count)
            {
                if ((this._latency >= 100.0) && ((((long) this._pakRecv[1]) - ((long) this._pakRecv[0])) > 0x3e8L))
                {
                    this._latency = 100.0;
                }
                this._connection.RemoveAttribute("payload");
                this._connection.RemoveAttribute("payload1");
                this._connection.RemoveAttribute("payload2");
                this._deltaDown = ((this._connection.WrittenBytes - ((long) this._beginningValues[0])) * 8L) / 0x3e8L;
                this._deltaTime = ((tickCount - ((long) this._beginningValues[2])) - (this._latency * this._cumLatency)) / 1000.0;
                if (this._deltaTime <= 0.0)
                {
                    this._deltaTime = (tickCount - ((long) this._beginningValues[2])) / 0x3e8L;
                }
                this._kbitDown = Math.Round((double) (this._deltaDown / this._deltaTime));
                ServiceUtils.InvokeOnConnection(this._connection, "onBWDone", new object[] { this._kbitDown, this._deltaDown, this._deltaTime, this._latency }, this);
            }
        }
    }
}

