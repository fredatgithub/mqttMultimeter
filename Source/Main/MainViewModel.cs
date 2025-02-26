using System;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Connection;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Pages.Info;
using mqttMultimeter.Pages.Log;
using mqttMultimeter.Pages.PacketInspector;
using mqttMultimeter.Pages.Publish;
using mqttMultimeter.Pages.Subscriptions;
using mqttMultimeter.Pages.TopicExplorer;
using ReactiveUI;

namespace mqttMultimeter.Main;

public sealed class MainViewModel : BaseViewModel
{
    object? _overlayContent;

    public MainViewModel(ConnectionPageViewModel connectionPage,
        PublishPageViewModel publishPage,
        SubscriptionsPageViewModel subscriptionsPage,
        InflightPageViewModel inflightPage,
        TopicExplorerPageViewModel topicExplorerPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage,
        LogPageViewModel logPage)
    {
        ConnectionPage = AttachEvents(connectionPage);
        PublishPage = AttachEvents(publishPage);
        SubscriptionsPage = AttachEvents(subscriptionsPage);
        InflightPage = AttachEvents(inflightPage);
        TopicExplorerPage = AttachEvents(topicExplorerPage);
        PacketInspectorPage = AttachEvents(packetInspectorPage);
        InfoPage = AttachEvents(infoPage);
        LogPage = AttachEvents(logPage);

        InflightPage.RepeatMessageRequested += item => PublishPage.RepeatMessage(item);
        topicExplorerPage.RepeatMessageRequested += item => PublishPage.RepeatMessage(item);
    }

    public event EventHandler? ActivatePageRequested;

    public ConnectionPageViewModel ConnectionPage { get; }

    public InflightPageViewModel InflightPage { get; }

    public InfoPageViewModel InfoPage { get; }

    public LogPageViewModel LogPage { get; }

    public object? OverlayContent
    {
        get => _overlayContent;
        set => this.RaiseAndSetIfChanged(ref _overlayContent, value);
    }

    public PacketInspectorPageViewModel PacketInspectorPage { get; }

    public PublishPageViewModel PublishPage { get; }

    public SubscriptionsPageViewModel SubscriptionsPage { get; }

    public TopicExplorerPageViewModel TopicExplorerPage { get; }

    TPage AttachEvents<TPage>(TPage page) where TPage : BasePageViewModel
    {
        page.ActivationRequested += (_, __) => ActivatePageRequested?.Invoke(page, EventArgs.Empty);
        return page;
    }
}