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
            try
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
                    // CORSを全てに設定
                    response.Headers.Add("Access-Control-Allow-Origin", "*");

                    // HTMLを表示する
                    if (request != null && _sendString != null)
                    {
                        Debug.Log("リクエスト " + request.Url + " レスポンス " + _sendString);
                        
                        var text = Encoding.UTF8.GetBytes(_sendString);
                        response.OutputStream.Write(text, 0, text.Length);
                        _sendString = null;
                    }
                    else
                    {
                        Debug.Log("リクエスト " + request.Url + " レスポンス 404");
                        response.StatusCode = 404;
                    }

                    response.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
