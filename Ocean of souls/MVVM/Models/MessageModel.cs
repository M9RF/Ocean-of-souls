using Ocean_of_souls.MVVM.Models.Base;
using System;

namespace Ocean_of_souls.MVVM.Models
{
    class MessageModel : Model
    {
        private int _Id;

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }

        private string _Username;

        public string Username
        {
            get => _Username;
            set => Set(ref _Username, value);
        }

        private string _PathImage;

        public string PathImage
        {
            get => _PathImage;
            set => Set(ref _PathImage, value);
        }

        private string _Message;

        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
        }

        private DateTime _Time;

        public DateTime Time
        {
            get => _Time;
            set => Set(ref _Time, value);
        }

        private bool _IsNativeOrigin;

        public bool IsNativeOrigin
        {
            get => _IsNativeOrigin;
            set => Set(ref _IsNativeOrigin, value);
        }

        private bool _FirstMessage;

        public bool FirstMessage
        {
            get => _FirstMessage;
            set => Set(ref _FirstMessage, value);
        }
    }
}
