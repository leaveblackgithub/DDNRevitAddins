using Autodesk.Revit.DB;

namespace DDNRevitAddins.Archive.Environments
{
    public static class TransactionExtension
    {

        public static bool ExtCommit(this Transaction transaction)
        {
            if (!transaction.ExtHasStarted()) return false;
            
            return transaction.Commit()==TransactionStatus.Committed;
        }

        public static bool ExtHasStarted(this Transaction transaction)
        {
            return transaction.HasStarted();
        }

        public static bool ExtRollBack(this Transaction transaction)
        {
            if (!transaction.ExtHasStarted()) return false;
            return transaction.RollBack()==TransactionStatus.RolledBack;

        }
    }
}
