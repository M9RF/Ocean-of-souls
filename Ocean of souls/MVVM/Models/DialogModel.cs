using System.Linq;
using Ocean_of_souls.MVVM.Models.Base;
using System.Collections.ObjectModel;

namespace Ocean_of_souls.MVVM.Models
{
    class DialogModel : Model
    {
        private int _Id;

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }

        private string _Name;

        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _PathImage;

        public string PathImage
        {
            get => _PathImage;
            set => Set(ref _PathImage, value);
        }

        private ObservableCollection<MessageModel> _Messages;

        public ObservableCollection<MessageModel> Messages
        {
            get => _Messages;
            set => Set(ref _Messages, value);
        }

        private string _Stat;

        public string Stat
        {
            get => _Stat;
            set => Set(ref _Stat, value);
        }

        public string LastMessage => Messages.First().Message;

        private string _Code;

        public string Code
        {
            get => _Code;
            set => Set(ref _Code, value);
        }

        private bool _IsConversation;

        public bool IsConversation
        {
            get => _IsConversation;
            set => Set(ref _IsConversation, value);
        }
    }
}
