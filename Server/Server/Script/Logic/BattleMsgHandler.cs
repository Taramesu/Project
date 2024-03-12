
public partial class MsgHandler
{
    public static void MsgMove(ClientState state, MsgBase msgBase)
    {
        MsgMove msgMove = (MsgMove)msgBase;
        Console.WriteLine(msgMove.x);
        msgMove.x++;
        NetManager.Send(state, msgMove);
    }
}