using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchProject
{
    public partial class frmSearch : Form
    {

        Button[] btnButton = new Button[26];
        OleDbConnection conn;
        string sql = "SELECT a.Author, t.Title, p.Name "+
            "FROM Authors as a, Titles as t, Publishers as p, Title_Author as ta "+
            "Where a.AU_ID = ta.AU_ID "+
            "AND t.ISBN = ta.ISBN "+
            "AND t.PubID = p.PubID ";
        public frmSearch()
        {
            InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            int btnWidth, btnLeft, btnTop;
            int btnheight = 30;

            var connstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Alia\Desktop\Books.accdb;
            Persist Security Info = False; ";
            conn = new OleDbConnection(connstring);
            conn.Open();

            btnWidth = ClientSize.Width / 13;
            btnLeft = ClientSize.Width - (13 * btnWidth);
            btnTop = grdBooks.Top + grdBooks.Height + 30;

            for (int i = 0; i < 26; i++)
            {
                btnButton[i] = new Button();
                btnButton[i].Text = ((char)(65 + i)).ToString();
                btnButton[i].Width = btnWidth;
                btnButton[i].Height = btnheight;
                btnButton[i].Left = btnLeft;
                btnButton[i].Top = btnTop;

                Controls.Add(btnButton[i]);
                btnButton[i].Click += new EventHandler(btnSQL_Click);
                btnLeft += btnWidth;
                if (i == 12)
                {
                    btnLeft = ClientSize.Width - (13 * btnWidth);
                    btnTop += btnheight;
                }
            }

        }

        private void btnSQL_Click(Object sender, EventArgs e)
        {
            OleDbCommand command = null;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            DataTable table = new DataTable();

            Button clickedButton = (Button)sender;
            string sqlStatement;

            switch (clickedButton.Text)
            {
                case "Show All Records":
                    sqlStatement = sql;
                    break;
                default:
                    sqlStatement = sql + "AND a.Author like '" + clickedButton.Text + "%'";
                    break;
                   // Add another and to where cause - a.Author like clickedButton.Text                 
            }


            try
            {
                command = new OleDbCommand(sqlStatement, conn);
                adapter.SelectCommand = command;
                adapter.Fill(table);
                grdBooks.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            command.Dispose();
            adapter.Dispose();
            table.Dispose();
        }

        private void frmSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            conn.Dispose();
        }

        private void btnAllRecords_Click(object sender, EventArgs e)
        {

        }
    }
}
