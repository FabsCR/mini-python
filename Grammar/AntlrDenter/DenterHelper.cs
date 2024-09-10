using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace AntlrDenter
{
    public abstract class DenterHelper
    {
        private readonly int _dedentToken;
        private readonly Queue<IToken> _dentsBuffer = new Queue<IToken>();
        private readonly LinkedList<int> _indentations = new LinkedList<int>();
        private readonly int _indentToken;
        private readonly int _nlToken;
        private IEofHandler _eofHandler;
        private bool _reachedEof;

        protected DenterHelper(int nlToken, int indentToken, int dedentToken)
        {
            _nlToken = nlToken;
            _indentToken = indentToken;
            _dedentToken = dedentToken;
            _eofHandler = new StandardEofHandler(this);
        }

        public IToken NextToken()
        {
            InitIfFirstRun();
            IToken t = _dentsBuffer.Count == 0
                ? PullToken()
                : _dentsBuffer.Dequeue();
            if (_reachedEof) return t;
            IToken r;
            if (t.Type == _nlToken)
                r = HandleNewlineToken(t);
            else if (t.Type == -1)
                r = _eofHandler.Apply(t);
            else
                r = t;
            return r;
        }

        public IDenterOptions GetOptions()
        {
            return new DenterOptionsImpl(this);
        }

        protected abstract IToken PullToken();

        private void InitIfFirstRun()
        {
            if (_indentations.Count == 0)
            {
                _indentations.AddFirst(0); // Inicializamos con indentación 0
                IToken firstRealToken;
                do
                {
                    firstRealToken = PullToken();
                } while (firstRealToken.Type == _nlToken);

                // No ignoramos NEWLINE pero aseguramos que haya token relevante.
                if (firstRealToken.Column > 0)
                {
                    _indentations.AddFirst(firstRealToken.Column);
                    _dentsBuffer.Enqueue(CreateToken(_indentToken, firstRealToken));
                }

                _dentsBuffer.Enqueue(firstRealToken); // Encolamos el primer token real
            }
        }

        private IToken HandleNewlineToken(IToken t)
        {
            IToken nextNext = PullToken();
            while (nextNext.Type == _nlToken)
            {
                t = nextNext;
                nextNext = PullToken(); // Avanzamos al próximo token
            }

            if (nextNext.Type == -1) return _eofHandler.Apply(nextNext);

            // Usamos el valor de columna del próximo token en lugar de la longitud del texto
            int indent = nextNext.Column;
            int prevIndent = _indentations.First.Value;
            IToken r;
            if (indent == prevIndent)
            {
                r = t;
            }
            else if (indent > prevIndent)
            {
                r = CreateToken(_indentToken, t);
                _indentations.AddFirst(indent);
            }
            else
            {
                r = UnwindTo(indent, t);
            }

            _dentsBuffer.Enqueue(nextNext); // Encolamos el token siguiente
            return r;
        }

        private IToken CreateToken(int tokenType, IToken copyFrom)
        {
            string tokenTypeStr;
            if (tokenType == _nlToken)
                tokenTypeStr = "newline";
            else if (tokenType == _indentToken)
                tokenTypeStr = "indent";
            else if (tokenType == _dedentToken)
                tokenTypeStr = "dedent";
            else
                tokenTypeStr = null;
            CommonToken r = new InjectedToken(copyFrom, tokenTypeStr);
            r.Type = tokenType;
            return r;
        }

        private IToken UnwindTo(int targetIndent, IToken copyFrom)
        {
            _dentsBuffer.Enqueue(CreateToken(_nlToken, copyFrom));
            while (true)
            {
                int prevIndent = _indentations.First.Value;
                _indentations.RemoveFirst();
                if (prevIndent == targetIndent) break;
                if (targetIndent > prevIndent)
                {
                    _indentations.AddFirst(prevIndent);
                    _dentsBuffer.Enqueue(CreateToken(_indentToken, copyFrom));
                    break;
                }

                _dentsBuffer.Enqueue(CreateToken(_dedentToken, copyFrom));
            }

            _indentations.AddFirst(targetIndent);
            return _dentsBuffer.Dequeue();
        }

        public static IBuilder0 Builder()
        {
            return new BuilderImpl();
        }

        private class StandardEofHandler : IEofHandler
        {
            private readonly DenterHelper _helper;

            public StandardEofHandler(DenterHelper helper)
            {
                _helper = helper;
            }

            public IToken Apply(IToken t)
            {
                // Forzamos un NEWLINE antes de procesar el EOF para asegurar DEDENT
                _helper._dentsBuffer.Enqueue(_helper.CreateToken(_helper._nlToken, t));
                
                IToken r;
                if (_helper._indentations.Count == 0)
                {
                    r = _helper.CreateToken(_helper._nlToken, t);
                    _helper._dentsBuffer.Enqueue(t);
                }
                else
                {
                    r = _helper.UnwindTo(0, t);  // Forzar dedent hasta nivel 0
                    _helper._dentsBuffer.Enqueue(t);
                }

                _helper._reachedEof = true;
                return r;
            }
        }

        private interface IEofHandler
        {
            IToken Apply(IToken t);
        }

        private class DenterOptionsImpl : IDenterOptions
        {
            private readonly DenterHelper _helper;

            public DenterOptionsImpl(DenterHelper helper)
            {
                _helper = helper;
            }

            public void IgnoreEof()
            {
                _helper._eofHandler = new EofHandler(_helper);
            }

            private class EofHandler : IEofHandler
            {
                private readonly DenterHelper _helper;

                public EofHandler(DenterHelper helper)
                {
                    _helper = helper;
                }

                public IToken Apply(IToken t)
                {
                    _helper._reachedEof = true;
                    return t;
                }
            }
        }

        private class InjectedToken : CommonToken
        {
            private readonly string _type;

            public InjectedToken(IToken oldToken, string type) : base(oldToken)
            {
                _type = type;
            }

            public override string Text
            {
                get => _type ?? base.Text;
                set => base.Text = value;
            }
        }

        public interface IBuilder0
        {
            IBuilder1 Nl(int nl);
        }

        public interface IBuilder1
        {
            IBuilder2 Indent(int indent);
        }

        public interface IBuilder2
        {
            IBuilder3 Dedent(int dedent);
        }

        public interface IBuilder3
        {
            DenterHelper PullToken(Func<IToken> puller);
        }

        private class BuilderImpl : IBuilder0, IBuilder1, IBuilder2, IBuilder3
        {
            private int _dedent;
            private int _indent;
            private int _nl;

            public IBuilder1 Nl(int nl)
            {
                _nl = nl;
                return this;
            }

            public IBuilder2 Indent(int indent)
            {
                _indent = indent;
                return this;
            }

            public IBuilder3 Dedent(int dedent)
            {
                _dedent = dedent;
                return this;
            }

            public DenterHelper PullToken(Func<IToken> puller)
            {
                return new DenterHelperImpl(_nl, _indent, _dedent, puller);
            }

            private class DenterHelperImpl : DenterHelper
            {
                private readonly Func<IToken> _puller;

                public DenterHelperImpl(int nlToken, int indentToken, int dedentToken, Func<IToken> puller) 
                    : base(nlToken, indentToken, dedentToken)
                {
                    _puller = puller;
                }

                protected override IToken PullToken()
                {
                    return _puller();
                }
            }
        }
    }
}
