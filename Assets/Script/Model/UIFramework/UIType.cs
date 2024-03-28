namespace UIFrameWork
{
    public class UIType
    {
        //类型名
        public string Name { get; private set; }
        //路径
        public string Path { get; private set; }
        //是否是热更UI
        public bool IsHotfixUI { get; private set; }

        public UIType(string path, bool isHotfixUI = true)
        {
            Path = path;
            IsHotfixUI = isHotfixUI;
            //取最后一个/符号对应的位置表示该UI的名字
            Name = path.Substring(path.LastIndexOf('/') + 1);  
        }
    }
}


