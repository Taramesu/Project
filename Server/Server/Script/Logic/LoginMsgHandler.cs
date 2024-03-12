
public partial class MsgHandler
{
    //注册协议处理
    public static void MsgRegister(ClientState state, MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;

        if (DbManager.Register(msg.id, msg.pw))
        {
            DbManager.CreatePlayer(msg.id);
            msg.result = 0;
        }
        else
        {
            msg.result = 1;
        }
        NetManager.Send(state, msg);
    }

    //登录协议处理
    public static void MsgLogin(ClientState state, MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;

        //密码校验
        if (!DbManager.CheckPassword(msg.id, msg.pw))
        {
            msg.result = 1;
            NetManager.Send(state, msg);
            return;
        }
        //不允许再次登录
        if(state.player != null) 
        {
            msg.result = 1;
            NetManager.Send(state, msg);
            return;
        }
        if(PlayerManager.IsOnline(msg.id))
        {
            //发送强制下线协议
            Player other = PlayerManager.GetPlayer(msg.id);
            MsgKick msgKick = new();
            msgKick.reason = 0;
            other.Send(msgKick);
            //断开连接
            NetManager.Close(other.state);
        }
        //获取玩家数据
        PlayerData playerData = DbManager.GetPlayerData(msg.id);
        if(playerData == null) 
        {
            msg.result = 1;
            NetManager.Send(state, msg);
            return;
        }
        //构建Player
        Player player = new(state);
        player.id = msg.id;
        player.data = playerData;
        PlayerManager.AddPlayer(msg.id, player);
        state.player = player;
        //返回协议
        msg.result = 0;
        player.Send(msg);
    }
}