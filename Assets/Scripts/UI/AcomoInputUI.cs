using TMPro;
using UnityEngine;

namespace UI
{
    public class AcomoInputUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField 元データ;

        [SerializeField] private TMP_Text 商品URL;
        
        [SerializeField] private TMP_Text 商品名;
        
        [SerializeField] private TMP_InputField 商品コメント素材;
        [SerializeField] private TMP_InputField 商品コメント色;
        [SerializeField] private TMP_InputField 商品コメント簡易説明;

        [SerializeField] private TMP_Text 商品コメント;

        [SerializeField] private TMP_Text カテゴリ1;
        [SerializeField] private TMP_Text カテゴリ2;
        [SerializeField] private TMP_Text カテゴリ3;

        [SerializeField] private TMP_Text ブランド;

        [SerializeField] private TMP_Text シーズン;

        [SerializeField] private TMP_Text 価格;


        [SerializeField] private TMP_Text 買付メモ;
    }
}