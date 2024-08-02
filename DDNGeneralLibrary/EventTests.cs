using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    /* [TestFixture]
     public class EventTests
     {
         public delegate void RunInCustomTransactionEventHandler(object sender, EventArgs e);
         public event RunInCustomTransactionEventHandler RunInCustomTransactionEvent;
         public string TestString;

         public void OnTransaction()
         {
             int i = 1;
             foreach (RunInCustomTransactionEventHandler handler in RunInCustomTransactionEvent.GetInvocationList())
             {
                 MessageBox.Show(i.ToString());
                 handler(this, new EventArgs());
                 i++;
             }
         }

         public void test1(object sender, EventArgs e)
         {
             MessageBox.Show("Test 1");
         }
         public void test2(object sender, EventArgs e)
         {
             MessageBox.Show("Test 2");
         }
         public void test3(object sender, EventArgs e)
         {
             MessageBox.Show("Test 3");
         }

         [Test]
         public void testEvents()
         {
             this.RunInCustomTransactionEvent += test1;
             this.RunInCustomTransactionEvent += test2;
             this.RunInCustomTransactionEvent += test3;
             OnTransaction();
         }*/

    /*        public IList<Delegate> DelegateList { get; set; }
            public delegate void DelegateHandler1(object sender, EventArgs e);
            public delegate void DelegateHandler2(object sender, EventArgs e);
            public void OnInvoke()
            {
                foreach (Delegate delegateHandler in DelegateList)
                {
                    if (delegateHandler is DelegateHandler1)
                    {
                        MessageBox.Show(nameof(DelegateHandler1));
                        delegateHandler.DynamicInvoke(new object[]{this,new EventArgs(), });
                        continue;
                    }
                    if (delegateHandler is DelegateHandler2)
                    {
                        MessageBox.Show(nameof(DelegateHandler2));
                        delegateHandler.DynamicInvoke(new object[] { this, new EventArgs(), });
                        continue;
                    }
                }

            }
            public void test1(object sender, EventArgs e)
            {
                MessageBox.Show("Test 1");
            }
            public void test2(object sender, EventArgs e)
            {
                MessageBox.Show("Test 2");
            }
            public void test3(object sender, EventArgs e)
            {
                MessageBox.Show("Test 3");
            }
            [Test]
            public void testEvents()
            {
                DelegateList = new List<Delegate>();
                this.DelegateList.Add((DelegateHandler1) test1);
                this.DelegateList.Add((DelegateHandler2)test2);
                this.DelegateList.Add((DelegateHandler2)test3);
                OnInvoke();
            }
        }*/
}
