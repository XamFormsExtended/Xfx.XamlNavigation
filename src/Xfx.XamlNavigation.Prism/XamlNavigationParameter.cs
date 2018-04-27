using Xamarin.Forms;

namespace Xfx.XamlNavigation.Prism
{
    public class XamlNavigationParameter : BindableObject
    {
        public string Key { get; set; }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value),
            typeof(object),
            typeof(XamlNavigationParameter));

        /// <summary>
        /// Value summary. This is a bindable property.
        /// </summary>
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}