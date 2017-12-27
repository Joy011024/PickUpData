namespace FluorineFx.Messaging.Api.Stream
{
    using System;

    [CLSCompliant(false)]
    public interface IPlaylist
    {
        void AddItem(IPlayItem item);
        void AddItem(IPlayItem item, int index);
        IPlayItem GetItem(int index);
        void NextItem();
        void PreviousItem();
        void RemoveAllItems();
        void RemoveItem(int index);
        void SetItem(int index);
        void SetPlaylistController(IPlaylistController controller);

        int Count { get; }

        IPlayItem CurrentItem { get; }

        int CurrentItemIndex { get; }

        bool HasMoreItems { get; }

        bool IsRandom { get; set; }

        bool IsRepeat { get; set; }

        bool IsRewind { get; set; }
    }
}

