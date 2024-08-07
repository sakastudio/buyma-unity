using System;
using System.Net;
using System.Text;
using UnityEngine;

namespace Server
{
    public class WebServer
    {
        private static string _sendString = null;

        public static void SetSendString(SendBuymData sendBuymData)
        {
            _sendString = JsonUtility.ToJson(sendBuymData);
            Debug.Log("送信データ " + _sendString);
        }
        
        
        public static void StartServer()
        {
            // HTTPリスナー作成
            HttpListener listener = new HttpListener();

            // リスナー設定
            listener.Prefixes.Clear();
            listener.Prefixes.Add(@"http://localhost:3500/");

            // リスナー開始
            listener.Start();

            Debug.Log("サーバーをスタート");

            while (true)
            {
                Debug.Log("リクエストを待機");
                    
                // リクエスト取得
                var context = listener.GetContext();
                var request = context.Request;
                    
                // レスポンス取得
                var response = context.Response;

                // HTMLを表示する
                if (request != null)
                {
                    // getが含まれるかどうか
                    if (request.Url.ToString().Contains("get") && _sendString != null)
                    {
                        var text = Encoding.UTF8.GetBytes(_sendString);
                        response.Headers.Add("Access-Control-Allow-Origin", "*"); // CORSを全てに設定
                        response.OutputStream.Write(text, 0, text.Length);
                        
                        Debug.Log("リクエスト get レスポンス " + _sendString);
                    } else if (request.Url.ToString().Contains("ok"))
                    {
                        _sendString = null;
                        var empty = Encoding.UTF8.GetBytes("");
                        response.Headers.Add("Access-Control-Allow-Origin", "*");// CORSを全てに設定
                        response.OutputStream.Write(empty, 0, empty.Length);
                        Debug.Log("リクエスト ok レスポンス");
                    }

                    response.Close();
                }
            }
        }
    }
}
