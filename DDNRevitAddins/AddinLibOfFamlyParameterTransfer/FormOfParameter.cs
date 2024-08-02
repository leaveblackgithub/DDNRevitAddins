using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDNRevitAddins.AddinLibOfFamlyParameterTransfer
{
    public partial class FormOfParameter : Form
    {
        public FormOfParameter()
        {
            InitializeComponent();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormOfParameter_Load(object sender, EventArgs e)
        {
        }

        public void GetFamilyDocumentsToCopyFrom(IList fileNames)
        {
            comboBox1.DataSource = fileNames;
        }
    }
}
