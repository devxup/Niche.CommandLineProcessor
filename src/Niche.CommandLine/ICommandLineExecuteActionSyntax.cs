using System;

namespace Niche.CommandLine
{
    /// <summary>
    /// Syntax for invoking an action after an options instance has been parsed
    /// </summary>
    /// <typeparam name="T">Type of options instance just configured.</typeparam>
    public interface ICommandLineExecuteActionSyntax<out T>
    {
        /// <summary>
        /// Do something useful with a properly configured option
        /// </summary>
        /// <param name="action">Action to invoke.</param>
        void Execute(Action<T> action);
    }

    /// <summary>
    /// An implementation of <see cref="ICommandLineExecuteActionSyntax{T}"/> that does nothing
    /// </summary>
    /// <typeparam name="T">Type of instance that was configured.</typeparam>
    public class NullCommandLineExecuteActionSyntax<T> : ICommandLineExecuteActionSyntax<T>
    {
        /// <summary>
        /// Do nothing at all with a properly configured option
        /// </summary>
        /// <param name="action">Action to ignore.</param>
        public void Execute(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Nothing
        }
    }
}
