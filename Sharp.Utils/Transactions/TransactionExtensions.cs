using System;

namespace Sharp.Utils.Transactions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ITransactable"/> interface.
    /// </summary>
    public static class TransactionExtensions
    {
        /// <summary>
        /// Begins a transaction on the specified object. The transaction will end when the return value of this method is disposed.
        /// </summary>
        /// <param name="transactable">An <see cref="ITransactable"/> instance.</param>
        /// <returns>An object that represents the transaction. The transaction will end when the object is disposed.</returns>
        public static IDisposable BeginTransaction(this ITransactable transactable)
        {
            return new Transaction(transactable);
        }
    }
}
