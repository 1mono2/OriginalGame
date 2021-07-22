using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{

    private const string ReadyState = "ReadyState";
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

}
