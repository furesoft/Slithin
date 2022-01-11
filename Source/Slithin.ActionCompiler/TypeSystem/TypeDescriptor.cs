using System.Collections.Generic;

namespace Slithin.ActionCompiler.TypeSystem
{
    public abstract class TypeDescriptor
    {
        public Dictionary<string, List<OperatorInfo>> Operators = new();

        public TypeDescriptor(string name, uint token)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; set; }
        public uint Token { get; set; }

        public OperatorInfo this[string op]
        {
            set
            {
                Add(op, value);
            }
        }

        public void Add(string op, OperatorInfo operatorInfo)
        {
            if (Operators.ContainsKey(op))
            {
                Operators[op].Add(operatorInfo);
            }
            else
            {
                var list = new List<OperatorInfo>
                {
                    operatorInfo
                };

                Operators.Add(op, list);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}