using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using CipherTool.Cipher;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public class Parser
    {
        public ParseSetting Setting { get; }

        public TokenStream TokenStream { get; }

        public List<ISentenceRoot> Sentences { get; }

        public Dictionary<string, Data> GlobalStorage { get; }

        private Parser(string[] args, ParseSetting parseSetting)
        {
            Setting = parseSetting;
            TokenStream = new TokenStream(args);
            Sentences = new List<ISentenceRoot>();
            GlobalStorage = new Dictionary<string, Data>();
            ParseTokens();
        }

        private void ParseTokens()
        {
            TokenStream.Reset();
            Sentences.Clear();

            var s0 = PopInstance<ISentenceRoot>();
            s0.ContinueParseWholeSentence(this);
            Sentences.Add(s0);

            while (TokenStream.TryMoveNext())
            {
                PopType<ISpliter>();
                var s = PopInstance<ISentenceRoot>();
                s.ContinueParseWholeSentence(this);
                Sentences.Add(s);
            }
        }

        public void Evaluate() => Sentences.ForEach(s => s.Execute());

        public static Parser Eval(string[] args, ParseSetting? setting = null)
        {
            Contract.Assert(args != null);
            Contract.Assert(args.Length > 0);

            setting ??= new ParseSetting();
            var parser = new Parser(args, setting);
            parser.Evaluate();
            return parser;
        }

        public IDictionary<string, object> KeyTokenMap { get; } = new Dictionary<string, object>()
        {
            {"then", typeof(ISpliter) },
            {"from", typeof(DataExpression) },
            {"to", typeof(DataTransformPostfix) },
            {"rand", typeof(RandomExpression) },
            {"random", "rand" },
            {"sm3", typeof(HashExpression<SM3Hash>) },
            {"md5", typeof(HashExpression<MD5Hash>) },
            {"sha1", typeof(HashExpression<SHA1Hash>) },
            {"sha256", typeof(HashExpression<SHA256Hash>) },
            {"sm2", typeof(AsymObject) },
            {"sm4", typeof(SymObject) },
            {"get", typeof(GetOperator) },
            {"set", typeof(SetOperator) },
            {"enc", typeof(EncOperator) },
            {"dec", typeof(DecOperator) },
            {"sign", typeof(SignOperator) },
            {"check", typeof(SignCheckOperator) },
            {"plain", DataFormat.Plain },
            {"txt", "plain" },
            {"text", "plain" },
            {"hex", DataFormat.Hex },
            {"base64", DataFormat.Base64 },
            {"bin", DataFormat.Bin },
            {"pem", DataFormat.Pem },
            {"arg", DataSource.Arg },
            {"data", DataSource.Arg },
            {"file", DataSource.File },
            {"path", DataSource.File },
            {"pipe", DataSource.Pipe },
            {"stdin", DataSource.Pipe },
            {"ecb", CipherMode.Ecb },
            {"cbc", CipherMode.Cbc },
            {"gcm", CipherMode.Gcm },
        };

        public object? FindKeyWordObject(string s)
        {
            Contract.Assume(s != null);

            var find = s.ToLower(ENV.CultureInfo);
            if (KeyTokenMap.ContainsKey(find))
            {
                var value = KeyTokenMap[find];
                if (value is string v)
                {
                    return FindKeyWordObject(v);
                }
                return value;
            }
            else
            {
                return null;
            }
        }

        public string PopString()
        {
            var token = TokenStream.PopToken();
            return token.Raw;
        }

        public Enum PopEnum(out Token token)
        {
            token = TokenStream.PopToken();
            var obj = FindKeyWordObject(token.Raw);
            if (obj is Enum e)
            {
                return e;
            }
            throw new UnexpectedTokenException(token);
        }

        public T PopEnum<T>() where T : Enum
        {
            var token = TokenStream.PopToken();
            var obj = FindKeyWordObject(token.Raw);
            if (obj is T t)
            {
                return t;
            }
            throw new UnexpectedTokenException(token);
        }

        public Type PopType<T>() where T : IUnit
        {
            var token = TokenStream.PopToken();
            var obj = FindKeyWordObject(token.Raw);
            if (obj is Type t && typeof(T).IsAssignableFrom(t))
            {
                return t;
            }
            throw new UnexpectedTokenException(token);
        }

        public bool CanPopToken<T>()
        {
            if (TokenStream.HasNextToken())
            {
                var token = TokenStream.NextToken();
                var obj = FindKeyWordObject(token.Raw);
                return obj switch
                {
                    Type t => typeof(T).IsAssignableFrom(t),
                    Enum e => e is T,
                    _ => false,
                };
            }
            return false;
        }

        public T PopInstance<T>() where T : IUnit
        {
            var type = PopType<T>();
            var instance = (T)Activator.CreateInstance(type);
            Contract.Assume(instance != null);
            return instance;
        }

        public IExpression PopExpression()
        {
            var exp = PopInstance<IExpression>();
            exp.ContinueParseExpression(this, false);
            return exp;
        }
    }
}
