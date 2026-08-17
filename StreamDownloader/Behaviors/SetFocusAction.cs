namespace StreamDownloader.Behaviors;

using System.Windows;
using Microsoft.Xaml.Behaviors;

public class SetFocusAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty TargetElementProperty = DependencyProperty.Register(
        nameof(TargetElement),
        typeof(UIElement),
        typeof(SetFocusAction),
        new PropertyMetadata(null));

    public UIElement TargetElement
    {
        get => (UIElement)this.GetValue(TargetElementProperty);
        set => this.SetValue(TargetElementProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        this.TargetElement?.Focus();
    }
}
