using System.Windows;
using Ocean_of_souls.Infrastructure.Commands.Base;

namespace Ocean_of_souls.Infrastructure.Commands.WindowTitle
{
    /// <summary>
    /// Minimizes the application
    /// </summary>
    class CollapseApplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => (parameter as Window).WindowState = WindowState.Minimized;
    }
}
