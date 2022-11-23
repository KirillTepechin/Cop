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

namespace KopForms
{
    public partial class TreeViewControl : UserControl
    {
        private List<string> _hierarhy;
        public void SetHierarhy(List<string> hierarhy)
        {
            _hierarhy = hierarhy;
        }
        
        public TreeViewControl()
        {
            InitializeComponent();
        }
        private int _selectedNodeIndex = 0;
        public int SelectedNodeIndex
        {
            get { return _selectedNodeIndex; }
            set { if(treeView.SelectedNode!=null)_selectedNodeIndex = treeView.SelectedNode.Index; }
        }
        public T GetSelectedValue<T>()
        {
            if (treeView.SelectedNode.Nodes.Count == 0)
            {
                T itemT = Activator.CreateInstance<T>();
                foreach(string prop in _hierarhy.Reverse<string>())
                {
                    string val = treeView.SelectedNode.Text;
                    var property = itemT.GetType().GetProperty(prop);

                    var propertyInfo = property;
                    var type = property?.PropertyType;
                    propertyInfo.SetValue(itemT, Convert.ChangeType(val, type));

                    if (treeView.SelectedNode.Parent!=null)
                    treeView.SelectedNode = treeView.SelectedNode.Parent;
                }
                return itemT;

            }
            else { return default; }
            
        }
       
        public void Add<T>(T obj)
        {
            var curNode = treeView.Nodes;
            
            foreach (string param in _hierarhy)
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(param);
                var value = propertyInfo.GetValue(obj, null).ToString();

                if (curNode.ContainsKey(value + _hierarhy.IndexOf(param)))
                {
                    TreeNode node = curNode.Find(value + _hierarhy.IndexOf(param), true)[0];
                    //curNode.Add(node);
                    curNode = node.Nodes;
                }
                else if (_hierarhy.IndexOf(param)==_hierarhy.Count-1)
                {
                    curNode.Add(value);
                }
                else
                {
                    TreeNode node = new TreeNode() { Name = value + _hierarhy.IndexOf(param), Text = value };
                    curNode.Add(node);
                    curNode = node.Nodes;
                }
            }

            
        }
        public void Clear()
        {
            treeView.Nodes.Clear();
        }
    }
    
}
