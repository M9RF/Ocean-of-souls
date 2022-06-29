using System.Windows;
using System.Windows.Input;
using Ocean_of_souls.MVVM.ViewModels.Base;
using Ocean_of_souls.Infrastructure.Commands;

namespace Ocean_of_souls.MVVM.ViewModels
{
    class DialogViewModel : ViewModel 
    {
        #region Properties

        #region General
        private string _WindowBackgroundColor;

        public string WindowBackgroundColor
        {
            get => _WindowBackgroundColor;
            set => Set(ref _WindowBackgroundColor, value);
        }

        private string _WindowTitleBackgroundColor;

        public string WindowTitleBackgroundColor
        {
            get => _WindowTitleBackgroundColor;
            set => Set(ref _WindowTitleBackgroundColor, value);
        }

        private string _WindowForeground;

        public string WindowForeground
        {
            get => _WindowForeground;
            set => Set(ref _WindowForeground, value);
        }

        private string _WindowTitle;

        public string WindowTitle
        {
            get => _WindowTitle;
            set => Set(ref _WindowTitle, value);
        }

        private string _Chapters;

        public string Chapters
        {
            get => _Chapters;
            set => Set(ref _Chapters, value);
        }

        private string _ContactBackgroundColor;

        public string ContactBackgroundColor
        {
            get => _ContactBackgroundColor;
            set => Set(ref _ContactBackgroundColor, value);
        }

        private string _BorderBrushColor;

        public string BorderBrushColor
        {
            get => _BorderBrushColor;
            set => Set(ref _BorderBrushColor, value);
        }

        private string _InformationBackgroundColor;

        public string InformationBackgroundColor
        {
            get => _InformationBackgroundColor;
            set => Set(ref _InformationBackgroundColor, value);
        }
        #endregion

        #region Message
        private string _MessageText;

        public string MessageText
        {
            get => _MessageText;
            set => Set(ref _MessageText, value);
        }

        private int _IdItem;

        public int IdItem
        {
            get => _IdItem;
            set => Set(ref _IdItem, value);
        }
        #endregion

        #region AddDialog
        private string _PathToLogotype;

        public string PathToLogotype
        {
            get => _PathToLogotype;
            set => Set(ref _PathToLogotype, value);
        }

        private string _NameGroup;

        public string NameGroup
        {
            get => _NameGroup;
            set => Set(ref _NameGroup, value);
        }

        #endregion

        #endregion

        public DialogViewModel()
        {
            /// Properties
            // General
            if (StaticTransmitter.IsDark)
            {
                WindowTitle = "Ocean os souls";
                WindowBackgroundColor = "#FF3E3F40";
                WindowTitleBackgroundColor = "#252525";
                WindowForeground = "Gray";
                ContactBackgroundColor = "#2F3136";
                InformationBackgroundColor = "#292B2F";
                BorderBrushColor = "#FF9E9E9E";
                Chapters = "#FFEEEEEE";
            }
            else
            {
                WindowTitle = "Ocean os souls";
                WindowBackgroundColor = "White";
                WindowTitleBackgroundColor = "#bfbfbf";
                WindowForeground = "#969696";
                ContactBackgroundColor = "#e6e6e6";
                InformationBackgroundColor = "#e3dcdc";
                BorderBrushColor = "#a8a8a8";
                Chapters = "#635d5d";
            }

            // AddChat
            PathToLogotype = "/LocalResources/Images/Logotype.png";

            // Message
            MessageText = StaticTransmitter.MessageText;
            IdItem = StaticTransmitter.IdItem;

            /// Commands
            // General
            CloseWindowCommand = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            // Message
            GiveAnAnswerCommand = new LambdaCommand(OnGiveAnAnswerCommandExecuted, CanGiveAnAnswerCommandExecute);
            // AddDialog
            AddDialogCommand = new LambdaCommand(OnAddDialogCommandExecuted, CanAddDialogCommandExecute);
            // CreateDialog
            CreateDialogCommand = new LambdaCommand(OnCreateDialogCommandExecuted, CanCreateDialogCommandExecute);
        }

        #region Helper function
        public void OnCloseWindow(object p) => (p as Window).Close();
        #endregion

        #region Commands

        #region General
        public ICommand CloseWindowCommand { get; }

        public bool CanCloseWindowCommandExecute(object p) => true;

        public void OnCloseWindowCommandExecuted(object p)
        {
            StaticTransmitter.IsAnswer = false;
            StaticTransmitter.isCreationGroup = false;
            OnCloseWindow(p);
        }
        #endregion

        #region Message
        public ICommand GiveAnAnswerCommand { get; }

        public bool CanGiveAnAnswerCommandExecute(object p) => true;

        public void OnGiveAnAnswerCommandExecuted(object p)
        {
            StaticTransmitter.IsAnswer = true;
            OnCloseWindow(p);
        }
        #endregion

        #region AddDialog
        public ICommand AddDialogCommand { get; }

        public bool CanAddDialogCommandExecute(object p) => true;

        public void OnAddDialogCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(NameGroup))
                NameGroup = "Пустое значение";
            else
            {
                StaticTransmitter.NameGroup = NameGroup;
                StaticTransmitter.isCreationGroup = true;
                OnCloseWindow(p);
            }
        }
        #endregion

        #region CreateDialog
        public ICommand CreateDialogCommand { get; }

        public bool CanCreateDialogCommandExecute(object p) => true;

        public void OnCreateDialogCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(NameGroup))
                NameGroup = "Пустое значение";
            else
            {
                StaticTransmitter.NameGroup = NameGroup;
                StaticTransmitter.isCreationGroup = true;
                OnCloseWindow(p);
            }
        }
        #endregion

        #endregion
    }
}
