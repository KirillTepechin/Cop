using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CopControlLibrary
{
    public partial class TextBoxControl : UserControl
    {
        public int? startRange;
        public int? endRange;
        private string error = string.Empty;
        private bool flagStart = false;
        private bool flagEnd = false;
        private event EventHandler TextChange;
        public TextBoxControl()
        {
            InitializeComponent();
            textBox.TextChanged += (sender, e)
                => {
                    TextChange?.Invoke(sender, e);
                };
        }

        public string Txt
        {
            get
            {
                if (IsCorrect())
                {
                    return textBox.Text;
                }
                else
                {
                    error = "Ошибка диапазона";
                    return null;
                }
                
            }
            set
            {
                if (IsCorrect()) textBox.Text = value;
            }
        }

        private bool IsCorrect()
        {
            return textBox.Text.Length >= StartRange && textBox.Text.Length <= EndRange;
        }

        public int StartRange
        {
            
            get
            {
                if (startRange == null)
                {
                    if (!flagStart)
                    {
                        MessageBox.Show("StartRange не определен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        flagStart = true;
                    }
                    return 0;
                }
                else
                {
                    return (int)startRange;
                }
            }
            set 
            {
                startRange = value;
            }
        }
        public int EndRange
        {
            get 
            {
                if (endRange == null)
                {
                    if (!flagEnd)
                    {
                        MessageBox.Show("EndRange не определен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        flagEnd = true;
                    }
                    return 0;
                }
                else
                {
                    return (int)endRange;
                }
            }
            set 
            { 
                endRange = value; 
            }
        }
    }
}
