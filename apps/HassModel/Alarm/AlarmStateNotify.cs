namespace Alarm;
[NetDaemonApp]

public class AlarmStateNotify
{
    public AlarmStateNotify(IHaContext ha)
    {        
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => WhatsAppAlarmArmedNotify(_services));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => WhatsAppAlarmDisarmedNotify(_services));       
    }

    private void WhatsAppAlarmArmedNotify(Services services)
    {
        services.Notify.Whatsapp
        (message: "Alarm uzbrojony!");    
    }

    private void WhatsAppAlarmDisarmedNotify(Services services)
    {
        services.Notify.Whatsapp
                 (message: "Alarm rozbrojony!");
    }


}