using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace JASON_Compiler
{
    // menna zakaria IMPLEMENTATION - Rules: 1, 2, 4, 8, 12, 17

    // marwan   IMPLEMENTATION - Rules: 3, 5, 19, 25, 26

    // anas  IMPLEMENTATION - Rules: 7, 9, 10, 11, 18

    //  yousef   IMPLEMENTATION - Rules: 6, 13, 14, 15, 16

    //  malalk   IMPLEMENTATION - Rules: 20, 21, 22, 23, 24

    //menna ezzat IMPLEMENTATION - Rules: 27, 28, 29, 30, 31

    public class Node
    {
        public List<Node> Children = new List<Node>();
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(Header());
            program.Children.Add(DeclSec());
            program.Children.Add(Block());
            program.Children.Add(match(Token_Class.Dot));
            MessageBox.Show("Success");
            return program;
        }
        Node Header()
        {
            Node header = new Node("Header");
            // write your code here to check the header sructure
            return header;
        }
        Node DeclSec()
        {
            Node declsec = new Node("DeclSec");
            // write your code here to check atleast the declare sturcure 
            // without adding procedures
            return declsec;
        }
        Node Block()
        {
            Node block = new Node("block");
            // write your code here to match statements
            return block;
        }

        
      
 

        // Rule 1: Number 
        Node Number()
        {
            Node number = new Node("Number");

            // Match the constant token (integer or float)
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Constant)
            {
                number.Children.Add(match(Token_Class.Constant));
            }

            return number;
        }

        // Rule 2: String
        Node String()
        {
            Node stringNode = new Node("String");

            
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.String)
            {
                stringNode.Children.Add(match(Token_Class.String));
            }

            return stringNode;
        }

        // Rule 4: Comment_Statement
        
        Node Comment_Statement()
        {
            Node comment = new Node("Comment_Statement");

            return comment;
        }

        // Rule 8: Arithmetic_Operator
        Node Arithmetic_Operator()
        {
            Node arithmeticOp = new Node("Arithmetic_Operator");

            if (InputPointer < TokenStream.Count)
            {
                Token_Class currentToken = TokenStream[InputPointer].token_type;

                if (currentToken == Token_Class.PlusOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.PlusOp));
                }
                else if (currentToken == Token_Class.MinusOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.MinusOp));
                }
                else if (currentToken == Token_Class.MultiplyOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.MultiplyOp));
                }
                else if (currentToken == Token_Class.DivideOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.DivideOp));
                }
            }

            return arithmeticOp;
        }

        // Helper method to check if current token is an arithmetic operator
        bool IsArithmeticOperator()
        {
            if (InputPointer >= TokenStream.Count)
                return false;

            Token_Class currentToken = TokenStream[InputPointer].token_type;
            return currentToken == Token_Class.PlusOp ||
                   currentToken == Token_Class.MinusOp ||
                   currentToken == Token_Class.MultiplyOp ||
                   currentToken == Token_Class.DivideOp;
        }

        // Rule 12: Datatype 
        Node Datatype()
        {
            Node datatype = new Node("Datatype");

            if (InputPointer < TokenStream.Count)
            {
                Token_Class currentToken = TokenStream[InputPointer].token_type;

                if (currentToken == Token_Class.Int)
                {
                    datatype.Children.Add(match(Token_Class.Int));
                }
                else if (currentToken == Token_Class.Float)
                {
                    datatype.Children.Add(match(Token_Class.Float));
                }
                else if (currentToken == Token_Class.String)
                {
                    datatype.Children.Add(match(Token_Class.String));
                }
            }

            return datatype;
        }

        // Helper method to check if current token is a datatype
        bool IsDatatype()
        {
            if (InputPointer >= TokenStream.Count)
                return false;

            Token_Class ct = TokenStream[InputPointer].token_type;
            return ct == Token_Class.Int ||
                   ct == Token_Class.Float ||
                   ct == Token_Class.String;
        }

        // Rule 17: Condition_Operator 
        Node Condition_Operator()
        {
            Node conditionOp = new Node("Condition_Operator");

            if (InputPointer < TokenStream.Count)
            {
                Token_Class currentToken = TokenStream[InputPointer].token_type;

                if (currentToken == Token_Class.LessThanOp)
                {
                    conditionOp.Children.Add(match(Token_Class.LessThanOp));
                }
                else if (currentToken == Token_Class.GreaterThanOp)
                {
                    conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
                }
                else if (currentToken == Token_Class.EqualOp)
                {
                    conditionOp.Children.Add(match(Token_Class.EqualOp));
                }
                else if (currentToken == Token_Class.NotEqualOp)
                {
                    conditionOp.Children.Add(match(Token_Class.NotEqualOp));
                }
            }

            return conditionOp;
        }

        // Helper method to check if current token is a condition operator
        bool IsConditionOperator()
        {
            if (InputPointer >= TokenStream.Count)
                return false;

            Token_Class currentToken = TokenStream[InputPointer].token_type;
            return currentToken == Token_Class.LessThanOp ||
                   currentToken == Token_Class.GreaterThanOp ||
                   currentToken == Token_Class.EqualOp ||
                   currentToken == Token_Class.NotEqualOp;
        }

       
        // Implement your logic here
        public Node match(Token_Class ExpectedToken)
        {
            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());
                    return newNode;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
