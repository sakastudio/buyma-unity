using System;

namespace Server
{
    [Serializable]
    public class SendBuymData
    {
        public string 商品名;
        public string 商品コメント;

        public string 第一カテゴリ;
        public string 第二カテゴリ;
        public string 第三カテゴリ;
        
        public string ブランド;
        
        public string シーズン;

        public string 色_サイズ情報;
        
        public int 価格;

        public string 出品メモ;
    }
}