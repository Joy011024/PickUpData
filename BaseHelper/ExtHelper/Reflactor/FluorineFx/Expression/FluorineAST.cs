namespace FluorineFx.Expression
{
    using antlr;
    using antlr.collections;
    using System;

    internal class FluorineAST : BaseAST
    {
        internal static readonly FluorineASTCreator Creator = new FluorineASTCreator();
        private string text;
        private int ttype;

        public FluorineAST()
        {
        }

        public FluorineAST(IToken token)
        {
            this.initialize(token);
        }

        public override string getText()
        {
            return this.text;
        }

        public override void initialize(AST t)
        {
            this.setText(t.getText());
            this.set_Type(t.get_Type());
        }

        public override void initialize(IToken tok)
        {
            this.setText(tok.getText());
            this.set_Type(tok.get_Type());
        }

        public override void initialize(int t, string txt)
        {
            this.set_Type(t);
            this.setText(txt);
        }

        public override void setText(string txt)
        {
            this.text = txt;
        }

        public string Text
        {
            get
            {
                return this.getText();
            }
            set
            {
                this.setText(value);
            }
        }

        public override int Type
        {
            get
            {
                return this.ttype;
            }
            set
            {
                this.ttype = value;
            }
        }

        internal class FluorineASTCreator : ASTNodeCreator
        {
            public override AST Create()
            {
                return new FluorineAST();
            }

            public override string ASTNodeTypeName
            {
                get
                {
                    return typeof(FluorineAST).FullName;
                }
            }
        }
    }
}

