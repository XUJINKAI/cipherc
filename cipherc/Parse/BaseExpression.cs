using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CipherTool.Parse
{
    public abstract class BaseExpression : BaseUnit, IExpression
    {
        public IList<IDataPostfix> Postfixes { get; } = new List<IDataPostfix>();

        private Data? EvalCache { get; set; }

        public void ContinueParseWholeSentence(Parser parser)
        {
            ContinueParseExpression(parser, true);
            if (Postfixes.Count == 0)
            {
                Postfixes.Add(new DataTransformPostfix(DataFormat.Hex, DataSource.Arg));
            }
        }

        public void ContinueParseExpression(Parser parser, bool allowPostfixes)
        {
            Contract.Assert(parser != null);

            SelfParse(parser);

            if (allowPostfixes)
            {
                while (parser.CanPopToken<IDataPostfix>())
                {
                    var post = parser.PopInstance<IDataPostfix>();
                    post.ContinueParse(parser);
                    Postfixes.Add(post);
                }
            }
        }

        public void Execute(Parser parser)
        {
            Eval(parser);
        }

        public Data Eval(Parser parser)
        {
            if (EvalCache.HasValue)
            {
                return EvalCache.Value;
            }
            EvalCache = SelfEval(parser);

            foreach (var sub in Postfixes)
            {
                sub.Eval(parser, this);
            }
            return EvalCache.Value;
        }

        protected abstract void SelfParse(Parser parser);
        protected abstract Data SelfEval(Parser parser);
    }
}
