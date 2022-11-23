using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace KopForms
{   
    [ToolboxItem(true)]
    public partial class CheckedListBoxControl : UserControl
    {
        private event EventHandler SelectedElementChange;

        public CheckedListBoxControl()
        {
            InitializeComponent();
            checkedListBox.SelectedValueChanged += (sender, e)
                => { SelectedElementChange?.Invoke(sender, e); } ;
        }
        public void Add(string str)
        {
            checkedListBox.Items.Add(str);
        }
        public void Clear()
        {
            checkedListBox.Items.Clear();
        }
        public string CheckedItems
        {
            get
            {
                if (checkedListBox.CheckedItems == null)
                {
                    return  "";
                }
                else
                {
                    return string.Join(", ", checkedListBox.CheckedItems.OfType<string>());
                }
            }
            set 
            {
                if (value != null)
                {
                    var res = checkedListBox.CheckedItems.IndexOf(value);

                    if (res != -1)
                    {
                        checkedListBox.SetItemChecked(res, true);
                    }
                }
            }
        }

    }
}
