//注册
public class MsgRegister : MsgBase
{
    public MsgRegister() { protoName = "MsgRegister"; }
    //客户端发送
    public string id = "";
    public string pw = "";
    //服务端回应（0-成功，1-失败）
    public int result = 0;
}

//登录
public class MsgLogin : MsgBase
{
    public MsgLogin() { protoName = "MsgLogin"; }
    //客户端发送
    public string id = "";
    public string pw = "";
    //服务端回应（0-成功，1-失败）
    public int result = 0;
}

//强制下线
public class MsgKick : MsgBase
{
    public MsgKick() { protoName = "MsgKick"; }
    //原因（0-二次登录，1-服务器维护，2-账号封禁）
    public int reason = 0;
}