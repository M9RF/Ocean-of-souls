using System.Windows;
using Ocean_of_souls.Infrastructure.Commands.Base;

namespace Ocean_of_souls.Infrastructure.Commands.WindowTitle
{
    /// <summary>
    /// Minimizes an application to a window
    /// </summary>
    class RollIntoWindowApplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var mainWindow = (parameter as Window);

            if (mainWindow.WindowState.Equals(WindowState.Maximized))
                mainWindow.WindowState = WindowState.Normal;
            else mainWindow.WindowState = WindowState.Maximized;
        }
    }
}
