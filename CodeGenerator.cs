using CS426.node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CS426.analysis
{
    class CodeGenerator : DepthFirstAdapter
    {
        StreamWriter _output;
        private int labelCounter = 0;

        public CodeGenerator(String outputFilename)
        {
            _output = new StreamWriter(outputFilename);
        }

        private void Write(String textToWrite)
        {
            Console.Write(textToWrite);
            _output.Write(textToWrite);
        }

        private void WriteLine(String textToWrite)
        {
            Console.WriteLine(textToWrite);
            _output.WriteLine(textToWrite);
        }

        public override void InAFile(AFile node)
        {
            WriteLine(".assembly extern mscorlib {}");
            WriteLine(".assembly toyprogram");
            WriteLine("{\n\t.ver 1:0:1:0\n}\n");
        }

        public override void OutAFile(AFile node)
        {
            _output.Close();
            Console.WriteLine("\n\n");
        }

        public override void InAMainDeclaration(AMainDeclaration node)
        {
            WriteLine(".method static void main() cil managed");
            WriteLine("{");
            WriteLine("\t.maxstack 128");
            WriteLine("\t.entrypoint\n");
        }

        public override void OutAMainDeclaration(AMainDeclaration node)
        {
            WriteLine("\n\tret");
            WriteLine("}");
        }

        public override void InAFunctDeclaration(AFunctDeclaration node)
        {
            WriteLine(".method static void " + node.GetId().Text + "() cil managed");
            WriteLine("{\n\t.maxstack 128\n");
        }

        public override void OutAFunctDeclaration(AFunctDeclaration node)
        {
            WriteLine("\n\tret");
            WriteLine("}\n");
        }

        public override void OutAVarDec(AVarDec node)
        {
            WriteLine("\t// Decalring Variable " + node.GetId().ToString());
            Write("\t.locals init (");

            if(node.GetRwType().Text == "int")
            {
                Write("int32 ");
            } 
            else if (node.GetRwType().Text == "float")
            {
                Write("float32 ");
            }
            else if (node.GetRwType().Text == "str")
            {
                Write("string ");
            }
            else
            {
                Write(node.GetRwType().Text + " ");
            }

            WriteLine(node.GetId().Text + ")\n");
        }

        public override void OutAIntLiteral(AIntLiteral node)
        {
           WriteLine("\tldc.i4 " + node.GetLitInteger().Text);
        }

        public override void OutAStrLiteral(AStrLiteral node)
        {
            WriteLine("\tldstr " + node.GetLitStr().Text);
        }

        public override void OutAFloatLiteral(AFloatLiteral node)
        {
            WriteLine("\tldc.r8 " + node.GetLitFloat().Text);
        }

        public override void OutAAssignment(AAssignment node)
        {
            WriteLine("\tstloc " + node.GetId().Text + "\n");
        }

        public override void OutAPlusExpression(APlusExpression node)
        {
            WriteLine("\tadd");
        }

        public override void OutAMultTerm(AMultTerm node)
        {
            WriteLine("\tmul");
        }

        public override void OutAMinusExpression(AMinusExpression node)
        {
            WriteLine("\tsub");
        }

        public override void OutADivTerm(ADivTerm node)
        {
            WriteLine("\tdiv");
        }

        public override void OutANegNegation(ANegNegation node)
        {
            WriteLine("\tneg");
        }

        public override void OutAVarOperand(AVarOperand node)
        {
            WriteLine("\tldloc " + node.GetId().Text);

        }

        public override void OutAFunctCall(AFunctCall node)
        {
            if (node.GetId().Text == "printInt")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(int32)");
            }
            else if (node.GetId().Text == "printString")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(string)");
            }
            else if (node.GetId().Text == "printFloat")
            {
                WriteLine("\tcall void [mscorlib]System.Console::Write(float32)");
            }
            else if (node.GetId().Text == "printLine")
            {
                WriteLine("\tldstr \"\\n\"");
                WriteLine("\tcall void [mscorlib]System.Console::Write(string)");
            }
            else
            {
                WriteLine("\tcall void " + node.GetId().Text + "()");
            }
        }

        private string GenerateUniqueLabel(string baseLabel)
        {
            return $"{baseLabel}_{labelCounter++}";
        }

        public override void OutAEqualBoolComp(AEqualBoolComp node)
        {
            string labelTrue = GenerateUniqueLabel("LABEL_TRUE");
            string labelContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tbeq " + labelTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + labelContinue);
            WriteLine(labelTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(labelContinue + ":");
        }


        public override void CaseAIfStatement(AIfStatement node)
        {
            string labelTrue = GenerateUniqueLabel("LABEL_TRUE");
            string labelFalse = GenerateUniqueLabel("LABEL_FALSE");
            string labelEnd = GenerateUniqueLabel("LABEL_END");


            // Branch based on the value on the stack
            WriteLine("\tbrtrue " + labelTrue); // If true, jump to LABEL_TRUE
            WriteLine("\tbr " + labelFalse); // otherwise, jump to LABEL_FALSE

            // Code for the "if true" block
            WriteLine("\t" + labelTrue + ":\n\t\t");
            node.GetIfStmt(); // Visit the "if" block
            WriteLine("\t\tbr " + labelEnd); // Jump to the end

            // End of the if/else statement
            WriteLine(labelEnd + ":");
        }

        public override void CaseAYesElseElseStmt(AYesElseElseStmt node)
        {
            string labelFalse = GenerateUniqueLabel("LABEL_FALSE");
            string labelEnd = GenerateUniqueLabel("LABEL_END");


            // Branch based on the value on the stack
            WriteLine("\tbrtrue " + labelFalse); // If true, jump to LABEL_FALSE
            WriteLine("\tbr " + labelEnd); // Otherwise, jump to LABEL_END

            // Code for the "else" block
            WriteLine(labelFalse + ":");
            node.GetRwElse(); // Visit the "else" block

            // End of the if/else statement
            WriteLine(labelEnd + ":");
        }
        
        public override void OutANegBoolNot(ANegBoolNot node)
        {
            WriteLine("\tldc.i4 0");
            WriteLine("\tceq");
        }
        
        public override void OutAMultBoolExp(AMultBoolExp node)
        {
            WriteLine("\tor");
        }
        
        public override void OutAMultBoolTerm(AMultBoolTerm node)
        {
            WriteLine("\tand");
        }

    }
}
