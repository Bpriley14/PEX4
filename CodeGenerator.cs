using CS426.node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            if (node.GetRwType().Text == "int")
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

        public override void OutAEqualNumComp(AEqualNumComp node)
        {
            WriteLine("\n\t// OutAEqualBoolComp Start");

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tbeq " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }
        public override void OutANotEqualNumComp(ANotEqualNumComp node)
        {

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tbne.un " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }

        public override void OutALessNumComp(ALessNumComp node)
        {

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tblt " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }

        public override void OutALessEqualNumComp(ALessEqualNumComp node)
        {

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tble " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }

        public override void OutAGreaterNumComp(AGreaterNumComp node)
        {

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tbgt " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }

        public override void OutAGreaterEqualNumComp(AGreaterEqualNumComp node)
        {

            string lblTrue = GenerateUniqueLabel("LABEL_TRUE");
            string lblContinue = GenerateUniqueLabel("LABEL_CONTINUE");

            WriteLine("\tbge " + lblTrue);
            WriteLine("\tldc.i4 0"); // Push 0 for false
            WriteLine("\tbr " + lblContinue);
            WriteLine(lblTrue + ":");
            WriteLine("\tldc.i4 1"); // Push 1 for true
            WriteLine(lblContinue + ":");
        }

        public override void CaseAIfStmt(AIfStmt node)
        {
            string lblTrue = GenerateUniqueLabel("LBL_TRUE");
            string lblFalse = GenerateUniqueLabel("LBL_FALSE");
            string lblContinue = GenerateUniqueLabel("LBL_CONTINUE");

            WriteLine("\n\t// Conditional Start");
            InAIfStmt(node);

            // some automatically generated code here that you shouldn't have to touch

            // This is the code for the conditional test  If you've done it correctly, it should generate appropriate code for the boolean expression
            // that will leave a 1 on the stack if the test evaluates to true after the code executes, 0 if it is false.  See bullet #16 in the ISIL reference.
            if (node.GetBoolExp() != null)
            {
                node.GetBoolExp().Apply(this);
            }

            //other boring syntax stuff I didn't need to touch

            //  Before I generate code to be executed if the test is true, need to generate the required control flow (bullet #17)
            if (node.GetStatements() != null)
            {
                WriteLine("\tbrtrue " + lblTrue);
                WriteLine("\tbr " + lblFalse);

                WriteLine("\t" + lblTrue + ":");

                node.GetStatements().Apply(this);  // generate the code for the true part by visiting further down the tree

                WriteLine("\tbr " + lblContinue);  // now that the true part is generated, branch on to the continuation
            }

            if (node.GetRBrace() != null)
            {
                node.GetRBrace().Apply(this);
            }

            // similarly, before I generate the code for the false branch, need to label the target as required above
            if (node.GetElseStmt() != null)
            {
                WriteLine("\t" + lblFalse + ":");
                node.GetElseStmt().Apply(this);  // generate the code for the false branch by visiting this node
            }

            WriteLine("\t" + lblContinue + ":");  // then provide the required continuation label
            WriteLine("\t// Conditional End\n");
            OutAIfStmt(node);
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

        public override void CaseALoopStmt(ALoopStmt node)
        {
            string lblTrue = GenerateUniqueLabel("LBL_TRUE");
            string lblContinue = GenerateUniqueLabel("LBL_Continue");

            WriteLine("\n\t// Loop Start");
            InALoopStmt(node);


            if (node.GetStatements() != null)
            {
                WriteLine("\t" + lblTrue + ":");

                //test code
                if (node.GetBoolExp() != null)
                {
                    node.GetBoolExp().Apply(this);
                }

                WriteLine("\tbrzero " + lblContinue);

                node.GetStatements().Apply(this);

                WriteLine("\tbr " + lblTrue);
            }

            if (node.GetRBrace() != null)
            {
                node.GetRBrace().Apply(this);
            }

            WriteLine("\t" + lblContinue + ":");
            WriteLine("\t// Loop End\n");
            OutALoopStmt(node);
        }
    }
}
