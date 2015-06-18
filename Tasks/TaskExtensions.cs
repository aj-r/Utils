using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Tasks
{
    /// <summary>
    /// Contains Task extension methods.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Returns a Task that waits for the specified Task to complete. If the Task does not complete in time, a TimeoutException is thrown.
        /// </summary>
        /// <param name="task">The task to await.</param>
        /// <param name="millisecondsDelay">The maximum number of millisends to wait for.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task WithTimeout(this Task task, int millisecondsDelay)
        {
            using (var cts = new CancellationTokenSource(millisecondsDelay))
            {
                await task.WithCancellation(cts.Token);
            }
        }

        /// <summary>
        /// Returns a Task that waits for the specified Task to complete. If the Task does not complete in time, a TimeoutException is thrown.
        /// </summary>
        /// <typeparam name="T">The type of te result produced by the task.</typeparam>
        /// <param name="task">The task to await.</param>
        /// <param name="millisecondsDelay">The maximum number of millisends to wait for.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<T> WithTimeout<T>(this Task<T> task, int millisecondsDelay)
        {
            using (var cts = new CancellationTokenSource(millisecondsDelay))
            {
                return await task.WithCancellation(cts.Token);
            }
        }

        /// <summary>
        /// Wraps the task in another task that has a cancellation token.
        /// </summary>
        /// <param name="task">The original task.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task that has a cancellation token.</returns>
        public static Task WithCancellation(this Task task, CancellationToken token)
        {
            return task.ContinueWith(t => t.GetAwaiter().GetResult(), token);
        }

        /// <summary>
        /// Wraps the task in another task that has a cancellation token.
        /// </summary>
        /// <typeparam name="T">The type of the result produced by the task.</typeparam>
        /// <param name="task">The original task.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task that has a cancellation token.</returns>
        public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken token)
        {
            return task.ContinueWith(t => t.GetAwaiter().GetResult(), token);
        }
    }
}
