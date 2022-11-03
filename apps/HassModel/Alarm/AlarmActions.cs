
namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    public AlarmActions(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => FlashingLights(_myEntities));
    }

    private void FlashingLights(Entities entities)
    {
        var allLights = new[] {
         entities.Light.Airqualityoutdoorledring,
         entities.Light.Led
        };              
       

        while (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            allLights.TurnOn(transition: 0, colorName: "Gold", brightness: 255);
            System.Threading.Thread.Sleep(500);
            allLights.TurnOn(transition: 0, colorName: "Blue", brightness: 255);
            System.Threading.Thread.Sleep(500);
        }
    }
}





