using Ocean_of_souls.MVVM.Models.Base;

namespace Ocean_of_souls.MVVM.Models
{
    class UserModel : Model
    {
        private int _Id;

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }

        private string _Login;

        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }

        private string _Password;

        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        private int _Id_profile;

        public int Id_profile
        {
            get => _Id_profile;
            set => Set(ref _Id_profile, value);
        }

        private int _Id_access_level;

        public int Id_access_level
        {
            get => _Id_access_level;
            set => Set(ref _Id_access_level, value);
        }

    }
}
