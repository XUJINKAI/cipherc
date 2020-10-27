using CipherTool.Tokenizer;

namespace CipherTool.Interpret
{
    public class Interpreter
    {
        public IContext Context { get; }

        public Interpreter(Setting setting)
        {
            Context = new Context(setting);
        }

        public void Interpret(string[] args)
        {
            var tokens = new TokenStream(args);
            var parser = new Parser(tokens);
            var ast = parser.BuildAst();
            var evaluator = new Evaluator(Context);
            ast.Accept(evaluator);
        }
    }
}
