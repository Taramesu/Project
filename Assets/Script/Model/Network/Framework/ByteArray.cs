using System;
using UnityEngine;

public class ByteArray
{
    //默认大小
    const int DEFAULT_SIZE = 1024;
    //默认有效数据阈值
    const int MIN_VALID_DATA_SIZE = 8;
    //初始大小
    int initSize = 0;
    //缓冲区
    public byte[] bytes;
    //读写位置
    public int readIdx = 0;
    public int writeIdx = 0;
    //容量
    private int capacity = 0;
    //剩余空间
    public int remain { get { return capacity - writeIdx; } }
    //数据长度
    public int length { get { return writeIdx-readIdx; } }
    //有效空间是否足够
    public bool remainEnough { get { return remain > MIN_VALID_DATA_SIZE; } }

    //构造函数
    public ByteArray(int size =  DEFAULT_SIZE)
    {
        bytes = new byte[size];
        capacity = size;
        initSize = size;
        readIdx = 0;
        writeIdx = 0;
    }

    //构造函数
    public ByteArray(byte[] defaultBytes)
    {
        bytes = defaultBytes;
        capacity = defaultBytes.Length;
        initSize = defaultBytes.Length;
        readIdx = 0;
        writeIdx = defaultBytes.Length;
    }

    /// <summary>
    /// 重设尺寸
    /// </summary>
    /// <param name="size"></param>
    public void ReSize(int size)
    {
        if (size < length) return;
        if (size < initSize) return;
        int n = 1;
        while (n < size) n *= 2;
        capacity = n;
        byte[] newBytes = new byte[capacity];
        Array.Copy(bytes, readIdx, newBytes, 0, writeIdx - readIdx);
        bytes = newBytes;
        writeIdx = length;
        readIdx = 0;
    }

    /// <summary>
    /// 移动数据
    /// </summary>
    public void MoveBytes()
    {
        Array.Copy(bytes, readIdx, bytes, 0, length);
        writeIdx = length;
        readIdx = 0;
    }

    /// <summary>
    /// 检查并移动数据
    /// </summary>
    public void CheckAndMoveBytes()
    {
        if(length<MIN_VALID_DATA_SIZE)
        {
            MoveBytes();
        }
    }

    /// <summary>
    /// 写入数据
    /// </summary>
    /// <param name="bs"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int Write(byte[] bs, int offset, int count)
    {
        if(remain<count) 
        {
            ReSize(length + count);
        }
        Array.Copy(bs, offset, bytes, writeIdx, count);
        writeIdx += count;
        return count;
    }

    /// <summary>
    ///读取数据 
    /// </summary>
    /// <param name="bs"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int Read(byte[] bs, int offset, int count) 
    {
        count = Math.Min(count, length);
        Array.Copy(bytes, 0, bs, offset, count);
        readIdx += count;
        CheckAndMoveBytes();
        return count;
    }

    /// <summary>
    /// 读取Int16
    /// </summary>
    /// <returns></returns>
    public Int16 ReadInt16()
    {
        if (length < 2) return 0;
        Int16 ret = (Int16)((bytes[1] << 8) | bytes[0]);
        readIdx += 2;
        CheckAndMoveBytes();
        return ret;
    }

    /// <summary>
    /// 读取Int32
    /// </summary>
    /// <returns></returns>
    public Int32 ReadInt32()
    {
        if(length < 4) return 0;
        Int32 ret = (Int32)((bytes[3] << 24) |
                            (bytes[2] << 16) |
                            (bytes[1] << 8)  |
                             bytes[0]);
        readIdx += 4;
        CheckAndMoveBytes();
        return ret;
    }

    /// <summary>
    /// 打印缓冲区（仅调试用）
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return BitConverter.ToString(bytes, readIdx, length);
    }

    /// <summary>
    /// 打印调试信息（仅调试用）
    /// </summary>
    /// <returns></returns>
    public string Debug()
    {
        return string.Format("readIdx({0}) writeIdx({1}) bytes({2})",
            readIdx, writeIdx, BitConverter.ToString(bytes, 0, bytes.Length));
    }
}