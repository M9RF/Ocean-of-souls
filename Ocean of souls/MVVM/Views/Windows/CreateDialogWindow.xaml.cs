using System.Windows;
using System.Windows.Input;

namespace Ocean_of_souls.MVVM.Views.Windows
{
    public partial class CreateDialogWindow : Window
    {
        public CreateDialogWindow()
        {
            InitializeComponent();
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
                DragMove();
        }
    }
}
