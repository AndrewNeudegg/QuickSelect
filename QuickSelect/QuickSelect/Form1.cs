using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuickSelectControl;

namespace QuickSelect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            quickSelectPanel1.PathSelected += QuickSelectPanel1_PathSelected;
        }

        private void QuickSelectPanel1_PathSelected(string path)
        {
            swStopwatch.Stop();
            MessageBox.Show("You Selected: " + path);
            MessageBox.Show(((double)swStopwatch.ElapsedMilliseconds/1000).ToString());
        }
        Stopwatch swStopwatch = new Stopwatch();
        private void button1_Click(object sender, EventArgs e)
        {
            swStopwatch = new Stopwatch();
            swStopwatch.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            swStopwatch = new Stopwatch();
            swStopwatch.Start();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\andrew\Desktop\Folder\";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
            }
            swStopwatch.Stop();
            MessageBox.Show(((double)swStopwatch.ElapsedMilliseconds / 1000).ToString());
        }
    }
}
