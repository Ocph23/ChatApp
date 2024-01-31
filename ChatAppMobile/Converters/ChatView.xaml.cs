
using CommunityToolkit.Maui.Views;
using Shared;

namespace ChatAppMobile.Converters;

public partial class ChatView : ContentView
{
    public ChatView()
    {
        InitializeComponent();
        this.BindingContext = this;
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create("MessageProperty", typeof(MessagePrivate), typeof(MessagePrivate), null);

    public MessagePrivate Message
    {
        get => (MessagePrivate)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public Command DownloadFileCommand { get; private set; }
}