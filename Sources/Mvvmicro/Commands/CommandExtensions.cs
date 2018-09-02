namespace Mvvmicro
{
    using System.Windows.Input;

    public static class CommandExtensions
    {
        /// <summary>
        /// Try to execute the command if CanExecute(parameter) is true.
        /// </summary>
        /// <returns><c>true</c>, if execute was tryed, <c>false</c> otherwise.</returns>
        /// <param name="command">The command.</param>
        /// <param name="parameter">The parameter.</param>
        public static bool TryExecute(this ICommand command, object parameter = null)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
