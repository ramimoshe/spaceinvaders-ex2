using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DreidelGame
{
    public partial class FormCameraProperties : Form
    {
        public FormCameraProperties()
        {
            InitializeComponent();
        }

        public object BoundObject
        {
            get { return propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }

        public void RefreshGrid()
        {
            propertyGrid1.Refresh();
        }
    }
}