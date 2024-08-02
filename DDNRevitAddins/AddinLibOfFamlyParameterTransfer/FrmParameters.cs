using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DDNRevitAddins.Archive.Environments;

namespace DDNRevitAddins.AddinLibOfFamlyParameterTransfer
{
    public partial class FrmParameters : System.Windows.Forms.Form
    {
        private IDictionary<string, Document> docs = new Dictionary<string, Document>();
        private Dictionary<string, FamilyParameter> parameters = new Dictionary<string, FamilyParameter>();
        private KeyValuePair<string, Document> selectedDoc = new KeyValuePair<string, Document>();
        private Dictionary<string, FamilyParameter> selectedParameters = new Dictionary<string, FamilyParameter>();
        private UIApplication app;

        public FrmParameters(UIApplication  appNew)
        {
            InitializeComponent();
            app = appNew;
        }
        public KeyValuePair<string, Document> SelectedDoc
        {
            get { return selectedDoc; }
        }
        public Dictionary<string, FamilyParameter> SelectedParameters
        {
            get { return selectedParameters; }
        }
        private void FrmParameter_Load(object sender, EventArgs e)
        {
        }

        public void GetOtherFamilyDocuments(IDictionary<string, Document> otherFamilyDocuments)
        {
            docs = otherFamilyDocuments;
            cmbDocs.DataSource = docs.Keys.ToList();
            cmbDocs.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var lstParametersSelectedItems = lstParameters.SelectedItems;
            if (lstParametersSelectedItems.Count == 0) return;
            foreach (object item in lstParametersSelectedItems)
            {
                selectedParameters.Add(item.ToString(), parameters[item.ToString()]);
            }
            this.Hide();
        }

        private void cmbDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadFamilyList();
        }

        private void loadFamilyList()
        {
            lstParameters.Items.Clear();
            string selectedDocName = cmbDocs.SelectedItem.ToString();
            selectedDoc = new KeyValuePair<string, Document>(selectedDocName, docs[selectedDocName]);
            parameters = selectedDoc.Value.ExtGetParameterDictionary();
            foreach (KeyValuePair<string, FamilyParameter> parameter in parameters)
            {
                lstParameters.Items.Add(parameter.Key);
            }
        }
    }
}
