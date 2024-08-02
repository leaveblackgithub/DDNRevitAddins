using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNGeneralLibrary
{
    public static class DebugExtension
    {
        public static void PrintFieldValue(string methodName, string fieldName, string valueString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Method<");
            stringBuilder.Append(methodName);
            stringBuilder.Append(">:Field<");
            stringBuilder.Append(fieldName);
            stringBuilder.Append(">:Value<");
            stringBuilder.Append(valueString);
            stringBuilder.Append(">");
            Debug.Print(stringBuilder.ToString());
        }

        public static void PrintFieldValue(string methodName, string fieldName, double valueDouble)
        {
            PrintFieldValue(methodName, fieldName, valueDouble.ToString());
        }
        public static void PrintFieldValue<TKey, TValue>(string methodName, string fieldName, IDictionary<TKey,TValue>dictionary)
        {
            StringBuilder dictionaryValue = new StringBuilder();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                dictionaryValue.AppendLine();
                dictionaryValue.Append(pair.Key.ToString());
                dictionaryValue.Append(":");
                dictionaryValue.Append(pair.Value.ToString());
            }
            PrintFieldValue(methodName, fieldName,dictionaryValue.ToString());
        }


    }
}

