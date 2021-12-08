namespace Slithin.ActionCompiler.TypeSystem
{
    public class Primitive
    {
        public Primitive(string name, uint token)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; set; }
        public uint Token { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}