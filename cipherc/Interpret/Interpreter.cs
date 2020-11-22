using System;
using System.Diagnostics;
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

        private void _interpret(string[] args)
        {
            var tokens = new TokenStream(args);
            var parser = new Parser(Context, tokens);
            var ast = parser.BuildAst();

            var validator = new NodeValidator();
            ast.Accept(validator);
            if (validator.ValidationResults.Count > 0)
            {
                throw new ValidationException(validator.ValidationResults);
            }

            var evaluator = new Evaluator(Context);
            evaluator.Visit(ast);
        }

        public void Interpret(string[] args)
        {
#if false
            _interpret(args);
            return;
#endif

            try
            {
                _interpret(args);
            }
            catch (ValidationException validationException)
            {
                Context.WriteErrorLine("Parser Error.");
                validationException.ValidationResults.ForEach(r => Context.WriteErrorLine(r.ErrorMessage ?? "unknown error"));
                if (Context.ThrowOnException) throw;
            }
            catch (DecodeException decodeException)
            {
                Context.WriteErrorLine(decodeException.Message);
                if (Context.ThrowOnException) throw;
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
