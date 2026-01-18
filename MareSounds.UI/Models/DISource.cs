using System.Windows.Markup;

namespace MareSounds.UI.Models
{
    public class DISource : MarkupExtension
    {
        public static Func<Type, object>? Resolver { get; set; }
        public Type? Type { get; set; }

        public override object? ProvideValue(IServiceProvider serviceProvider) => Type is not null ? Resolver?.Invoke(Type) : null;
    }
}
