using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TestByteArray : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //[1 ´´½¨]
        ByteArray buff = new ByteArray(8);
        Debug.Log("[1 debug ]¡ú" + buff.Debug());
        Debug.Log("[1 string]¡ú" + buff.ToString());
        //[2 write]
        byte[] wb = new byte[] { 1, 2, 3, 4, 5 };
        buff.Write(wb, 0, 5);
        Debug.Log("[2 debug ]¡ú" + buff.Debug());
        Debug.Log("[2 string]¡ú" + buff.ToString());
        //[3 read]
        byte[] rb = new byte[4];
        buff.Read(rb, 0, 2);
        Debug.Log("[3 debug ]¡ú" + buff.Debug());
        Debug.Log("[3 string]¡ú" + buff.ToString());
        Debug.Log("[3 rb ]¡ú" + BitConverter.ToString(rb));
        //[4 write, resize]
        wb = new byte[] { 6, 7, 8, 9, 10, 11 };
        buff.Write(wb, 0, 6);
        Debug.Log("[4 debug ]¡ú" + buff.Debug());
        Debug.Log("[4 string]¡ú" + buff.ToString());
    }
}