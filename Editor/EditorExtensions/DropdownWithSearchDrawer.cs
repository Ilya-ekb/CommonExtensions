#if UNITY_EDITOR
using System.Linq;
using EditorExtensions.Drawers;
using TriInspector;
using TriInspector.Resolvers;

[assembly: RegisterTriAttributeDrawer(typeof(DropdownWithSearchDrawer<>), TriDrawerOrder.Decorator)]

namespace EditorExtensions.Drawers
{
    public class DropdownWithSearchDrawer<T> : TriAttributeDrawer<DropdownWithSearchAttribute>
    {
        private DropdownValuesResolver<T> valuesResolver;

        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            valuesResolver = DropdownValuesResolver<T>.Resolve(propertyDefinition, Attribute.Values);
            if (valuesResolver.TryGetErrorString(out var error))
                return error;
            return TriExtensionInitializationResult.Ok;
        }

        public override TriElement CreateElement(TriProperty property, TriElement next)
        {
            var items = valuesResolver.GetDropdownItems(property);
            if (items == null)
                return next;
            return new TriDropdownWithSearchElement(property, valuesResolver.GetDropdownItems);
        }
    }
}
#endif