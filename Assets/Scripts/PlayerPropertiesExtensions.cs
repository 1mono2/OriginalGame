using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{

    private const string ReadyState = "ReadyState";
    private const string StartTime = "StartTime";
    private static readonly Hashtable propsToSet = new Hashtable();
    

    public static bool GetReadyState(this Photon.Realtime.Player player)
    {
        return (player.CustomProperties[ReadyState] is true) ? true : false;
    }

    public static void SetReadyStateToTrue(this Photon.Realtime.Player player)
    {
        propsToSet[ReadyState] = true;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetReadyStateToFalse(this Photon.Realtime.Player player)
    {
        propsToSet[ReadyState] = false;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static bool TryGetStartTime(this Room room, out int timestamp)
    {
        if(room.CustomProperties[StartTime]  is int value)
        {
            timestamp = value;
            return true;
        }
        else
        {
            timestamp = 0;
            return false;
        }
    }

    public static void SetStartTime(this Room room, int timestamp)
    {
        propsToSet[StartTime] = timestamp;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

}
