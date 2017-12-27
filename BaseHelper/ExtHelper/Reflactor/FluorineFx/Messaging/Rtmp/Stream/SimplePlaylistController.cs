namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api.Stream;
    using System;

    internal class SimplePlaylistController : IPlaylistController
    {
        public int NextItem(IPlaylist playlist, int itemIndex)
        {
            if (itemIndex < 0)
            {
                itemIndex = -1;
            }
            if (playlist.IsRepeat)
            {
                return itemIndex;
            }
            if (playlist.IsRandom)
            {
                int num = itemIndex;
                if (playlist.Count > 1)
                {
                    Random random = new Random();
                    while (itemIndex == num)
                    {
                        itemIndex = random.Next(playlist.Count);
                    }
                }
                return itemIndex;
            }
            int num2 = itemIndex + 1;
            if (num2 < playlist.Count)
            {
                return num2;
            }
            if (playlist.IsRewind)
            {
                return ((playlist.Count > 0) ? 0 : -1);
            }
            return -1;
        }

        public int PreviousItem(IPlaylist playlist, int itemIndex)
        {
            if (itemIndex > playlist.Count)
            {
                return (playlist.Count - 1);
            }
            if (playlist.IsRepeat)
            {
                return itemIndex;
            }
            if (playlist.IsRandom)
            {
                Random random = new Random();
                int num = itemIndex;
                while (itemIndex == num)
                {
                    itemIndex = random.Next(playlist.Count);
                }
                num = itemIndex;
                return itemIndex;
            }
            int num2 = itemIndex - 1;
            if (num2 >= 0)
            {
                return num2;
            }
            if (playlist.IsRewind)
            {
                return (playlist.Count - 1);
            }
            return -1;
        }
    }
}

