using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Returns a Task that waits for the specified Task to complete. If the Task does not complete in time, a TimeoutException is thrown.
        /// </summary>
        /// <param name="task">The task to await.</param>
        /// <param name="millisecondsDelay">The maximum number of millisends to wait for.</param>
        /// <param name="cts">A <see cref="System.Threading.CancellationTokenSource"/> that can be used to cancel the task.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.TimeoutException" />
        public static async Task Timeout(this Task task, int millisecondsDelay, CancellationTokenSource cts = null)
        {
            if (cts == null)
                cts = new CancellationTokenSource();
            var resultTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, cts.Token)).ConfigureAwait(false);
            cts.Cancel();
            if (resultTask != task)
                throw new TimeoutException();
            await task;
        }

        /// <summary>
        /// Returns a Task that waits for the specified Task to complete. If the Task does not complete in time, a TimeoutException is thrown.
        /// </summary>
        /// <typeparam name="T">The type of te result produced by the task.</typeparam>
        /// <param name="task">The task to await.</param>
        /// <param name="millisecondsDelay">The maximum number of millisends to wait for.</param>
        /// <param name="cts">A <see cref="System.Threading.CancellationTokenSource"/> that can be used to cancel the task.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.TimeoutException" />
        public static async Task<T> Timeout<T>(this Task<T> task, int millisecondsDelay, CancellationTokenSource cts = null)
        {
            bool createdCts = false;
            if (cts == null)
            {
                cts = new CancellationTokenSource();
                createdCts = true;
            }
            try
            {
                var resultTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, cts.Token)).ConfigureAwait(false);
                if (resultTask != task)
                    throw new TimeoutException();
                return await task;
            }
            finally
            {
                // Cancel the original task or the delay task - whichever one did not finish
                cts.Cancel();
                // Dispose the CancellationTokenSource if this method created it.
                if (createdCts)
                    cts.Dispose();
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
