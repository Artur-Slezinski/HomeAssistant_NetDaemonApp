﻿namespace Alarm;
[NetDaemonApp]

public class AlarmStateNotify
{
    public AlarmStateNotify(IHaContext ha, IScheduler scheduler)
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

    public void WhatsAppAlarmArmedNotify(Services services)
    {
        services.Notify.Whatsapp
                 (message: "Alarm uzbrojony!");
    }

    public void WhatsAppAlarmDisarmedNotify(Services services)
    {
        services.Notify.Whatsapp
                 (message: "Alarm rozbrojony!");
    }
}