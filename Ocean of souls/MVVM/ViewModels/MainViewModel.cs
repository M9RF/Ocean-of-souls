using System;
using System.Linq;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Controls;
using Ocean_of_souls.DataService;
using Ocean_of_souls.MVVM.Models;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Ocean_of_souls.MVVM.Views.Windows;
using Ocean_of_souls.MVVM.ViewModels.Base;
using Ocean_of_souls.Infrastructure.Commands;
using Ocean_of_souls.Infrastructure.InformationSecurity.Hash;
using Ocean_of_souls.Infrastructure.InformationSecurity.AsynchronousEncryption;
using Ocean_of_souls.MVVM.Views.Pages.Main.AboutAccount;
using Ocean_of_souls.MVVM.Views.Pages.Main.AboutSettings;

namespace Ocean_of_souls.MVVM.ViewModels
{
    class MainViewModel : ViewModel
    {
        #region Properties

        #region User
        public ObservableCollection<UserModel> User { get; set; } = new ObservableCollection<UserModel>();

        public ObservableCollection<ProfileModel> UserProfile { get; set; } = new ObservableCollection<ProfileModel>();
        #endregion

        #region General
        /// <summary>
        /// App navigation
        /// </summary>
        public enum AuthorizationMode { Welcome, SignUp, SignIn, Paramount, AllContact, AbAccount, AbSettings, LoadingLocation, InformationUser };

        private AuthorizationMode _Authorization;

        public AuthorizationMode Authorization
        {
            get => _Authorization;
            set => Set(ref _Authorization, value);
        }

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

        private string _WindowIcon;

        public string WindowIcon
        {
            get => _WindowIcon;
            set => Set(ref _WindowIcon, value);
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

        #region Welcome
        private string _PathToLogotype;

        public string PathToLogotype
        {
            get => _PathToLogotype;
            set => Set(ref _PathToLogotype, value);
        }
        #endregion

        #region SignUp
        private string _UserLoginSignUp;

        public string UserLoginSignUp
        {
            get => _UserLoginSignUp;
            set => Set(ref _UserLoginSignUp, value);
        }

        private string _UserEmailSignUp;

        public string UserEmailSignUp
        {
            get => _UserEmailSignUp;
            set => Set(ref _UserEmailSignUp, value);
        }

        private string _UserNameSignUp;

        public string UserNameSignUp
        {
            get => _UserNameSignUp;
            set => Set(ref _UserNameSignUp, value);
        }
        #endregion

        #region SignIn
        private string _UserLoginSignIn;

        public string UserLoginSignIn
        {
            get => _UserLoginSignIn;
            set => Set(ref _UserLoginSignIn, value);
        }
        #endregion

        #region Paramount
        public ObservableCollection<DialogModel> Dialogs { get; set; } = new ObservableCollection<DialogModel>();

        private DialogModel _SelectedDialog;

        public DialogModel SelectedDialog
        {
            get => _SelectedDialog;
            set => Set(ref _SelectedDialog, value);
        }

        private string _Message;

        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
        }
        #endregion

        #region AllContact
        public ObservableCollection<DialogModel> Contacts { get; set; } = new ObservableCollection<DialogModel>();

        private string _Pattern;

        public string Pattern
        {
            get => _Pattern;
            set
            {
                Set(ref _Pattern, value);
                SelectedContact = Contacts.FirstOrDefault(s => s.Name.Equals(Pattern));
            }
        }

        private DialogModel _SelectedContact;

        public DialogModel SelectedContact
        {
            get => _SelectedContact;
            set => Set(ref _SelectedContact, value);
        }
        #endregion

        #region Account and Settings
        private UserControl _AboutAccountCurrent;

        public UserControl AboutAccountCurrent
        {
            get => _AboutAccountCurrent;
            set => Set(ref _AboutAccountCurrent, value);
        }

        private UserControl _AboutSettingsCurrent;

        public UserControl AboutSettingsCurrent
        {
            get => _AboutSettingsCurrent;
            set => Set(ref _AboutSettingsCurrent, value);
        }

        #region Account
        private string _NewName;

        public string NewName
        {
            get => _NewName;
            set => Set(ref _NewName, value);
        }

        private string _NewEmail;

        public string NewEmail
        {
            get => _NewEmail;
            set => Set(ref _NewEmail, value);
        }

