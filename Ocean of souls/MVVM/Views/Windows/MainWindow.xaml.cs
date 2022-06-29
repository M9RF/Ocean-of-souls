using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Ocean_of_souls.MVVM.ViewModels;
using static Ocean_of_souls.MVVM.ViewModels.MainViewModel;

namespace Ocean_of_souls
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static readonly EnumConverter enumConverter = new EnumConverter(typeof(AuthorizationMode));

        private void OnAuthorizationMode(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                if (!(e.Parameter is AuthorizationMode mode))
                {
                    if (enumConverter.IsValid(e.Parameter))
                        mode = (AuthorizationMode)enumConverter.ConvertFrom(e.Parameter);
                    else
                        return;
                }
                vm.Authorization = mode;
            }
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
                DragMove();
        }
    }
}
