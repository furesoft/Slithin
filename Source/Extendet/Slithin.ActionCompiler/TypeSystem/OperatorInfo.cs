using System.Collections;
using System.Collections.Generic;

namespace Slithin.ActionCompiler.TypeSystem
{
    public struct OperatorInfo : IEnumerable<string>
    {
        public List<string> InputTypes = new List<string>();
        public string OutputType;

        public OperatorInfo(string outputType)
        {
            OutputType = outputType;
        }

        public void Add(string inputType)
        {
            InputTypes.Add(inputType);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return InputTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}