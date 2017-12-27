namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class OpLike : BinaryOperator
    {
        private StringBuilder ConvertGroupSubexpression(char[] carr, ref int pos)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            while (carr[pos] != ']')
            {
                if (flag)
                {
                    builder.Append('^');
                    flag = false;
                }
                if (carr[pos] == '!')
                {
                    builder.Remove(1, builder.Length - 1);
                    flag = true;
                }
                else
                {
                    builder.Append(carr[pos]);
                }
                pos++;
            }
            builder.Append(']');
            return builder;
        }

        private string ConvertLikeExpression(string expression)
        {
            char[] carr = expression.ToCharArray();
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            for (int i = 0; i < carr.Length; i++)
            {
                StringBuilder builder2;
                char ch = carr[i];
                if (ch <= '*')
                {
                    switch (ch)
                    {
                        case '#':
                        {
                            if (!flag)
                            {
                                goto Label_00C0;
                            }
                            builder.Append('\\').Append('d').Append('{').Append('1').Append('}');
                            continue;
                        }
                        case '%':
                        {
                            builder.Append('*');
                            continue;
                        }
                        case '*':
                            goto Label_006C;
                    }
                    goto Label_011B;
                }
                if (ch != '?')
                {
                    if (ch == '[')
                    {
                        goto Label_00F2;
                    }
                    if (ch != '_')
                    {
                        goto Label_011B;
                    }
                }
                builder.Append('.');
                continue;
            Label_006C:
                builder.Append('.').Append('*');
                continue;
            Label_00C0:
                builder.Append('^').Append('\\').Append('d').Append('{').Append('1').Append('}');
                flag = true;
                continue;
            Label_00F2:
                builder2 = this.ConvertGroupSubexpression(carr, ref i);
                if (builder2.Length > 2)
                {
                    builder.Append(builder2);
                }
                continue;
            Label_011B:
                builder.Append(carr[i]);
            }
            if (flag)
            {
                builder.Append('$');
            }
            return builder.ToString();
        }

        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            string text = base.Left.EvaluateInternal(context, evalContext) as string;
            string pattern = base.Right.EvaluateInternal(context, evalContext) as string;
            return this.StrLike(text, pattern);
        }

        private bool StrLike(string text, string pattern)
        {
            if (StringUtils.IsNullOrEmpty(text) && StringUtils.IsNullOrEmpty(pattern))
            {
                return true;
            }
            if ((StringUtils.IsNullOrEmpty(text) || StringUtils.IsNullOrEmpty(pattern)) && (pattern != "[]"))
            {
                return false;
            }
            RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase;
            Regex regex = new Regex(this.ConvertLikeExpression(pattern), options);
            return regex.IsMatch(text);
        }
    }
}

