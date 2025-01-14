namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class RgbaFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            if (Arguments.Count == 2)
            {
                Guard.ExpectNode<Color>(Arguments[0], this, Location);
                Guard.ExpectNode<Number>(Arguments[1], this, Location);

                return new Color(((Color)Arguments[0]).RGB, ((Number)Arguments[1]).Value);
            }

            Guard.ExpectNumArguments(4, Arguments.Count, this, Location);
            Guard.ExpectAllNodes<Number>(Arguments, this, Location);

            var args = Arguments.Cast<Number>();

            var rgb = args.Take(3);

            return new Color(rgb, args.ElementAt(3));
        }
    }
}