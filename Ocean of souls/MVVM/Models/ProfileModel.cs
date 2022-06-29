using Ocean_of_souls.MVVM.Models.Base;

namespace Ocean_of_souls.MVVM.Models
{
    class ProfileModel : Model
    {
        private int _Id;

        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }

        private string _Email;

        public string Email
        {
            get => _Email;
            set => Set(ref _Email, value);
        }

        private string _Name;

        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private int _Id_photo;

        public int Id_photo
        {
            get => _Id_photo;
            set => Set(ref _Id_photo, value);
        }

        private string _PathImage;

        public string PathImage
        {
            get => _PathImage;
            set => Set(ref _PathImage, value);
        }

        private int _Id_stat;

        public int Id_stat
        {
            get => _Id_stat;
            set => Set(ref _Id_stat, value);
        }

        private string _Stat;

        public string Stat
        {
            get => _Stat;
            set => Set(ref _Stat, value);
        }

        private int _Id_theme;

        public int Id_theme
        {
            get => _Id_theme;
            set => Set(ref _Id_theme, value);
        }

        private bool _Theme;

        public bool Theme
        {
            get => _Theme;
            set => Set(ref _Theme, value);
        }
    }
}
