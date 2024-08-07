using System;
using System.Text.RegularExpressions;
using Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AcomoInputUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField 元データ;

        [SerializeField] private TMP_Text 商品URL;
        [SerializeField] private Button URLを開く;
        
        [SerializeField] private TMP_InputField 商品コメント簡易商品名;
        [SerializeField] private TMP_InputField 商品コメント素材;
        [SerializeField] private TMP_InputField 商品コメント色;
        [SerializeField] private TMP_Dropdown 商品コメントリスト;

        [SerializeField] private Toggle サイズを入力;
        [SerializeField] private GameObject サイズ入力欄;
        [SerializeField] private TMP_InputField サイズ;

        [SerializeField] private Button 送信ボタン;

        private void Awake()
        {
            URLを開く.onClick.AddListener(() =>
            {
                Application.OpenURL(商品URL.text);
            });
            サイズを入力.onValueChanged.AddListener(isOn =>
            {
                サイズ入力欄.SetActive(isOn);
            });
            元データ.onEndEdit.AddListener(_ => CreateData());
            送信ボタン.onClick.AddListener(() =>
            {
                var data = CreateData();
                if (data != null)
                {
                    WebServer.SetSendString(data);
                }
            });
        }

        public SendBuymData CreateData()
        {
            try
            {
                var split = 元データ.text.Split('\t');
                //全角数字を半角数字に変換
                var 商品num = ZenToHanNum(split[0]);
                var カテゴリ1 = split[3].Split(" > ")[0];
                var カテゴリ2 = split[3].Split(" > ")[1];
                var カテゴリ3 = split[3].Split(" > ")[2];
                var シーズン = split[5];
                var 商品名 = split[6];
                var url = split[7];
                商品URL.text = url;
                var 価格 = int.Parse(split[12].Replace("¥", "").Replace(",", ""));
                
                var 利益 = split[25];
                var 利益率 = split[26];

                var buymaData = new SendBuymData();
                buymaData.商品名 = 商品名;
                
                var 商品簡易コメント = 商品コメントリスト.options[商品コメントリスト.value].text.Replace("{アイテム}",商品コメント簡易商品名.text);
                buymaData.商品コメント = 商品コメントテンプレート
                    .Replace("{素材}", 商品コメント素材.text)
                    .Replace("{色名}", 商品コメント色.text)
                    .Replace("{簡易コメント}", 商品簡易コメント);
                buymaData.第一カテゴリ = カテゴリ1;
                buymaData.第二カテゴリ = カテゴリ2;
                buymaData.第三カテゴリ = カテゴリ3;

                buymaData.ブランド = "CHANEL";
                buymaData.買付ショップ名 = "CHANEL直営店";
                buymaData.URL = url;
                buymaData.シーズン = シーズン;

                var 色サイズ = 色_サイズ情報テンプレート
                    .Replace("{ブランド名}", "CHANEL")
                    .Replace("{簡易商品名}", 商品コメント簡易商品名.text)
                    .Replace("{色名}", 商品コメント色.text.Replace("\n",""))
                    .Replace("{素材}", 商品コメント素材.text.Replace("\n",""))
                    .Replace("{簡易コメント}", 商品簡易コメント);
                if (サイズを入力.isOn)
                {
                    色サイズ = 色サイズ.Replace("{サイズ}", サイズ情報テンプレート.Replace("{サイズ情報}", サイズ.text));
                }
                else
                {
                    色サイズ = 色サイズ.Replace("\n{サイズ}", "");
                }
                buymaData.色_サイズ情報 = 色サイズ;
                
                buymaData.価格 = 価格;
                buymaData.出品メモ = $"三浦 伽奈\nNo.{商品num}\n利益 {利益}\n利益率 {利益率}";

                return buymaData;
            }
            catch (Exception e)
            {
                Debug.LogError("Parse元データ Error: " + e.Message + "\n" + e.StackTrace);
                return null;
            }
        }
        
        
        string ZenToHanNum(string s)
        {
            return Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
        }

        private const string 商品コメントテンプレート = @"+‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥+

{素材}
{色名}

◆{簡易コメント}

+‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥+

◆当店の商品は全て直営店または正規販売店より買付しております。100%正規品ですのでご安心ください。

◆在庫状況が日々変動するため、ご購入前に必ずお問い合わせより在庫の確認をお願いしております。

◆在庫状況により指定とは異なる発送地・方法で発送する場合もございます。その場合は必ず事前にご連絡いたします。（※発送国共に変更→価格変更有）

◆海外買付の場合、注文確定完了後から平均2～3週間程度でお届けをします。
※お急ぎのお客様にはDHL, UPS等の速達便もご用意しております。（追加送料）
※税関・国際貨物の混み具合により、日数が前後する場合がございます。

◆ご注文確定後すぐに買付いたしますので、キャンセルは受付致しかねます。

◆ラッピング・レシートコピー等はご注文前にお申し付けいただいた方のみ対応させていただいております。

◆お客様のデバイス、モニター、設定環境等により、色味に若干の誤差が生じる場合がございます。

◆手作業で丁寧に検品をし梱包しておりますが、海外商品は日本基準よりも低いため、多少の傷・汚れやほつれ、タグがついていない、配送時のシワ、箱がつぶれているなどの場合があります。基本的にクレームとしてお受けできませんのでご了承下さい。

◆付属品は買付国店舗の「正規付属品」をお付けし発送いたします。
※商品カテゴリー、買付国によって付属品内容が異なる場合がありますことを予めご了承ください。

◆取引完了後、お客様のご都合による返品交換は受け付けておりませんので、予めご了承くださいませ。
在庫が完売の場合は、お取引キャンセルとなりバイマより返金対応となります。

◆100％正規品を取り扱っていますがご不安な方は「あんしんプラス」のご加入をお勧めしております。　
詳細はこちら→　https://qa.buyma.com/bm/anshin/1006.html

◆関税 （海外発送商品 対象）発生した場合はバイマの規約に基づきご購入者様負担となります。　詳細はこちら→　https://qa.buyma.com/buy/3105.html
";

        private const string サイズ情報テンプレート = @"サイズ：{サイズ情報}
＊商品の実寸には若干の誤差が生じる場合がございます。";

        private const string 色_サイズ情報テンプレート = @"※こちらは受注確定後に買い付け、お取り寄せを致します。
注文前に必ず在庫状況をお問い合わせ下さい。

※ご注文の前に必ず【お取引について】をご一読ください。

+‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥+

【商品について】
ブランド名：{ブランド名}
商品名：{簡易商品名}
カラー：{色名}
素材：{素材}
{サイズ}

◆{簡易コメント}

+‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥+

【こちらから当店の新着・人気商品をご覧いただけます】

◆新着商品◆
https://www.buyma.com/buyer/12144887/item_1.html

◆人気商品◆
https://www.buyma.com/r/-B12144887O1/

◆HERMES◆
https://www.buyma.com/r/_HERMES-%E3%82%A8%E3%83%AB%E3%83%A1%E3%82%B9/-B12144887/

◆CHANEL◆
https://www.buyma.com/r/_CHANEL-%E3%82%B7%E3%83%A3%E3%83%8D%E3%83%AB/-B12144887/

+‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥‥+

気になる商品がございましたら、お気軽にお問い合わせ下さいませ。
";

    }
}