using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitUi();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetStatus("Compiling...");
            textBox2.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();

            string Code = textBox1.Text.ToLower();
            JASON_Compiler.Start_Compiling(Code);
            PrintTokens();
            treeView1.Nodes.Add(Parser.PrintParseTree(JASON_Compiler.treeroot));
            PrintErrors();
            SetStatus($"Parsed. Errors: {Errors.Error_List.Count}");
        }
        void PrintTokens()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.Tokens.Count; i++)
            {
               dataGridView1.Rows.Add(JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).lex, JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for(int i=0; i<Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i] + "\r\n";
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            JASON_Compiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            Errors.Error_List.Clear();
            SetStatus("Ready");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetStatus("Ready");
        }

        private void InitUi()
        {
            treeView1.ShowNodeToolTips = true;
            // Apply dark theme styling similar to the reference UI
            this.BackColor = Color.FromArgb(18, 22, 30);

            Font headerFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            Font monoFont = new Font("Consolas", 11F, FontStyle.Regular);
            Font bodyFont = new Font("Segoe UI", 11F, FontStyle.Regular);

            labelSource.Font = headerFont;
            labelTokens.Font = headerFont;
            labelParseTree.Font = headerFont;
            labelErrors.Font = headerFont;

            labelSource.ForeColor = Color.White;
            labelTokens.ForeColor = Color.White;
            labelParseTree.ForeColor = Color.White;
            labelErrors.ForeColor = Color.White;

            textBox1.Font = monoFont;
            textBox1.BackColor = Color.FromArgb(22, 30, 40);
            textBox1.ForeColor = Color.WhiteSmoke;
            textBox1.BorderStyle = BorderStyle.FixedSingle;

            textBox2.Font = bodyFont;
            textBox2.BackColor = Color.FromArgb(22, 30, 40);
            textBox2.ForeColor = Color.IndianRed;
            textBox2.BorderStyle = BorderStyle.FixedSingle;

            dataGridView1.BackgroundColor = Color.FromArgb(22, 30, 40);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(22, 30, 40);
            dataGridView1.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 90, 140);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 36, 48);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            treeView1.BackColor = Color.FromArgb(22, 30, 40);
            treeView1.ForeColor = Color.WhiteSmoke;
            treeView1.BorderStyle = BorderStyle.FixedSingle;
            treeView1.Font = bodyFont;

            statusStrip1.BackColor = Color.FromArgb(18, 22, 30);
            statusStrip1.ForeColor = Color.WhiteSmoke;
            statusLabel.ForeColor = Color.WhiteSmoke;

            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.FromArgb(46, 161, 155); // teal-ish
            button1.ForeColor = Color.White;
            button1.FlatAppearance.BorderSize = 0;
            button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);

            button2.FlatStyle = FlatStyle.Flat;
            button2.BackColor = Color.FromArgb(230, 125, 70); // orange-ish
            button2.ForeColor = Color.White;
            button2.FlatAppearance.BorderSize = 0;
            button2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);

            this.ForeColor = Color.WhiteSmoke;
            SetStatus("Ready");
        }

        private void SetStatus(string text)
        {
            if (statusLabel != null)
                statusLabel.Text = text;
        }
    }
}
