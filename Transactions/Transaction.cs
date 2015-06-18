using System;

namespace Utils.Transactions
{
    /// <summary>
    /// Provides a convenient way of scoping transactions with a using block.
    /// </summary>
    /// <remarks>
    /// A transaction is started when this object is created, and ended when this object is disposed.
    /// </remarks>
    internal class Transaction : IDisposable
    {
        private ITransactable transactable;

        /// <summary>
        /// Creates a new transaction and increments the transaction level on the specified object.
        /// </summary>
        internal Transaction(ITransactable transactable)
        {
            if (transactable == null)
                throw new ArgumentNullException("transactable");
            this.transactable = transactable;
            transactable.IncreaseTransactionLevel();
        }

        void IDisposable.Dispose()
        {
            transactable.DecreaseTransactionLevel();
        }
    }
}
