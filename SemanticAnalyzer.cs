//.......Tasks.......................
//Literals:                              Completed, Tested
//Operand:                               Completed, Tested
//Parenthetical_exp:                     Completed, Tested
//Negation:                              Completed, Tested
//Term:                                  Completed, Tested
//Expression:                            Completed, Tested
//Constants:                             Completed, Tested
//Declarations:                          Almost Completed, Tested
//param_declarations:                    Almost Completed
//assignment:                            Completed, Tested
//Booleans:                              Completed, Tested
//num compare:                           Completed, Tested 
//call_params:                           Completed, Not tested
//funct_call:                            Completed, Not tested
//statement:                             Not Started
//statements:                            Not Started
//loop Statement:                        Completed, Tested
//if Statement:                          Completed, Tested
//.................................

using CS426.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CS426.analysis
{
    internal class SemanticAnalyzer : DepthFirstAdapter
    {
        //Global symbol Table
        // EX: function def, data types, constants

        Dictionary<string, Definition> globalSymbolTable = new Dictionary<string, Definition>();

        //local symbol table
        //ex: local variables
        Dictionary<string, Definition> localSymbolTable = new Dictionary<string, Definition>();

        //decorated parse tree
        Dictionary<Node, Definition> decoratedParseTree = new Dictionary<Node, Definition>();

        public void PrintWarning(Token t, String message)
        {
            Console.WriteLine("Line " + t.Line + ", Col" + t.Pos + ": " + message);
        }

        public override void InAFile(AFile node)
        {
            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "int";

            Definition strDefinition = new StringDefinition();
            strDefinition.name = "str";

            Definition floatDefinition = new NumberDefinition();
            floatDefinition.name = "float";

            globalSymbolTable.Add("int", intDefinition);
            globalSymbolTable.Add("str", strDefinition);
            globalSymbolTable.Add("float", floatDefinition);

            //Console.WriteLine("You just entered a program node");
        }

        // --------------------------------------------------------------
        // Literals
        // --------------------------------------------------------------
        public override void OutAIntLiteral(AIntLiteral node)
        {
            //create definition
            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "int";

            decoratedParseTree.Add(node, intDefinition);
        }
        public override void OutAStrLiteral(AStrLiteral node)
        {
            Definition strDefinition = new StringDefinition();
            strDefinition.name = "str";

            decoratedParseTree.Add(node, strDefinition);
        }
        public override void OutAFloatLiteral(AFloatLiteral node)
        {
            Definition floatDefinition = new NumberDefinition();
            floatDefinition.name = "float";

            decoratedParseTree.Add(node, floatDefinition);
        }

        // --------------------------------------------------------------
        // Operand
        // --------------------------------------------------------------
        public override void OutAVarOperand(AVarOperand node)
        {
            //name of id
            String varName = node.GetId().Text;

            Definition varDefinition;

            //test var name in symbol table
            if (!localSymbolTable.TryGetValue(varName, out varDefinition))
            {
                //Console.WriteLine(varName + " does not exist");
                PrintWarning(node.GetId(), varName + " does not exist!");
            }

            //test var def is variable
            else if (!(varDefinition is VariableDefinition))
            {
                PrintWarning(node.GetId(), varName + " is not a variable!");
            }
            else
            {
                VariableDefinition v = (VariableDefinition)varDefinition;

                //decorate node with var type
                decoratedParseTree.Add(node, v.variableType);
            }
        }
        public override void OutALitOperand(ALitOperand node)
        {
            Definition literalDef;

            if (!decoratedParseTree.TryGetValue(node.GetLiteral(), out literalDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else
            {
                decoratedParseTree.Add(node, literalDef);
            }
        }

        // --------------------------------------------------------------
        // parenthetical_exp
        // --------------------------------------------------------------
        public override void OutANoneParentheticalExp(ANoneParentheticalExp node)
        {
            Definition operandDef;

            if (!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else
            {
                decoratedParseTree.Add(node, operandDef);
            }
        }
        public override void OutASomeParentheticalExp(ASomeParentheticalExp node)
        {
            Definition expressionDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, expressionDefiniton);
            }
        }

        // --------------------------------------------------------------
        // negation
        // --------------------------------------------------------------
        public override void OutAPosNegation(APosNegation node)
        {
            Definition parentheticalexpDef;

            if (!decoratedParseTree.TryGetValue(node.GetParentheticalExp(), out parentheticalexpDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else
            {
                decoratedParseTree.Add(node, parentheticalexpDef);
            }
        }
        public override void OutANegNegation(ANegNegation node)
        {
            Definition parentheticalexpDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetParentheticalExp(), out parentheticalexpDefiniton))
            {
                //error printed already
            }
            else if (!(parentheticalexpDefiniton is NumberDefinition))
            {
                PrintWarning(node.GetMinusSign(), "Only a number can be negated!");
            }
            else
            {
                decoratedParseTree.Add(node, parentheticalexpDefiniton);
            }
        }

        // --------------------------------------------------------------
        // term
        // --------------------------------------------------------------
        public override void OutASingleTerm(ASingleTerm node)
        {
            Definition negationDef;

            if (!decoratedParseTree.TryGetValue(node.GetNegation(), out negationDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else
            {
                decoratedParseTree.Add(node, negationDef);
            }
        }
        public override void OutAMultTerm(AMultTerm node)
        {
            Definition termDef;
            Definition negationDef;

            if (!decoratedParseTree.TryGetValue(node.GetTerm(), out termDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetNegation(), out negationDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (termDef.GetType() != negationDef.GetType())
            {
                PrintWarning(node.GetStar(), "Cannot multiply " + termDef.name
                    + " by " + negationDef.name);
            }
            else if (!(termDef is NumberDefinition))
            {
                PrintWarning(node.GetStar(), "Cannot multiply something of type "
                    + termDef.name);
            }
            else
            {
                // Decorate ourselves (either expression2def or expression3def would work)
                decoratedParseTree.Add(node, termDef);
            }
        }
        public override void OutADivTerm(ADivTerm node)
        {
            Definition termDef;
            Definition negationDef;

            if (!decoratedParseTree.TryGetValue(node.GetTerm(), out termDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetNegation(), out negationDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (termDef.GetType() != negationDef.GetType())
            {
                PrintWarning(node.GetSlash(), "Cannot divide " + termDef.name
                    + " by " + negationDef.name);
            }
            else if (!(termDef is NumberDefinition))
            {
                PrintWarning(node.GetSlash(), "Cannot divide something of type "
                    + termDef.name);
            }
            else
            {
                // Decorate ourselves (either expression2def or expression3def would work)
                decoratedParseTree.Add(node, termDef);
            }
        }

        // --------------------------------------------------------------
        // Expressions
        // --------------------------------------------------------------
        public override void OutASoloExpression(ASoloExpression node)
        {
            Definition termDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetTerm(), out termDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, termDefiniton);
            }
        }
        public override void OutAMinusExpression(AMinusExpression node)
        {
            Definition expressionType;
            Definition termType;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetTerm(), out termType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expressionType.name != termType.name)
            {
                PrintWarning(node.GetMinusSign(), "Could not subtract " + expressionType.name
                    + " and " + termType.name);
            }
            else if (!(expressionType is NumberDefinition))
            {
                PrintWarning(node.GetMinusSign(), "Could not subtract something of type "
                    + expressionType.name);
            }
            else
            {
                decoratedParseTree.Add(node, expressionType);
            }
        }
        public override void OutAPlusExpression(APlusExpression node)
        {
            Definition expressionType;
            Definition termType;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetTerm(), out termType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expressionType.name != termType.name)
            {
                PrintWarning(node.GetPlusSign(), "Could not add " + expressionType.name
                    + " and " + termType.name);
            }
            else if (!(expressionType is NumberDefinition))
            {
                PrintWarning(node.GetPlusSign(), "Could not add something of type "
                    + expressionType.name);
            }
            else
            {
                decoratedParseTree.Add(node, expressionType);
            }
        }

        // --------------------------------------------------------------
        // Declarations
        // --------------------------------------------------------------
        public override void OutAVarDec(AVarDec node)
        {
            Definition typeDef;
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetRwType().Text, out typeDef))
            {
                PrintWarning(node.GetRwType(), "Type " + node.GetRwType().Text + " does not exist!");
            }
            else if (!(typeDef is TypeDefinition))
            {
                PrintWarning(node.GetRwType(), "Identifier " + node.GetRwType().Text + " is not a recognized data type");
            }
            else if (localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
            }
            else if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used as a constant");
            }
            else
            {
                VariableDefinition newVariableDefinition = new VariableDefinition();
                newVariableDefinition.name = node.GetId().Text;
                newVariableDefinition.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node.GetId().Text, newVariableDefinition);
            }
        }

        public override void OutASomeFunctDeclarations(ASomeFunctDeclarations node)
{
    Definition idDef;
    Definition typeDef;
    if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
    {
        PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
        return;
    }

    
    localSymbolTable = new Dictionary<string, Definition>();

    
    FunctionDefinition newFunctionDefinition = new FunctionDefinition
    {
        name = node.GetId().Text,
        parameters = new List<VariableDefinition>()
    };

    
    var parameterList = new List<dynamic>();

    
    var paramDeclarations = node.GetParamDeclarations();
    if (paramDeclarations != null)
    {
        
        if (paramDeclarations is IEnumerable<dynamic>)
        {
            parameterList.AddRange((IEnumerable<dynamic>)paramDeclarations);
        }
        
        else
        {
            parameterList.Add(paramDeclarations);
        }

        
        foreach (var paramNode in parameterList)
        {
                var paramType = paramNode.GetType().Text;
                var paramName = paramNode.GetId().Text;

                if (!globalSymbolTable.TryGetValue(paramType, out typeDef))
                {
                    PrintWarning(paramNode.GetId(), "Type " + paramType + " does not exist!");
                }

                
                VariableDefinition parameterDefinition = new VariableDefinition
                {
                    name = paramName,
                    variableType = paramType
                };

                newFunctionDefinition.parameters.Add(parameterDefinition);
                localSymbolTable[paramName] = parameterDefinition;
        }
    }

    
    globalSymbolTable.Add(node.GetId().Text, newFunctionDefinition);
}

        public override void OutANoneFunctDeclarations(ANoneFunctDeclarations node)
        {
            base.OutANoneFunctDeclarations(node);
        }

        public override void OutASomeConstDeclarations(ASomeConstDeclarations node)
        {
            Definition typeDef;
            Definition idDef;
            Definition valueDef;
        
        
            if (!globalSymbolTable.TryGetValue(node.GetRwType().Text, out typeDef))
            {
                PrintWarning(node.GetId(), "Type " + node.GetType().ToString() + " does not exist!");
            }
            else if (!(typeDef is TypeDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetType().ToString() + " is not a recognized data type");
            }
            else if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression(), out valueDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            //else if (((VariableDefinition)idDef).variableType.name != valueDef.name)
            else if (typeDef.name != valueDef.name)
            {
                PrintWarning(node.GetId(), "Cannot assign value of type " + valueDef.name
                    + " to variable of type " + typeDef.name);
            }
             else
            {
                var newConstantDefinition = new ConstantDefinition(node.GetId().Text, (TypeDefinition)typeDef, node.GetType().ToString());
                globalSymbolTable.Add(node.GetId().Text, newConstantDefinition);
            }
        }
        
        // public override void OutASomeConstDeclarations(ASomeConstDeclarations node)
        // {
        //     Definition typeDef;
        //     Definition idDef;
        //     Definition valueDef;


        //     if (!globalSymbolTable.TryGetValue(node.GetId().Text, out typeDef))
        //     {
        //         PrintWarning(node.GetId(), "Type " + node.GetType().ToString() + " does not exist!");
        //     }
        //     else if (!(typeDef is TypeDefinition))
        //     {
        //         PrintWarning(node.GetId(), "Identifier " + node.GetType().ToString() + " is not a recognized data type");
        //     }
        //     else if (localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
        //     {
        //         PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
        //     }
        //     else
        //     {

        //         if (!localSymbolTable.TryGetValue(node.GetId().Text, out valueDef))
        //         {
        //             PrintWarning(node.GetId(), "Value " + node.GetId().Text + " is not declared");
        //         }
        //         else if (valueDef is VariableDefinition valueVariable)
        //         {

        //             if (valueVariable.variableType != typeDef)
        //             {
        //                 PrintWarning(node.GetId(), "Type mismatch: cannot assign "
        //                     + valueVariable.variableType.name + " to " + typeDef.name);
        //             }
        //             else
        //             {
        //                 var newConstantDefinition = new ConstantDefinition(node.GetId().Text, (TypeDefinition)typeDef, node.GetType().ToString());
        //                 localSymbolTable.Add(node.GetId().Text, newConstantDefinition);
        //             }
        //         }
        //         else
        //         {
        //             PrintWarning(node.GetId(), "Assigned value is not a recognized variable or constant");
        //         }
        //     }
        // }
        public override void OutANoneConstDeclarations(ANoneConstDeclarations node)
        {
            base.OutANoneConstDeclarations(node);
        }


        // --------------------------------------------------------------
        // Param Declarations
        // --------------------------------------------------------------
        public override void OutAParamDeclaration(AParamDeclaration node)
        {
            Definition typeDef;
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetRwType().Text, out typeDef))
            {
                PrintWarning(node.GetRwType(), "Type " + node.GetRwType().Text + " does not exist!");
            }
            else if (!(typeDef is TypeDefinition))
            {
                PrintWarning(node.GetRwType(), "Identifier " + node.GetRwType().Text + " is not a recognized data type");
            }
            else if (localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
            }
            else
            {
                VariableDefinition newVariableDefinition = new VariableDefinition();
                newVariableDefinition.name = node.GetId().Text;
                newVariableDefinition.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node.GetId().Text, newVariableDefinition);
            }
        }
        public override void OutASomeParamDeclarations(ASomeParamDeclarations node)
        {
            Definition idDef;

            if (globalSymbolTable.TryGetValue(node.GetParamDeclarations().ToString(), out idDef))
            {
                Console.WriteLine("Invalid Parameter: " + idDef);
            }
            else
            {
                //Wipes out the local symbol table
                localSymbolTable = new Dictionary<string, Definition>();

                //Registers the New Function Definition in the Global Table
                FunctionDefinition newFunctionDefinition = new FunctionDefinition();
                newFunctionDefinition.name = node.GetParamDeclarations().ToString();

                // TODO:  You will have to figure out how to populate this with parameters
                // when you work on PEX 3
                newFunctionDefinition.parameters = new List<VariableDefinition>();

                // Adds the Function!
                globalSymbolTable.Add(node.GetParamDeclarations().ToString(), newFunctionDefinition);
            }
        } //Needs implimentation

        public override void OutAOneParamDeclarations(AOneParamDeclarations node)
        {

            Definition idDef;

            if (globalSymbolTable.TryGetValue(node.GetParamDeclaration().ToString(), out idDef))
            {
                Console.WriteLine("Invalid Parameter: " + idDef);
            }
            else
            {
                //Wipes out the local symbol table
                localSymbolTable = new Dictionary<string, Definition>();

                //Registers the New Function Definition in the Global Table
                FunctionDefinition newFunctionDefinition = new FunctionDefinition();
                newFunctionDefinition.name = node.GetParamDeclaration().ToString();

                // Adds the Function!
                globalSymbolTable.Add(node.GetParamDeclaration().ToString(), newFunctionDefinition);
            }
        }
        public override void OutANoneParamDeclarations(ANoneParamDeclarations node)
        {
            base.OutANoneParamDeclarations(node);

            // No parameters to validate
        }


        // --------------------------------------------------------------
        // Assignment
        // --------------------------------------------------------------
        public override void OutAAssignment(AAssignment node)
        {
            Definition idDef;
            Definition expressionDef;

             if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
             {
                 PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used as a constant, value is final");
             }
            else if (!localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " does not exist");
            }
            else if (!(idDef is VariableDefinition))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " is not a variable");
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (((VariableDefinition)idDef).variableType.name != expressionDef.name)
            {
                PrintWarning(node.GetId(), "Cannot assign value of type " + expressionDef.name
                    + " to variable of type " + ((VariableDefinition)idDef).variableType.name);
            }
            else
            {
                // NOTHING IS REQUIRED ONCE ALL THE TESTS HAVE PASSED
            }
        }

        // --------------------------------------------------------------
        // Booleans
        // --------------------------------------------------------------
        public override void OutANoneBoolParens(ANoneBoolParens node)
        {
            Definition numCompDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetNumComp(), out numCompDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, numCompDefiniton);
            }
        }

        public override void OutASomeBoolParens(ASomeBoolParens node)
        {
            Definition boolExpDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolExp(), out boolExpDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolExpDefiniton);
            }
        }

        public override void OutAEqualBoolComp(AEqualBoolComp node)
        {
            Definition boolCompType;
            Definition boolParensType;

            if (!decoratedParseTree.TryGetValue(node.GetBoolComp(), out boolCompType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetBoolParens(), out boolParensType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (boolCompType.name != boolParensType.name)
            {
                PrintWarning(node.GetEqSign(), "Could not compare " + boolCompType.name
                    + " and " + boolParensType.name);
            }
            else if (!(boolCompType is NumberDefinition))
            {
                PrintWarning(node.GetEqSign(), "Could not compare something of type "
                    + boolCompType.name);
            }
            else
            {
                decoratedParseTree.Add(node, boolCompType);
            }
        }

        public override void OutANotEqualBoolComp(ANotEqualBoolComp node)
        {
            Definition boolCompType;
            Definition boolParensType;

            if (!decoratedParseTree.TryGetValue(node.GetBoolComp(), out boolCompType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetBoolParens(), out boolParensType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (boolCompType.name != boolParensType.name)
            {
                PrintWarning(node.GetNeqSign(), "Could not compare " + boolCompType.name
                    + " and " + boolParensType.name);
            }
            else if (!(boolCompType is NumberDefinition))
            {
                PrintWarning(node.GetNeqSign(), "Could not compare something of type "
                    + boolCompType.name);
            }
            else
            {
                decoratedParseTree.Add(node, boolCompType);
            }
        }

        public override void OutASoloBoolComp(ASoloBoolComp node)
        {
            Definition boolParensDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolParens(), out boolParensDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolParensDefiniton);
            }
        }

        public override void OutANegBoolNot(ANegBoolNot node)
        {
            Definition boolCompDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolComp(), out boolCompDefiniton))
            {
                //error printed already
            }
            else if (!(boolCompDefiniton is NumberDefinition))
            {
                PrintWarning(node.GetNotSign(), "Only a number or expression can be negated, no strings!");
            }
            else
            {
                decoratedParseTree.Add(node, boolCompDefiniton);
            }
        }

        public override void OutAPosBoolNot(APosBoolNot node)
        {
            Definition boolCompDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolComp(), out boolCompDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolCompDefiniton);
            }
        }

        public override void OutAMultBoolTerm(AMultBoolTerm node)
        {
            Definition boolTermType;
            Definition boolNotType;

            if (!decoratedParseTree.TryGetValue(node.GetBoolTerm(), out boolTermType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetBoolNot(), out boolNotType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (boolTermType.name != boolNotType.name)
            {
                PrintWarning(node.GetAndSign(), "Could not & " + boolTermType.name
                    + " and " + boolNotType.name);
            }
            else if (!(boolTermType is NumberDefinition))
            {
                PrintWarning(node.GetAndSign(), "Could not & something of type "
                    + boolTermType.name);
            }
            else
            {
                decoratedParseTree.Add(node, boolTermType);
            }
        }

        public override void OutASingleBoolTerm(ASingleBoolTerm node)
        {
            Definition boolNotDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolNot(), out boolNotDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolNotDefiniton);
            }
        }

        public override void OutAMultBoolExp(AMultBoolExp node)
        {
            Definition boolExpType;
            Definition boolTermsType;

            if (!decoratedParseTree.TryGetValue(node.GetBoolExp(), out boolExpType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetBoolTerm(), out boolTermsType))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (boolExpType.name != boolTermsType.name)
            {
                PrintWarning(node.GetOrSign(), "Could not | " + boolExpType.name
                    + " and " + boolTermsType.name);
            }
            else if (!(boolExpType is NumberDefinition))
            {
                PrintWarning(node.GetOrSign(), "Could not | something of type "
                    + boolExpType.name);
            }
            else
            {
                decoratedParseTree.Add(node, boolExpType);
            }
        }

        public override void OutASingleBoolExp(ASingleBoolExp node)
        {
            Definition boolTermDefiniton;

            if (!decoratedParseTree.TryGetValue(node.GetBoolTerm(), out boolTermDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolTermDefiniton);
            }
        }

        // --------------------------------------------------------------
        // Num Compare
        // --------------------------------------------------------------
        public override void OutALessNumComp(ALessNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetLtSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetLtSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }
        public override void OutALessEqualNumComp(ALessEqualNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetLeqSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetLeqSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }

        public override void OutAGreaterNumComp(AGreaterNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetGtSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetGtSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }

        public override void OutAGreaterEqualNumComp(AGreaterEqualNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetGeqSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetGeqSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }

        public override void OutAEqualNumComp(AEqualNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetEqSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetEqSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }

        public override void OutANotEqualNumComp(ANotEqualNumComp node)
        {
            Definition expression1Type;
            Definition expression2Type;

            if (!decoratedParseTree.TryGetValue(node.GetLhs(), out expression1Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!decoratedParseTree.TryGetValue(node.GetRhs(), out expression2Type))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (expression1Type.name != expression2Type.name)
            {
                PrintWarning(node.GetNeqSign(), "Could not compare " + expression1Type.name
                    + " and " + expression2Type.name);
            }
            else if (!(expression1Type is NumberDefinition))
            {
                PrintWarning(node.GetNeqSign(), "Could not compare something of type "
                    + expression1Type.name);
            }
            else
            {
                decoratedParseTree.Add(node, expression1Type);
            }
        }

        // --------------------------------------------------------------
        // Call Parameters
        // --------------------------------------------------------------
        public List<Definition> callParameters = new List<Definition>();
        public override void OutASomeCallParams(ASomeCallParams node)
        {
            var expr = node.GetExpression();

            if (decoratedParseTree.TryGetValue(expr, out Definition expressionDef))
            {
                if (expressionDef is NumberDefinition || expressionDef is StringDefinition)
                {
                    callParameters.Add(expressionDef);
                }
                else
                {
                    Console.WriteLine("Invalid Parameter: " + expressionDef);
                }
            }
        }

        public override void OutAOneCallParams(AOneCallParams node)
        {
            Definition expressionDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!(expressionDef is NumberDefinition) || !(expressionDef is StringDefinition))
            {
                Console.WriteLine("Invalid Parameter: " + expressionDef);

            }
        }
        public override void OutANoneCallParams(ANoneCallParams node)
        {
            base.OutANoneCallParams(node);
        }

        // --------------------------------------------------------------
        // Fuction Calls
        // --------------------------------------------------------------
        public override void OutAFunctCall(AFunctCall node)
        {
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " not found");

            }
            else if (!(idDef is FunctionDefinition functionDef))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " is not a function");

            }
            else
            {
                var expectedParameters = functionDef.parameters;
                bool allParamsMatch = true;

                if (expectedParameters.Count != callParameters.Count)
                {
                    PrintWarning(node.GetId(), "Incorrect number of arguments in function call. Expected " + expectedParameters.Count + " but found " + callParameters.Count);
                    allParamsMatch = false;
                }
                else
                {
                    for (int i = 0; i < expectedParameters.Count; i++)
                    {
                        var expectedParam = expectedParameters[i];
                        var providedArg = callParameters[i];
                        var expectedType = expectedParam.GetType();
                        var actualType = providedArg.GetType();

                        if (expectedType != actualType)
                        {
                            PrintWarning(node.GetId(), $"Argument type mismatch at position {i + 1}. Expected " + $"{expectedType} but found {actualType}.");
                            allParamsMatch = false;
                            break;
                        }
                    }

                    if (allParamsMatch)
                    {

                        decoratedParseTree.Add(node, functionDef);
                    }
                }
            }
            callParameters.Clear();
        }


        // --------------------------------------------------------------
        // Statement
        // --------------------------------------------------------------
        public override void CaseADeclarationStatement(ADeclarationStatement node)
        {
            base.CaseADeclarationStatement(node);
        }
        public override void CaseAFunctCall(AFunctCall node)
        {
            base.CaseAFunctCall(node);
        }
        public override void CaseAAssignment(AAssignment node)
        {
            base.CaseAAssignment(node);
        }
        public override void CaseAIfStmt(AIfStmt node)
        {
            base.CaseAIfStmt(node);
        }
        public override void CaseALoopStmt(ALoopStmt node)
        {
            base.CaseALoopStmt(node);
        }

        // --------------------------------------------------------------
        // Statements
        // --------------------------------------------------------------
        public override void OutASomeStatements(ASomeStatements node)
        {
            foreach (var statement in node.GetStatements())
            {
                statement.Apply(this); // Process each statement node
            }
        } //Needs implimentation
        public override void CaseANoneStatements(ANoneStatements node)
        {
            // No statements to process, so nothing is needed here.
        }

        // --------------------------------------------------------------
        // Loop Statement
        // --------------------------------------------------------------

        public override void OutALoopStmt(ALoopStmt node)
        {
            Definition boolExpDefiniton;
        
            if (!decoratedParseTree.TryGetValue(node.GetBoolExp(), out boolExpDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolExpDefiniton);
            }
        }
        // public override void OutALoopStmt(ALoopStmt node)
        // {
        //     var exprNode = node.GetBoolExp();
        //     Definition exprType;


        //     if (localSymbolTable.TryGetValue(exprNode.ToString(), out exprType))
        //     {

        //         if (!(exprType is TypeDefinition typeDef && typeDef.name == "Boolean"))
        //         {
        //             //Needs tokens but i can't find any
        //             PrintWarning(node.GetRwWhile(), "Condition in while loop must be a Boolean expression");
        //         }
        //     }
        //     else
        //     {
        //         //needs tokens but I can't find any
        //         PrintWarning(node.GetRwWhile(), "Expression in while loop is not defined");
        //     }


        //     base.OutALoopStmt(node);
        // }

        // --------------------------------------------------------------
        // If Statement
        // --------------------------------------------------------------
        public override void OutAIfStmt(AIfStmt node)
        {
            Definition boolExpDefiniton;
        
            if (!decoratedParseTree.TryGetValue(node.GetBoolExp(), out boolExpDefiniton))
            {
                //error printed already
            }
            else
            {
                decoratedParseTree.Add(node, boolExpDefiniton);
            }
        }
        // public override void OutAIfStmt(AIfStmt node)
        // {

        //     var conditionNode = node.GetBoolExp();
        //     Definition conditionType;

        //     // Check if the condition expression type is in the symbol table
        //     if (localSymbolTable.TryGetValue(conditionNode.ToString(), out conditionType))
        //     {
        //         // Verify if the type is Boolean
        //         if (!(conditionType is TypeDefinition typeDef && typeDef.name == "Boolean"))
        //         {
        //             //needs tokens but I can't find any
        //             PrintWarning(node.GetRwIf(), "Condition in if statement must be a Boolean expression");
        //         }
        //     }
        //     else
        //     {
        //         //needs tokens but I can't find any
        //         PrintWarning(node.GetRwIf(), "Expression in if statement is not defined");
        //     }
        // }


    }
}
