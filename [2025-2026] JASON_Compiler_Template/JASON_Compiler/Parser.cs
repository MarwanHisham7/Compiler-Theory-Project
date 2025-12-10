using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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

        //rule 31
        Node Program()
        {
            Node program = new Node("Program");

            program.Children.Add(Function_list());
            program.Children.Add(Main_Function());

            MessageBox.Show("Success");
            return program; ;
        }

        Node Function_list()
        {
            Node list = new Node("Function_List");

            if (InputPointer >= TokenStream.Count)
                return list;

            if (TokenStream[InputPointer].token_type == Token_Class.Main)
                return list;

            list.Children.Add(Function_statement());
            list.Children.Add(Function_list());
            return list;
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
            if (InputPointer >= TokenStream.Count)
                return block;
            // write your code here to match statements

            //block.Children.Add(match(Token_Class.Begin));
            block.Children.Add(Statements());
            //block.Children.Add(match(Token_Class.End));

            return block;
        }

        Node Statements()
        {
            Node s =new Node("Statements");

            if (InputPointer >= TokenStream.Count || TokenStream[InputPointer].token_type == Token_Class.End || TokenStream[InputPointer].token_type == Token_Class.ElseIf  || TokenStream[InputPointer].token_type == Token_Class.Else ||
                TokenStream[InputPointer].token_type == Token_Class.Until)
            {
                return s;
            }

            s.Children.Add(Statement());
            s.Children.Add(Statements());
            return s;
        }

        Node Statement()
        {
            Node s = new Node("Statement");

            if (InputPointer >= TokenStream.Count)
                return s;

            Token_Class ct = TokenStream[InputPointer].token_type;

            switch (ct)
            {
                case Token_Class.Int:
                case Token_Class.Float:
                case Token_Class.String:
                    s.Children.Add(Declaration_Statement());
                    break;

                // Assignment or Function Call (start with identifier)
                case Token_Class.Idenifier:
                    s.Children.Add(Statement_Start_With_Id());
                    break;

                // Read
                case Token_Class.Read:
                    s.Children.Add(Read_Statement());
                    break;

                // Write
                case Token_Class.Write:
                    s.Children.Add(Write_Statement());
                    break;

                // Return
                case Token_Class.Return:
                    s.Children.Add(Return_Statement());
                    break;

                // If
                case Token_Class.If:
                    s.Children.Add(IfStatement());
                    break;

                // Repeat
                case Token_Class.Repeat:
                    s.Children.Add(RepeatStatement());
                    break;

                default:
                    Errors.Error_List.Add($"Parsing Error: Unexpected token in Statement: {ct}");
                    InputPointer++;
                    break;
            }

            return s;

        }


        Node Statement_Start_With_Id()
        {
            Node stmt = new Node("ID_Statement");

            int lookahead = InputPointer + 1;

            if (lookahead < TokenStream.Count &&
                TokenStream[lookahead].token_type == Token_Class.AssignmentOp)
            {
                stmt.Children.Add(Assign_stmt());
                stmt.Children.Add(match(Token_Class.Semicolon));
            }
            else if (lookahead < TokenStream.Count &&
                     TokenStream[lookahead].token_type == Token_Class.left_parenthesis)
            {
                stmt.Children.Add(Function_Call());
                stmt.Children.Add(match(Token_Class.Semicolon));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Invalid identifier-based statement");
                InputPointer++;
            }

            return stmt;
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

        // Rule 3: Reserved Keywords
        Node ReservedKeyWord()
        {
            if (InputPointer >= TokenStream.Count)
                return null;

            Token_Class ct = TokenStream[InputPointer].token_type;
            Node n = null;

            switch (ct)
            {
                case Token_Class.Int:
                    n = new Node("int");
                    n.Children.Add(match(Token_Class.Int));
                    break;
                case Token_Class.Float:
                    n = new Node("float");
                    n.Children.Add(match(Token_Class.Float));
                    break;
                case Token_Class.String:
                    n = new Node("string");
                    n.Children.Add(match(Token_Class.String));
                    break;
                case Token_Class.Read:
                    n = new Node("read");
                    n.Children.Add(match(Token_Class.Read));
                    break;
                case Token_Class.Write:
                    n = new Node("write");
                    n.Children.Add(match(Token_Class.Write));
                    break;
                case Token_Class.Repeat:
                    n = new Node("repeat");
                    n.Children.Add(match(Token_Class.Repeat));
                    break;
                case Token_Class.Until:
                    n = new Node("until");
                    n.Children.Add(match(Token_Class.Until));
                    break;
                case Token_Class.If:
                    n = new Node("if");
                    n.Children.Add(match(Token_Class.If));
                    break;
                case Token_Class.ElseIf:
                    n = new Node("elseif");
                    n.Children.Add(match(Token_Class.ElseIf));
                    break;
                case Token_Class.Else:
                    n = new Node("else");
                    n.Children.Add(match(Token_Class.Else));
                    break;
                case Token_Class.Then:
                    n = new Node("then");
                    n.Children.Add(match(Token_Class.Then));
                    break;
                case Token_Class.Return:
                    n = new Node("return");
                    n.Children.Add(match(Token_Class.Return));
                    break;
                case Token_Class.Endl:
                    n = new Node("endl");
                    n.Children.Add(match(Token_Class.Endl));
                    break;
                default:
                    return null;
            }
            return n;
        }

        // Rule 4: Comment_Statement

        Node Comment_Statement()
        {
            Node comment = new Node("Comment_Statement");

            return comment;
        }

        //Rule 5: Identifiers
        Node Identifier()
        {
            Node id = new Node("Identifier");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                id.Children.Add(match(Token_Class.Idenifier));
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected Identifier");
            }
            return id;
        }
        Node Identifier_Tail()
        {
            Node tail = new Node("Identifier_Tail");
            return tail;
        }
        // Rule 6: Function Call
        Node Function_Call()
        {
            Node fun_call = new Node("Function_Call");


            fun_call.Children.Add(match(Token_Class.Idenifier));


            fun_call.Children.Add(match(Token_Class.left_parenthesis));


            fun_call.Children.Add(Argument_List());


            fun_call.Children.Add(match(Token_Class.right_parenthesis));

            return fun_call;
        }

        Node Argument_List()
        {
            Node argu = new Node("Argument_List");

            if (InputPointer >= TokenStream.Count)
                return argu;

            if (TokenStream[InputPointer].token_type == Token_Class.right_parenthesis)
                return argu;

            argu.Children.Add(Expression());

            argu.Children.Add(Argument_Tail());

            return argu;
        }

        Node Argument_Tail()
        {
            Node tail = new Node("Argument_Tail");

            if (InputPointer >= TokenStream.Count)
                return tail;

            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                tail.Children.Add(match(Token_Class.Comma));
                tail.Children.Add(Expression());
                tail.Children.Add(Argument_Tail());
            }

            return tail;
        }

        //Rule 7: term
        public Node Term()
        {
            //Term → number | identifier | function_call
            Node term = new Node("Term");
            if (InputPointer < TokenStream.Count)
            {
                //if (TokenStream[InputPointer].token_type == Token_Class.Digit)
                //{
                //    term.Children.Add(match(Token_Class.Digit));
                //    return term;
                //}
                if (TokenStream[InputPointer].token_type == Token_Class.Number)
                {


                    term.Children.Add(match(Token_Class.Number));
                    return term;
                }

                else if (TokenStream[InputPointer].token_type == Token_Class.Identifier) //AMBUGUITY
                {
                    ++InputPointer;
                    if (InputPointer < TokenStream.Count)
                    {
                        if (TokenStream[InputPointer].token_type == Token_Class.left_parenthesis)
                        {
                            --InputPointer;
                            term.Children.Add(Function_Call());
                            return term;
                        }
                    }

                    --InputPointer;
                    term.Children.Add(match(Token_Class.Identifier));
                    return term;

                }
                else
                {
                    printError("Term");
                }
            }

            return term;

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
        // Rule 9: Equation
        Node Equation()
        {
            Node equation = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    equation.Children.Add(match(Token_Class.LParanthesis));
                    equation.Children.Add(Equation());
                    equation.Children.Add(match(Token_Class.RParanthesis));
                }
                else
                {
                    equation.Children.Add(Term());
                    equation.Children.Add(Equation_Tail());
                }
            }
            return equation;
        }

        Node Equation_Tail()
        {
            Node equationTail = new Node("Equation_Tail");
            if (IsArithmeticOperator())
            {
                equationTail.Children.Add(Arithmetic_Operator());
                if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    equationTail.Children.Add(match(Token_Class.LParanthesis));
                    equationTail.Children.Add(Equation());
                    equationTail.Children.Add(match(Token_Class.RParanthesis));
                }
                else
                {
                    equationTail.Children.Add(Term());
                    equationTail.Children.Add(Equation_Tail());
                }
            }
            return equationTail;
        }
        // Rule 10: expression
        public Node Expression()  //Expression → string | Term | Equation 
        {
            Node expression = new Node("Expression");
            if (InputPointer >= TokenStream.Count)
                return expression;
            if (InputPointer < TokenStream.Count)
            {
                bool isString = (TokenStream[InputPointer].token_type == Token_Class.String);
                bool isNumber = (TokenStream[InputPointer].token_type == Token_Class.Number);
                bool isIdnetifier = (TokenStream[InputPointer].token_type == Token_Class.Identifier);
                //bool isDigit = (TokenStream[InputPointer].token_type == Token_Class.Digit);

                if (isString)
                {
                    expression.Children.Add(match(Token_Class.String));
                    return expression;
                }
                if (isNumber || isIdnetifier) //term or equation
                {
                    ++InputPointer;
                    if (InputPointer < TokenStream.Count)
                    {
                        bool isPlusOp = (TokenStream[InputPointer].token_type == Token_Class.PlusOp);
                        bool isMinusOp = (TokenStream[InputPointer].token_type == Token_Class.MinusOp);
                        bool isMultiplyOp = (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp);
                        bool isDivideOp = (TokenStream[InputPointer].token_type == Token_Class.DivideOp);

                        if (isPlusOp || isMinusOp || isMultiplyOp || isDivideOp)
                        {
                            --InputPointer;
                            expression.Children.Add(Equation());
                            return expression;
                        }
                    }
                    --InputPointer;
                    expression.Children.Add(Term());
                    return expression;
                }
                else
                    printError("Expression");

            }
            return expression;
        }

        private void printError(string v)
        {
            throw new NotImplementedException();
        }

        // Rule 11: assign statement
        public Node Assign_stmt()  
        {
            Node ass_stmt = new Node("Assigment stmt");
            if (InputPointer >= TokenStream.Count)
                return ass_stmt;
            ass_stmt.Children.Add(match(Token_Class.Identifier));
            ass_stmt.Children.Add(match(Token_Class.AssignmentOp));
            ass_stmt.Children.Add(Expression());
            return ass_stmt;
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

        // Rule 13: Declaration_Statement
        Node Declaration_Statement()
        {
            Node decl = new Node("Declaration_Statement");

            decl.Children.Add(Datatype());
            decl.Children.Add(Identifier_List());
            decl.Children.Add(match(Token_Class.Semicolon));

            return decl;
        }

        Node Identifier_List()
        {
            Node list = new Node("Identifier_List");

            list.Children.Add(Identifier_Item());
            list.Children.Add(Identifier_List_Tail());

            return list;
        }

        Node Identifier_List_Tail()
        {
            Node tail = new Node("Identifier_List_Tail");

            if (InputPointer >= TokenStream.Count)
                return tail;

            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                tail.Children.Add(match(Token_Class.Comma));
                tail.Children.Add(Identifier_Item());
                tail.Children.Add(Identifier_List_Tail());
            }

            return tail;
        }

        Node Identifier_Item()
        {
            Node item = new Node("Identifier_Item");

            item.Children.Add(match(Token_Class.Idenifier));
            item.Children.Add(Assignment_Opt());

            return item;
        }

        Node Assignment_Opt()
        {
            Node opt = new Node("Assignment_Opt");

            if (TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)
            {
                opt.Children.Add(match(Token_Class.AssignmentOp));
                opt.Children.Add(Expression());
            }

            return opt;
        }

        // Rule 14: Write Statement
        Node Write_Statement()
        {
            Node w = new Node("Write_Statement");

            w.Children.Add(match(Token_Class.Write));
            w.Children.Add(Write_Content());
            w.Children.Add(match(Token_Class.Semicolon));

            return w;
        }

        Node Write_Content()
        {
            Node content = new Node("Write_Content");

            if (TokenStream[InputPointer].token_type == Token_Class.Endl)
                content.Children.Add(match(Token_Class.Endl));
            else
                content.Children.Add(Expression());

            return content;
        }
        // Rule 15: Read Statement
        Node Read_Statement()
        {
            Node r = new Node("Read_Statement");

            r.Children.Add(match(Token_Class.Read));
            r.Children.Add(match(Token_Class.Idenifier));
            r.Children.Add(match(Token_Class.Semicolon));

            return r;
        }
        // Rule 16: Return Statement
        Node Return_Statement()
        {
            Node ret = new Node("Return_Statement");

            ret.Children.Add(match(Token_Class.Return));
            ret.Children.Add(Expression());
            ret.Children.Add(match(Token_Class.Semicolon));

            return ret;
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
        // Rule 18: condition
        public Node Condition() 
        {
            Node cond = new Node("Condition");

            cond.Children.Add(match(Token_Class.Identifier));
            cond.Children.Add(Condition_Op());
            cond.Children.Add(Term());
            return cond;

        }

        private Node Condition_Op()
        {
            throw new NotImplementedException();
        }

        // Rule 19: Boolean Operator
        Node Boolean_Operator()
        {
            Node boolOp = new Node("Boolean_Operator");

            if (InputPointer < TokenStream.Count)
            {
                Token_Class currentToken = TokenStream[InputPointer].token_type;

                if (currentToken == Token_Class.AndOp)
                {
                    boolOp.Children.Add(match(Token_Class.AndOp));
                }
                else if (currentToken == Token_Class.OrOp)
                {
                    boolOp.Children.Add(match(Token_Class.OrOp));
                }
            }

            return boolOp;
        }

        // Helper method to check if current token is a boolean operator
        bool IsBooleanOperator()
        {
            if (InputPointer >= TokenStream.Count)
                return false;

            Token_Class currentToken = TokenStream[InputPointer].token_type;
            return currentToken == Token_Class.AndOp ||
                   currentToken == Token_Class.OrOp;
        }
        // Rule 20:Condition Statement 
        Node ConditionStatement()
        {
            Node cond = new Node("ConditionStatement");
            
            if (InputPointer >= TokenStream.Count)
                return cond;

            cond.Children.Add(Condition());
            cond.Children.Add(ConditionTail());
            return cond;
        }
        Node ConditionTail()
        {
            Node tail = new Node("ConditionTail");

            if (InputPointer >= TokenStream.Count)
                return tail;

            if (IsBooleanOperator())
            {
                tail.Children.Add(Boolean_Operator());
                tail.Children.Add(Condition());
                tail.Children.Add(ConditionTail());
            
            }

            return tail;

        }
        // Rule 21: If Statement
        Node IfStatement()
        {
            Node ifnode = new Node("IfStatement");
            ifnode.Children.Add(match(Token_Class.If));
            ifnode.Children.Add(ConditionStatement());
            ifnode.Children.Add(match(Token_Class.Then));
            ifnode.Children.Add(Block());
            ifnode.Children.Add(IfTail());
            ifnode.Children.Add(match(Token_Class.End));
            return ifnode;

        }
        Node IfTail()
        {
            Node tail = new Node("IfTail");
            if (InputPointer >= TokenStream.Count)
                return tail;
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.ElseIf)
            {
                tail.Children.Add(ElseIfStatement());
                tail.Children.Add(IfTail());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                tail.Children.Add(ElseStatement());
            }
            return tail;

        }
        // Rule 22: Else If Statement
        Node ElseIfStatement() 
        {
            Node elseifnode = new Node("ElseIfStatement");
            elseifnode.Children.Add(match(Token_Class.ElseIf));
            elseifnode.Children.Add(ConditionStatement());
            elseifnode.Children.Add(match(Token_Class.Then));
            elseifnode.Children.Add(Block());
            return elseifnode;
        }
        //Rule 23 :Else Statememnt
        Node ElseStatement() 
        {
            Node elsenode = new Node("ElseStatement");
            elsenode.Children.Add(match(Token_Class.Else));
            elsenode.Children.Add(Block());
            return elsenode;
        }
        // Rule 24 Repeat Statement
        Node RepeatStatement()
        {
            Node rep = new Node("RepeatStatement");
            if (InputPointer >= TokenStream.Count)
                return rep;
            rep.Children.Add(match(Token_Class.Repeat));
            rep.Children.Add(Block());
            rep.Children.Add(match(Token_Class.Until));
            rep.Children.Add(ConditionStatement());
            return rep;



        }
         

        // Rule 25: Function Name
        Node FunctionName()
        {
            Node fn = new Node("FunctionName");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                fn.Children.Add(Identifier());
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected Function Name (Identifier)");
            }
            return fn;
        }

        // Rule 26: Parameter
        Node Parameter()
        {
            Node param = new Node("Parameter");
            if (IsDatatype())
            {
                param.Children.Add(Datatype());
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected datatype in parameter");
                return param;
            }
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                param.Children.Add(Identifier());
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected identifier in parameter");
            }
            return param;
        }


        //Rule 27: function declare
        Node Function_Declaration()
        {
            Node func = new Node("Function Declaration");
            func.Children.Add(Datatype());
            func.Children.Add(FunctionName());
            func.Children.Add(match(Token_Class.left_parenthesis));
            func.Children.Add(Parameter_List());
            func.Children.Add(match(Token_Class.right_parenthesis));
            return func;

        }

         Node Parameter_List()
        {
            Node paramList = new Node("Parameter_List");

            if (InputPointer >= TokenStream.Count)
                return paramList;

            if (TokenStream[InputPointer].token_type == Token_Class.right_parenthesis)
                return paramList;

            paramList.Children.Add(Parameter());
            paramList.Children.Add(Parameter_Tail());
            return paramList;
        }

         Node Parameter_Tail()
        {
            Node paramTail = new Node("Parameter_Tail");

            if (InputPointer >= TokenStream.Count)
                return paramTail;

            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {

                paramTail.Children.Add(match(Token_Class.Comma));
                paramTail.Children.Add(Parameter());
                paramTail.Children.Add(Parameter_Tail());
            }
            

            return paramTail;
        }


        //rule 28 : function body
        Node Function_Body()
        {
            Node funcBody = new Node("Function_Body");
            funcBody.Children.Add(match(Token_Class.openBrac));
            funcBody.Children.Add(Block());
            funcBody.Children.Add(return_opt());
            funcBody.Children.Add(match(Token_Class.closeBrac));
            return funcBody;
        }

        Node return_opt()
        {
            Node r = new Node("Return_Opt");
            if (InputPointer >= TokenStream.Count)
                return r;

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                r.Children.Add(Read_Statement());
            }

            return r;
        }


        //29 Function statement
        Node Function_statement()
        {
            Node fstmt = new Node("Function_statement");
            fstmt.Children.Add(Function_Declaration());
            fstmt.Children.Add(Function_Body());
            return fstmt;
        }

        //rule 30: main
        Node Main_Function()
        {
            Node main = new Node("Main");
            main.Children.Add(Datatype());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.left_parenthesis));
            main.Children.Add(match(Token_Class.right_parenthesis));
            main.Children.Add(Function_Body());

            return main;


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
