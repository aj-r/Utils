using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp.Utils.Tasks
{
    /// <summary>
    /// Contains extension methods for managing processes.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(this Process process)
        {
            return WaitForExitAsync(process, CancellationToken.None);
        }

        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != CancellationToken.None)
                cancellationToken.Register(() => tcs.TrySetCanceled());
            return tcs.Task;
        }
    }
}