        private string _NewStat;

        public string NewStat
        {
            get => _NewStat;
            set => Set(ref _NewStat, value);
        }

        private string _Complaint;

        public string Complaint
        {
            get => _Complaint;
            set => Set(ref _Complaint, value);
        }

        private string _Technical;

        public string Technical
        {
            get => _Technical;
            set => Set(ref _Technical, value);
        }
        #endregion

        #endregion

        #endregion

        public static MainViewModel Instance { get; } = new MainViewModel();

        private MainViewModel()
        {
            /// Standard setting
            WindowTitle = "Ocean of souls";
            WindowBackgroundColor = "#FF3E3F40";
            WindowTitleBackgroundColor = "#252525";
            WindowForeground = "Gray";
            ContactBackgroundColor = "#2F3136";
            InformationBackgroundColor = "#292B2F";
            BorderBrushColor = "#FF9E9E9E";
            Chapters = "#FFEEEEEE";
            WindowIcon = "/LocalResources/Images/ico.ico";

            /// Properties

            // Welcome
            PathToLogotype = "/LocalResources/Images/Logotype.png";

            /// Commands
            // SignUp
            AccountRegistrationCommand = new LambdaCommand(OnAccountRegistrationCommandExecuted, CanAccountRegistrationCommandExecute);

            // SignIn
            AccountAuthorizationCommand = new LambdaCommand(OnAccountAuthorizationCommandExecuted, CanAccountAuthorizationCommandCommandExecute);

            // Paramount
            SendMessageCommand = new LambdaCommand(OnSendMessageCommandExecuted, CanSendMessageCommandExecute);
            DeleteDialogCommand = new LambdaCommand(OnDeleteDialogCommandExecuted, CanDeleteDialogCommandExecute);
            AddDialogCommand = new LambdaCommand(OnAddDialogCommandExecuted, CanAddDialogCommandExecute);
            CreateDialogCommand = new LambdaCommand(OnCreateDialogCommandExecuted, CanCreateDialogCommandExecute);
            UpdateDialogCommand = new LambdaCommand(OnUpdateDialogCommandExecuted, CanUpdateDialogCommandExecute);

            // AllContact
            AllContactShowCommand = new LambdaCommand(OnAllContactShowCommandExecuted, CanAllContactShowCommandExecute);
            AddContactCommand = new LambdaCommand(OnAddContactCommandExecuted, CanAddContactCommandExecute);

            /// Account and Settings
            SwitchingPageCommand = new LambdaCommand(OnSwitchingPageCommandExecuted, CanSwitchingPageCommandExecute);
            ChangePhotoCommand = new LambdaCommand(OnChangePhotoCommandExecuted, CanChangePhotoCommandExecute);
            RenameNameCommand = new LambdaCommand(OnRenameNameCommandExecuted, CanRenameNameCommandExecute);
            RenameStatCommand = new LambdaCommand(OnRenameStatCommandExecuted, CanRenameStatCommandExecute);
            RenameEmailCommand = new LambdaCommand(OnRenameEmailCommandExecuted, CanRenameEmailCommandExecute);
            RenamePasswordCommand = new LambdaCommand(OnRenamePasswordCommandExecuted, CanRenamePasswordCommandExecute);
            ChangeThemeCommand = new LambdaCommand(OnChangeThemeCommandExecuted, CanChangeThemeCommandExecute);
            SubmitAcomplaintCommand = new LambdaCommand(OnSubmitAcomplaintCommandExecuted, CanSubmitAcomplaintCommandExecute);
            TechnicalAcomplaintCommand = new LambdaCommand(OnTechnicalAcomplaintCommandExecuted, CanTechnicalAcomplaintCommandExecute);
            ClearApplicationCommand = new LambdaCommand(OnClearApplicationCommandExecuted, CanClearApplicationCommandExecute);
            ExitFromAccountCommand = new LambdaCommand(OnExitFromAccountCommandExecuted, CanExitFromAccountCommandExecute);
        }

