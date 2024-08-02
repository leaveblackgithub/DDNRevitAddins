using System;

namespace DDNRevitAddins.General
{
    public class ExceptionToCancel:Exception
    {
        public ExceptionToCancel(string message) : base(message)
        {

        }
    }
}
