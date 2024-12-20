using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS426.analysis
{
    public abstract class Definition
    {
        public string name;

        public string toString()
        { 
            return name;
        }
    }

    public abstract class TypeDefinition : Definition { }

    public class NumberDefinition : TypeDefinition { }

    public class StringDefinition : TypeDefinition { }

    public class BooleanDefinition : Definition { }

    public class VariableDefinition : Definition 
    {
        public TypeDefinition variableType;
    }

    public class  FunctionDefinition : Definition
    {
        public List<VariableDefinition> parameters;
    }

    public class ConstantDefinition : Definition
    {
        
        public string name { get; set; }

        
        public TypeDefinition constantType { get; set; }

        public object value { get; set; }

        
        public ConstantDefinition(string name, TypeDefinition constantType, object value)
        {
            this.name = name;
            this.constantType = constantType;
            this.value = value;
        }
    }

}
