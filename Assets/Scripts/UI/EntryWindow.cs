using System;
using UnityEngine;

public class EntryWindow : MonoBehaviour
{
    public Action OnClose;

    public void Close()
    {
        OnClose();
    }
}