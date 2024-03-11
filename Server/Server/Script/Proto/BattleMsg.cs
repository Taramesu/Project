public class MsgMove : MsgBase
{
    public MsgMove() { protoName = "MsgMove"; }
    public int x;
    public int y;
    public int z;
}