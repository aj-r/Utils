namespace Sharp.Utils.Transactions
{
    /// <summary>
    /// Supports nested transactions. 
    /// </summary>
    public interface ITransactable
    {
        /// <summary>
        /// Increases the transaction level. This will start a transaction if one has not already started. 
        /// </summary>
        void IncreaseTransactionLevel();

        /// <summary>
        /// Decreases the transaction level. This will end a transaction the transaction level is back to its original value. 
        /// </summary>
        void DecreaseTransactionLevel();

        /// <summary>
        /// Gets whether a transaction is currently in progress for the current transactable object.
        /// </summary>
        /// <returns><value>true</value> if a transaction is in progress; otherwise <value>false</value>.</returns>
        bool TransactionInProgress { get; }
    }
}
