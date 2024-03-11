
public partial class MsgHandler
{
    public static void MsgPing(ClientState state, MsgBase msgBase)
    {
        Console.WriteLine("MsgPing");
        state.lastPingTime = NetManager.GetTimeStamp();
        MsgPong msgPong = new();
        NetManager.Send(state, msgPong);
    }
}