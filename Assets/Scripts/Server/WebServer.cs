using System;
using System.Net;
using System.Text;
using UnityEngine;

namespace Server
{
    public class WebServer
    {
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

                    // HTMLを表示する
                    if (request != null)
                    {
                        Debug.Log("リクエスト " + request.Url);
                        
                        // CORSを全てに設定
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        
                        byte[] text = Encoding.UTF8.GetBytes("{\"test\" : \"hoge\",\"test2\" : 2}");
                        response.OutputStream.Write(text, 0, text.Length);
                    }
                    else
                    {
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
