
public class Player
{
    //id
    public string id = "";
    //指向ClientState
    public ClientState state;

    //临时数据
    public int x;
    public int y;
    public int z;

    //数据库数据
    public PlayerData data;

    public Player(ClientState state)
    {
        this.state = state;
    }

    //发送消息
    public void Send(MsgBase msgBase)
    {
        NetManager.Send(state, msgBase);
    }
}
