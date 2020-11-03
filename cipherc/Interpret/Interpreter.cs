using System;
using System.Text;
using CipherTool.Cli;
using CipherTool.Exceptions;
using CipherTool.Tokenizer;

namespace CipherTool.Interpret
{
    public class Interpreter
    {
        public IContext Context { get; }

        public Interpreter() : this(new Context()) { }

        public Interpreter(IContext ctx)
        {
            Context = ctx;
        }

        public void Interpret(string argLine)
        {
            var args = NativeMethods.CommandLineToArgs(argLine);
            Interpret(args);
        }

        public void Interpret(string[] args)
        {
            var tokens = new TokenStream(args);
            try
            {
                var parser = new Parser(Context, tokens);
                var ast = parser.BuildAst();
                var evaluator = new Evaluator(Context);
                ast.Accept(evaluator);
            }
            catch (UnexpectedTokenException unexpectedTokenException)
            {
                Context.WriteErrorLine(unexpectedTokenException.Message);
                if (Context.ThrowOnException) throw;
            }
            catch (ExpectedMoreTokenException expectedMoreTokenException)
            {
                Context.WriteErrorLine(expectedMoreTokenException.Message);
                if (Context.ThrowOnException) throw;
            }
            catch (NoPipeInputException noPipeInputException)
            {
                Context.WriteErrorLine(noPipeInputException.Message);
                if (Context.ThrowOnException) throw;
            }
            catch (Exception exception)
            {
                Context.WriteErrorLine(exception.Message);
                if (exception.StackTrace != null)
                    Context.WriteErrorLine(exception.StackTrace);
                if (Context.ThrowOnException) throw;
            }
        }
    }
}
