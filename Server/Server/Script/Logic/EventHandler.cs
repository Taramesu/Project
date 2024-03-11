
public partial class EventHandler
{
    public static void OnDisconnect(ClientState state)
    {
        Console.WriteLine("Close");
    }

    /// <summary>
    /// 超时检测
    /// </summary>
    public static void OnTimer()
    {
        CheckPing();
    }

    public static void CheckPing()
    {
        //现在的时间戳
        long timeNow = NetManager.GetTimeStamp();
        //遍历，删除
        foreach(ClientState s in NetManager.clients.Values) 
        {
            if(timeNow - s.lastPingTime > NetManager.pingInterval * 4)
            {
                Console.WriteLine($"PingTimeInterval:{timeNow - s.lastPingTime},TimeNow:{timeNow},lastPingTime:{s.lastPingTime}");
                Console.WriteLine("Ping Close" + s.socket.RemoteEndPoint.ToString());
                NetManager.Close(s);
                return;
            }
        }
    }
}
