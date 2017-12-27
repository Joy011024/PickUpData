using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"1 + (2 - 3) * 4";

            var stream = new AntlrInputStream(input);
            var lexer = new MyGrammarLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new MyGrammarParser(tokens);
            var tree = parser.program();

            var visitor = new MyGrammarVisitor();
            var result = visitor.Visit(tree);

            Console.WriteLine(tree.ToStringTree(parser));
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
