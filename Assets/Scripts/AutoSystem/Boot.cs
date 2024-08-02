using System;
using System.Threading;
using Server;
using UnityEngine;

namespace AutoSystem
{
    public class Boot : MonoBehaviour
    {
        Thread thread;
        private void Start()
        {
            thread = new Thread(WebServer.StartServer);
            thread.Start();
        }
        
        private void OnApplicationQuit()
        {
            thread.Abort();
        }
    }
}