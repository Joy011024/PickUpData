namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using log4net;
    using System;

    internal class VideoFrameDropper : IFrameDropper
    {
        private FrameDropperState _state;
        private static ILog log = LogManager.GetLogger(typeof(VideoFrameDropper));

        public VideoFrameDropper()
        {
            this.Reset();
        }

        public bool CanSendPacket(RtmpMessage message, long pending)
        {
            IRtmpEvent body = message.body;
            if (!(body is VideoData))
            {
                return true;
            }
            VideoData data = body as VideoData;
            FrameType frameType = data.FrameType;
            bool flag = false;
            switch (this._state)
            {
                case FrameDropperState.SEND_ALL:
                    return true;

                case FrameDropperState.SEND_INTERFRAMES:
                    if (frameType != FrameType.KEYFRAME)
                    {
                        if (frameType == FrameType.INTERFRAME)
                        {
                            flag = true;
                        }
                        return flag;
                    }
                    if (pending == 0L)
                    {
                        this._state = FrameDropperState.SEND_ALL;
                    }
                    return true;

                case FrameDropperState.SEND_KEYFRAMES:
                    flag = frameType == FrameType.KEYFRAME;
                    if (flag && (pending == 0L))
                    {
                        this._state = FrameDropperState.SEND_KEYFRAMES_CHECK;
                    }
                    return flag;

                case FrameDropperState.SEND_KEYFRAMES_CHECK:
                    flag = frameType == FrameType.KEYFRAME;
                    if (flag && (pending == 0L))
                    {
                        this._state = FrameDropperState.SEND_INTERFRAMES;
                    }
                    return flag;
            }
            return flag;
        }

        public void DropPacket(RtmpMessage message)
        {
            IRtmpEvent body = message.body;
            if (body is VideoData)
            {
                VideoData data = body as VideoData;
                FrameType frameType = data.FrameType;
                switch (this._state)
                {
                    case FrameDropperState.SEND_ALL:
                        if (frameType != FrameType.DISPOSABLE_INTERFRAME)
                        {
                            if (frameType == FrameType.INTERFRAME)
                            {
                                this._state = FrameDropperState.SEND_KEYFRAMES;
                            }
                            else if (frameType == FrameType.KEYFRAME)
                            {
                                this._state = FrameDropperState.SEND_KEYFRAMES;
                            }
                            break;
                        }
                        break;

                    case FrameDropperState.SEND_INTERFRAMES:
                        if (frameType != FrameType.INTERFRAME)
                        {
                            if (frameType == FrameType.KEYFRAME)
                            {
                                this._state = FrameDropperState.SEND_KEYFRAMES;
                            }
                            break;
                        }
                        this._state = FrameDropperState.SEND_KEYFRAMES_CHECK;
                        break;

                    case FrameDropperState.SEND_KEYFRAMES_CHECK:
                        if (frameType == FrameType.KEYFRAME)
                        {
                            this._state = FrameDropperState.SEND_KEYFRAMES;
                        }
                        break;
                }
            }
        }

        public void Reset()
        {
            this.Reset(FrameDropperState.SEND_ALL);
        }

        public void Reset(FrameDropperState state)
        {
            this._state = state;
        }

        public void SendPacket(RtmpMessage message)
        {
        }
    }
}

