using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace JASON_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.TextChanged += TextBox1_TextChanged;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //string Code=textBox1.Text.ToLower();
            string Code = textBox1.Text;
            JASON_Compiler.Start_Compiling(Code);
            PrintTokens();
         //   PrintLexemes();

            PrintErrors();
        }
        void PrintTokens()
        {
            for (int i = 0; i < JASON_Compiler.Jason_Scanner.Tokens.Count; i++)
            {
               dataGridView1.Rows.Add(JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).lex, JASON_Compiler.Jason_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for(int i=0; i<Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            JASON_Compiler.TokenStream.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        /*  void PrintLexemes()
{
for (int i = 0; i < JASON_Compiler.Lexemes.Count; i++)
{
textBox2.Text += JASON_Compiler.Lexemes.ElementAt(i);
textBox2.Text += Environment.NewLine;
}
}*/

       ////////////////// // for gui
            private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            int selectionStart = textBox1.SelectionStart;
            int selectionLength = textBox1.SelectionLength;

            textBox1.SelectAll();
            textBox1.SelectionColor = Color.Black;

          
            HighlightWords(new string[] { "int", "float", "bool", "string", "if", "else", "while", "do", "begin", "end" }, Color.Blue);
            // for Numbers
            HighlightPattern(@"\b\d+(\.\d+)?\b", Color.Purple);
            //for  Strings
            HighlightPattern("\".*?\"", Color.Green);
            //  for Comments
            HighlightPattern(@"/\*.*?\*/", Color.Gray);

            textBox1.SelectionStart = selectionStart;
            textBox1.SelectionLength = selectionLength;
            textBox1.SelectionColor = Color.Black;
        }

        private void HighlightWords(string[] words, Color color)
        {
            foreach (string word in words)
            {
                HighlightPattern($@"\b{word}\b", color);
            }
        }

        private void HighlightPattern(string pattern, Color color)
        {
            var matches = Regex.Matches(textBox1.Text, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                textBox1.Select(m.Index, m.Length);
                textBox1.SelectionColor = color;
            }
        }


    }
}
