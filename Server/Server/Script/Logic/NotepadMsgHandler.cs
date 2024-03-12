
public partial class MsgHandler
{
    //获取记事本内容
    public static void MsgGetText(ClientState state, MsgBase msgBase)
    {
        MsgGetText msg = (MsgGetText)msgBase;
        Player player = state.player;
        if(player == null) { return; }
        //获取text
        msg.text = player.data.text;
        player.Send(msg);
    }

    public static void MsgSaveText(ClientState state, MsgBase msgBase) 
    {
        MsgSaveText msg = (MsgSaveText)msgBase;
        Player player = state.player;
        if(player == null) { return; }
        //获取text
        player.data.text = msg.text;
        player.Send(msg);
    }
}