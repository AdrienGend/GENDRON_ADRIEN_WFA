using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GENDRON_ADRIEN_WFA
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent(); // Initialiser le composant
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1(); // Ouvrir le formulaire 1
            frm.Show(); 
            this.Hide(); 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Quitter l'application
        }
    }
}
