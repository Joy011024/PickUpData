namespace FluorineFx.Messaging.Api.Stream
{
    using System;

    [CLSCompliant(false)]
    public interface IPlaylistController
    {
        int NextItem(IPlaylist playlist, int itemIndex);
        int PreviousItem(IPlaylist playlist, int itemIndex);
    }
}