        #region Helper function
        public bool isEmptyField(object[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
                if (string.IsNullOrEmpty((fields[i] as string)))
                    return true;
            return false;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }

            catch
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        private void ModeCanMethod(AuthorizationMode parameter)
        {
            switch (parameter)
            {
                case AuthorizationMode.Welcome:
                    Authorization = AuthorizationMode.Welcome;
                    break;

                case AuthorizationMode.SignUp:
                    Authorization = AuthorizationMode.SignUp;
                    break;

                case AuthorizationMode.SignIn:
                    Authorization = AuthorizationMode.SignIn;
                    break;

                case AuthorizationMode.Paramount:
                    Authorization = AuthorizationMode.Paramount;
                    break;

                case AuthorizationMode.AllContact:
                    Authorization = AuthorizationMode.AllContact;
                    break;

                case AuthorizationMode.LoadingLocation:
                    Authorization = AuthorizationMode.LoadingLocation;
                    break;
            }
        }

        private void LoadingUserData()
        {
            var idUser = DatabaseManagement.GetIdUsersByMaskLogin(StaticTransmitter.Login);
            User = DatabaseManagement.GetUserByMaskId(idUser);
            UserProfile = DatabaseManagement.GetUserProfileByMaskId(User[0].Id);

            if (UserProfile[0].Theme)
            {
                StaticTransmitter.IsDark = true;
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
                StaticTransmitter.IsDark = false;
                WindowTitle = "Ocean os souls";
                WindowBackgroundColor = "White";
                WindowTitleBackgroundColor = "#bfbfbf";
                WindowForeground = "#969696";
                ContactBackgroundColor = "#e6e6e6";
                InformationBackgroundColor = "#e3dcdc";
                BorderBrushColor = "#a8a8a8";
                Chapters = "#635d5d";
            }

            DatabaseManagement.AddVisit(User[0].Id);
            DatabaseManagement.AddCharacteristics(User[0].Id);
        }

        private void LoadingAllUser()
        {
            Contacts = DatabaseManagement.GetAllUserProfiles();
            var itemToDel = Contacts.Where(x => x.Id == User[0].Id).First();
            var itemIndexToDel = Contacts.IndexOf(itemToDel);
            Contacts.RemoveAt(itemIndexToDel);

            for (int i = 0; i < Dialogs.Count; i++)
            {
                bool isItemDel = Contacts.Where((x) => x.Name == Dialogs[i].Name).Any();
                
                if (isItemDel)
                {
                    var itemToDelContacts = Contacts.Where(x => x.Name == Dialogs[i].Name).First();
                    var itemIndexToDelContacts = Contacts.IndexOf(itemToDelContacts);
                    Contacts.RemoveAt(itemIndexToDelContacts);
                }
            }
        }

        private void LoadingDialogs()
        {
            Dialogs = DatabaseManagement.GetDialogByMaskIdAndName(User[0].Id, UserProfile[0].Name);
        }
        #endregion

        #region Commands

        #region SignUp
        public ICommand AccountRegistrationCommand { get; }

        public bool CanAccountRegistrationCommandExecute(object p) => true;

        public void OnAccountRegistrationCommandExecuted(object p)
        {
            var passwordString = (p as PasswordBox).Password;
            object[] fields = { UserLoginSignUp, UserEmailSignUp, UserNameSignUp, passwordString };

            var resultIsEmptyField = isEmptyField(fields);
            
            if (resultIsEmptyField)
                UserLoginSignUp = "Заполните все поля!";

            else
            {
                if (IsValidEmail(UserEmailSignUp))
                {
                    var random = new Random();
                    var profileData = new ProfileModel
                    {
                        Email = UserEmailSignUp,
                        Name = UserNameSignUp,
                        Id_photo = random.Next(1, 20),
                        Id_stat = 1,
                        Id_theme = 1
                    };

                    var validationOfExistingEmail = DatabaseManagement.GetIdProfilesByMaskEmail(UserEmailSignUp);
                    var validationOfExistingLogin = DatabaseManagement.GetIdUsersByMaskLogin(UserLoginSignUp);

                    if (validationOfExistingEmail.Equals(-1) && validationOfExistingLogin.Equals(-1))
                    {
                        DatabaseManagement.AddProfile(profileData);
                        var idAddedProfile = DatabaseManagement.GetIdProfilesByMaskEmail(profileData.Email);
                        profileData.Id = idAddedProfile;

                        var userData = new UserModel
                        {
                            Login = UserLoginSignUp,
                            Password = RFC.GetHashPassword(passwordString),
                            Id_profile = profileData.Id,
                            Id_access_level = 1
                        };

                        DatabaseManagement.AddUser(userData);

                        ModeCanMethod(AuthorizationMode.SignIn);
                    }

                    else UserLoginSignUp = "Такой пользователь уже существует!";
                }
                else UserEmailSignUp = "Неверный формат!";
            }
        }
        #endregion

        #region SignIn
        public ICommand AccountAuthorizationCommand { get; }

        public bool CanAccountAuthorizationCommandCommandExecute(object parameter) => true;

        public void OnAccountAuthorizationCommandExecuted(object parameter)
        {
            var passwordString = (parameter as PasswordBox).Password;
            var userData = new UserModel
            {
                Login = UserLoginSignIn,
                Password = passwordString
            };

            if (DatabaseManagement.IsExistsNamePasswordByMaskUserModel(userData))
            {
                StaticTransmitter.Login = userData.Login;
                LoadingUserData();
                LoadingDialogs();
                ModeCanMethod(AuthorizationMode.Paramount);
            }
            else UserLoginSignIn = "Неверные данные";
        } 
        #endregion

        #region Paramount
        public ICommand SendMessageCommand { get; }

        public bool CanSendMessageCommandExecute(object p) => true;

        public void OnSendMessageCommandExecuted(object p)
        {
            if (SelectedDialog == null)
                Message = "Выберите нужный вам диалог!";
            else
            {
                SelectedDialog.Messages.Add(new MessageModel
                {
                    Message = Message,
                    PathImage = UserProfile[0].PathImage,
                    Time = DateTime.Now,
                    IsNativeOrigin = true,
                    FirstMessage = true,
                    Username = UserProfile[0].Name,
                });

                var rsa = new RSAByOOS(Message);

                DatabaseManagement.SendMessage(SelectedDialog.Id, User[0].Id, rsa.EncryptString, rsa.PublicKeyString);
            }

            Message = "";
        }

        public ICommand DeleteDialogCommand { get; }

        public bool CanDeleteDialogCommandExecute(object p) => true;

        public void OnDeleteDialogCommandExecuted(object p)
        {
            StaticTransmitter.MessageText = $"Вы действительно хотите удалить {(p as DialogModel).Name}?";
            var messageWin = new MessageWindow();
            messageWin.ShowDialog();

            if (!messageWin.IsActive && StaticTransmitter.IsAnswer)
            {
                var itemToDel = Dialogs.Where(x => x.Id == (p as DialogModel).Id).First();
                var itemIndexToDel = Dialogs.IndexOf(itemToDel);
                Dialogs.RemoveAt(itemIndexToDel);
                DatabaseManagement.DeleteDialogByMaskId((p as DialogModel).Id);
            } 
        }

        public ICommand AddDialogCommand { get; }

        public bool CanAddDialogCommandExecute(object p) => true;

        public void OnAddDialogCommandExecuted(object p)
        {
            var addDialog = new AddDialogWindow();
            addDialog.ShowDialog();

            if (!addDialog.IsActive && StaticTransmitter.isCreationGroup)
            {
                var listNewDialogs = DatabaseManagement.GetDialogByMaskCode(StaticTransmitter.NameGroup, User[0].Id);
                if (listNewDialogs.Count.Equals(0))
                {
                    StaticTransmitter.MessageText = $"Беседы не существует!";
                    var messageWin = new MessageWindow();
                    messageWin.ShowDialog();
                }
                else Dialogs.Add(listNewDialogs[0]);
            }
        }

        public ICommand CreateDialogCommand { get; }

        public bool CanCreateDialogCommandExecute(object p) => true;

        public void OnCreateDialogCommandExecuted(object p)
        {
            var addDialog = new CreateDialogWindow();
            addDialog.ShowDialog();

            if (!addDialog.IsActive && StaticTransmitter.isCreationGroup)
            {
                var nameGroup = StaticTransmitter.NameGroup;
                DatabaseManagement.AddDialog(nameGroup, User[0].Id);
                
                var rsa = new RSAByOOS($"Я очутился тут...");

                var idDialog = DatabaseManagement.GetIdDialogByMaskName(nameGroup);

                DatabaseManagement.SendMessage(idDialog, User[0].Id, rsa.EncryptString, rsa.PublicKeyString);

                var listNewDialogs = DatabaseManagement.GetDialogByMaskName(nameGroup, User[0].Id);
                Dialogs.Add(listNewDialogs[0]);
            }
        }

        public ICommand UpdateDialogCommand { get; }

        public bool CanUpdateDialogCommandExecute(object p) => true;

        public void OnUpdateDialogCommandExecuted(object p)
        {
            var listNewDialogs = DatabaseManagement.GetDialogByMaskIdAndName(User[0].Id, UserProfile[0].Name);

            if (listNewDialogs.Count != Dialogs.Count)
            {
                Dialogs = listNewDialogs;
                SelectedDialog = null;
                ModeCanMethod(AuthorizationMode.LoadingLocation);
            }

            for (int i = 0; i < listNewDialogs.Count; i++)
            {
                if (listNewDialogs[i].Id == Dialogs[i].Id)
                {
                    Dialogs[i].Messages = listNewDialogs[i].Messages;
                }
            }
        }
        #endregion

        #region AllContact
        public ICommand AllContactShowCommand { get; }

        public bool CanAllContactShowCommandExecute(object p) => true;

        public void OnAllContactShowCommandExecuted(object p)
        {
            LoadingAllUser();
            ModeCanMethod(AuthorizationMode.AllContact);
        }

        public ICommand AddContactCommand { get; }

        public bool CanAddContactCommandExecute(object p) => true;

        public void OnAddContactCommandExecuted(object p)
        {
            var num = (int)p;
            var itemToAdd = Contacts.Where(x => x.Id == num).Select(x => x).First();
            var itemIndexToAdd = Contacts.IndexOf(itemToAdd);
            
            var name = RFC.GetHashPassword(Contacts[itemIndexToAdd].Name);
            DatabaseManagement.AddContact($"{name}", User[0].Id, Contacts[itemIndexToAdd].Id);
            var idDialog = DatabaseManagement.GetIdDialogByMaskName(name);
            Contacts[itemIndexToAdd].Id = idDialog;
            Dialogs.Add(Contacts[itemIndexToAdd]);

            var rsa = new RSAByOOS($"Бррр...");

            DatabaseManagement.SendMessage(idDialog, User[0].Id, rsa.EncryptString, rsa.PublicKeyString);
            ModeCanMethod(AuthorizationMode.Paramount);
        }
        #endregion

        #region Account and Settings
        public ICommand SwitchingPageCommand { get; }

        public bool CanSwitchingPageCommandExecute(object p) => true;

        public void OnSwitchingPageCommandExecuted(object p)
        {
            switch ((p as string))
            {
                case "AboutAccountProfile":
                    AboutAccountCurrent = new AboutAccountProfile();
                    break;

                case "AboutAccountTheme":
                    AboutAccountCurrent = new AboutAccountTheme();
                    break;

                case "AboutAccountDeleteProfile":
                    AboutAccountCurrent = new AboutAccountDeleteProfile();
                    break;

                case "ScreenSettingsMemory":
                    AboutSettingsCurrent = new ScreenSettingsMemory();
                    break;

                case "ScreenSettingsInformation":
                    AboutSettingsCurrent = new ScreenSettingsInformation();
                    break;

                case "ScreenSettingsSupport":
                    AboutSettingsCurrent = new ScreenSettingsSupport();
                    break;
            }
        }

        public ICommand ChangePhotoCommand { get; }

        public bool CanChangePhotoCommandExecute(object p) => true;

        public void OnChangePhotoCommandExecuted(object p)
        {
            var random = new Random();
            UserProfile[0].PathImage = DatabaseManagement.UpdatePhoto(UserProfile[0].Id, random.Next(1, 19));

        }

        public ICommand ExitFromAccountCommand { get; }

        public bool CanExitFromAccountCommandExecute(object p) => true;

        public void OnExitFromAccountCommandExecuted(object p)
        {
            UserLoginSignIn = "";
            Authorization = AuthorizationMode.SignIn;
        }

        public ICommand RenameNameCommand { get; }

        public bool CanRenameNameCommandExecute(object p) => true;

        public void OnRenameNameCommandExecuted(object p)
        {
            if (String.IsNullOrEmpty(NewName))
                NewName = "Строка не заполнена!";
            else
            {
                UserProfile[0].Name = NewName;
                DatabaseManagement.UpdateName(UserProfile[0].Id, NewName);
                NewName = "";
            }
        }

        public ICommand RenameStatCommand { get; }

        public bool CanRenameStatCommandExecute(object p) => true;

        public void OnRenameStatCommandExecuted(object p)
        {
            if (String.IsNullOrEmpty(NewStat))
                NewStat = "Строка не заполнена!";
            else
            {
                UserProfile[0].Stat = NewStat;
                DatabaseManagement.UpdateStat(UserProfile[0].Id, NewStat);
                NewStat = "";
            }
        }

        public ICommand RenameEmailCommand { get; }

        public bool CanRenameEmailCommandExecute(object p) => true;

        public void OnRenameEmailCommandExecuted(object p)
        {
            if (String.IsNullOrEmpty(NewEmail))
                NewEmail = "Строка не заполнена!";
            else
            {
                if (IsValidEmail(NewEmail))
                {
                    UserProfile[0].Email = NewEmail;
                    DatabaseManagement.UpdateEmail(UserProfile[0].Id, User[0].Id, NewEmail);
                    NewEmail = "";
                }
                else NewEmail = "Неверный формат!";
            }
        }

        public ICommand RenamePasswordCommand { get; }

        public bool CanRenamePasswordCommandExecute(object p) => true;

        public void OnRenamePasswordCommandExecuted(object p)
        {
            var passwordString = (p as PasswordBox).Password;
            if (String.IsNullOrEmpty(passwordString))
            {
                StaticTransmitter.MessageText = $"Строка не заполнена!";
                var messageWin = new MessageWindow();
                messageWin.ShowDialog();
            }
            
            else
            {
                DatabaseManagement.UpdatePassword(UserProfile[0].Id, RFC.GetHashPassword(passwordString));
                (p as PasswordBox).Password = "";
            }
        }

        public ICommand ChangeThemeCommand { get; }

        public bool CanChangeThemeCommandExecute(object p) => true;

        public void OnChangeThemeCommandExecuted(object p)
        {
            if ((p as string).Equals("true"))
            {
                StaticTransmitter.IsDark = true;
                DatabaseManagement.UpdateTheme(UserProfile[0].Id, 1);
                WindowTitle = "Ocean os souls";
                WindowBackgroundColor = "#FF3E3F40";
                WindowTitleBackgroundColor = "#252525";
                WindowForeground = "Gray";
                ContactBackgroundColor = "#2F3136";
                InformationBackgroundColor = "#292B2F";
                BorderBrushColor = "#FF9E9E9E";
                Chapters = "#FFEEEEEE";
            }

            else if ((p as string).Equals("false"))
            {
                StaticTransmitter.IsDark = false;
                DatabaseManagement.UpdateTheme(UserProfile[0].Id, 2);
                WindowTitle = "Ocean os souls";
                WindowBackgroundColor = "White";
                WindowTitleBackgroundColor = "#bfbfbf";
                WindowForeground = "#969696";
                ContactBackgroundColor = "#e6e6e6";
                InformationBackgroundColor = "#e3dcdc";
                BorderBrushColor = "#a8a8a8";
                Chapters = "#635d5d";
            }
        }

        public ICommand SubmitAcomplaintCommand { get; }

        public bool CanSubmitAcomplaintCommandExecute(object p) => true;

        public void OnSubmitAcomplaintCommandExecuted(object p)
        {
            if (String.IsNullOrEmpty(Complaint))
                Complaint = $"Строка не заполнена!";
            else
            {
                DatabaseManagement.AddComplaint(User[0].Id,Complaint);
                Complaint = $"Аккаунт в скором времени будет удален!";
            }
        }

        public ICommand TechnicalAcomplaintCommand { get; }

        public bool CanTechnicalAcomplaintCommandExecute(object p) => true;

        public void OnTechnicalAcomplaintCommandExecuted(object p)
        {
            if (String.IsNullOrEmpty(Technical))
                Technical = $"Строка не заполнена!";
            else
            {
                DatabaseManagement.AddComplaint(User[0].Id, Technical);
                Technical = "Отправлено!";
            }
        }

        public ICommand ClearApplicationCommand { get; }

        public bool CanClearApplicationCommandExecute(object p) => true;

        public void OnClearApplicationCommandExecuted(object p)
        {
            StaticTransmitter.MessageText = $"Приложение очищено!";
            var messageWin = new MessageWindow();
            messageWin.ShowDialog();
        }
        #endregion

        #endregion
    }
}
