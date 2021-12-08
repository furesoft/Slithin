namespace Slithin.ActionCompiler.TypeSystem
{
    public class Primitive
    {
        public Primitive(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}