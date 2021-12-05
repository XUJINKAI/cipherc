using System.CommandLine;
using System.Linq;

namespace cipherc.Parser.Options
{
    internal class VersionOption : Option
    {
        public VersionOption(string[] aliases, string? description = null) : base(aliases, description)
        {
            AddValidators();
        }

        private void AddValidators()
        {
            AddValidator(result =>
            {
                if (result.Parent is { } parent &&
                    parent.Children.Where(r => r.Symbol is not VersionOption).Count() > 0)
                {
                    return "--version option cannot be combined with other arguments.";
                }

                return null;
            });
        }
    }
}