using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
 
public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, Int,Float,String,Bool,AndOp, OrOp
}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("IF", Token_Class.If);
            ReservedWords.Add("BEGIN", Token_Class.Begin);
            ReservedWords.Add("CALL", Token_Class.Call);
            ReservedWords.Add("DECLARE", Token_Class.Declare);
            ReservedWords.Add("END", Token_Class.End);
            ReservedWords.Add("DO", Token_Class.Do);
            ReservedWords.Add("ELSE", Token_Class.Else);
            ReservedWords.Add("ENDIF", Token_Class.EndIf);
            ReservedWords.Add("ENDUNTIL", Token_Class.EndUntil);
            ReservedWords.Add("ENDWHILE", Token_Class.EndWhile);
            ReservedWords.Add("INTEGER", Token_Class.Integer);
            ReservedWords.Add("PARAMETERS", Token_Class.Parameters);
            ReservedWords.Add("PROCEDURE", Token_Class.Procedure);
            ReservedWords.Add("PROGRAM", Token_Class.Program);
            ReservedWords.Add("READ", Token_Class.Read);
            ReservedWords.Add("REAL", Token_Class.Real);
            ReservedWords.Add("SET", Token_Class.Set);
            ReservedWords.Add("THEN", Token_Class.Then);
            ReservedWords.Add("UNTIL", Token_Class.Until);
            ReservedWords.Add("WHILE", Token_Class.While);
            ReservedWords.Add("WRITE", Token_Class.Write);
            ReservedWords.Add("INT", Token_Class.Int);
            ReservedWords.Add("FLOAT", Token_Class.Float);
            ReservedWords.Add("STRING", Token_Class.String);
            ReservedWords.Add("BOOL", Token_Class.Bool);
         ////////////
            ReservedWords.Add("AND", Token_Class.AndOp);
            ReservedWords.Add("OR", Token_Class.OrOp);

            ////////////////////////////////////
            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
         
            Operators.Add("<>", Token_Class.NotEqualOp);


        }

        public void StartScanning(string SourceCode)
        {
            Tokens.Clear();
            Errors.Error_List.Clear();

            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = "";


                // ----------TaskName ->>  Numbers ----------
                //task1-> anas

                if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    CurrentLexeme += CurrentChar;
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                        while (char.IsDigit(CurrentChar) || CurrentChar == '.' || char.IsLetter(CurrentChar))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            j++;
                            if (j >= SourceCode.Length)
                                break;
                            CurrentChar = SourceCode[j];
                        }
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                    continue;
                }




                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;

                // ---------- TaskName ->> Identifiers or reserved Keywords ----------
                // task2 -> menna ezzat

                if (char.IsLetter(CurrentChar) || CurrentChar == '_')
                {
                    j = i;
                    string value = "";

                    while (j < SourceCode.Length && (char.IsLetterOrDigit(SourceCode[j]) || SourceCode[j] == '_'))
                    {
                        value += SourceCode[j];
                        j++;
                    }
                    if (ReservedWords.ContainsKey(value.ToUpper()))
                    {
                        FindTokenClass(value);
                    }
                    else if (isIdentifier(value))
                    {
                        Token Tok = new Token { lex = value, token_type = Token_Class.Idenifier };
                        Tokens.Add(Tok);
                    }
                    else
                    {
                        Errors.Error_List.Add($"Undefined Token: {value}");
                    }

                    i = j - 1;
                    continue;
                }
              
                else if (!char.IsWhiteSpace(CurrentChar) && !Operators.ContainsKey(CurrentChar.ToString()) && CurrentChar != '"')
                {
                    j = i;
                    string value = "";

                    while (j < SourceCode.Length &&
                           !char.IsWhiteSpace(SourceCode[j]) &&
                           !Operators.ContainsKey(SourceCode[j].ToString()))
                    {
                        value += SourceCode[j];
                        j++;
                    }

                    if (value.All(char.IsDigit))
                    {
                        Token Tok = new Token { lex = value, token_type = Token_Class.Constant };
                        Tokens.Add(Tok);
                    }
                    else
                    {
                        Errors.Error_List.Add($"Undefined Token: {value}");
                    }

                    i = j - 1;
                    continue;
                }



                // ---------- TaskName ->> Strings ----------
                //task3 -> malak

                if (CurrentChar == '"')
                {
                    j = i + 1;
                    string value = "\"";
                    while (j < SourceCode.Length && SourceCode[j] != '"')
                    {
                
                        if (SourceCode[j] == '\\' && j + 1 < SourceCode.Length)
                        {
                            value += SourceCode[j];    
                            j++;
                            value += SourceCode[j];    
                            j++;
                            continue;
                        }

                        value += SourceCode[j];
                        j++;
                    }

                    if (j < SourceCode.Length && SourceCode[j] == '"')
                    {
                        value += "\"";
                        i = j;             
                        FindTokenClass(value);
                        continue;            
                    }
                    else
                    {
                        Errors.Error_List.Add("STRING STARTED BUT NEVER CLOSED");
                        break;
                    }
                }


                // ---------- TaskName ->> Comments ----------
                //task4-> yousef

                if (CurrentChar == '/' && i + 1 < SourceCode.Length)
                {
                    char next = SourceCode[i + 1];

                    if (next == '/')
                    {
                        j = i + 2;
                        while (j < SourceCode.Length && SourceCode[j] != '\n')
                            j++;
                        i = j - 1;
                        continue;
                    }

                    if (next == '*')
                    {
                        j = i + 2;
                        bool closed = false;
                        while (j + 1 < SourceCode.Length)
                        {
                            if (SourceCode[j] == '*' && SourceCode[j + 1] == '/')
                            {
                                closed = true;
                                j += 2;
                                break;
                            }
                            j++;
                        }

                        if (!closed)
                        {
                            Errors.Error_List.Add("Comment started but never closed");
                            i = SourceCode.Length;
                            break;
                        }

                        i = j - 1;
                        continue;
                    }
                }


                // ----------  TaskName ->> Operators ----------
                //task5-> marwan

                string op = CurrentChar.ToString();
                bool opfound=false;
                for(int m=0;m<Operators.Count;m++)
                {
                    if(op== Operators.Keys.ElementAt(m))
                    {
                        Token optoken = new Token { lex = op, token_type = Operators[op] };
                        Tokens.Add(optoken);
                        opfound = true;
                        break;
                    }
                }                
                if(!opfound)
                    Errors.Error_List.Add($"Undefined Token: {CurrentChar}");        
             
            }
           

            // ---------- Helper Methods ----------
            void FindTokenClass(string Lex)
            {
                Token Tok = new Token();
                Tok.lex = Lex.Trim();

                if (ReservedWords.ContainsKey(Lex.ToUpper()))
                    Tok.token_type = ReservedWords[Lex.ToUpper()];

                else if (Operators.ContainsKey(Lex))

                    Tok.token_type = Operators[Lex];

                else if (isIdentifier(Lex))

                    Tok.token_type = Token_Class.Idenifier;

                else if (isConstant(Lex))
                {
                    if (Lex.Contains("."))  
                        Tok.token_type = Token_Class.Float;
                    else                     
                        Tok.token_type = Token_Class.Int;
                }

                else if (Regex.IsMatch(Lex, "^\"([^\"\\\\]|\\\\.)*\"$"))

                    Tok.token_type = Token_Class.String;
              

                else
                {
                    Errors.Error_List.Add($"Undefined Token: {Lex}");
                    return;
                }

                Tokens.Add(Tok);
            }

            bool isIdentifier(string lex)
            {
                string pattern = @"^[A-Za-z_][A-Za-z0-9_]*$";
                return Regex.IsMatch(lex, pattern);
            }

            bool isConstant(string lex)
            {

                string pattern = @"^[0-9]+(\.[0-9]+)?$";
                return Regex.IsMatch(lex, pattern);
            }
        }
    }
}
