using System;
using UnityEngine;
using UnityEngine.UI;

namespace ImageEditor
{
    public class OneImageEditor : MonoBehaviour
    {
        [SerializeField] private Texture2D tmpEditImage;
        
        
        [SerializeField] private RawImage rawImage;

        private void Awake()
        {
            if (tmpEditImage != null)
            {
                SetImage(tmpEditImage);
            }
        }


        public void SetImage(Texture2D image)
        {
            rawImage.texture = image;
            // 画像のサイズに合わせてRawImageのサイズを変更
            RectTransform rectTransform = rawImage.rectTransform;
            
            // サイズの合計が300を超えないように調整
            float scale = 300f / (image.width + image.height);
            rectTransform.sizeDelta = new Vector2(image.width * scale, image.height * scale);
        }
    }
}