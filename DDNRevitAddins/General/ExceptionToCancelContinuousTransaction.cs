using System;

namespace DDNRevitAddins.General
{
    public class ExceptionToCancelContinuousTransaction:Exception
    {
        public ExceptionToCancelContinuousTransaction(string message) : base(message)
        {

        }

    }
}
