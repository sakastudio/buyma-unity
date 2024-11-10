using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ImageEditor
{
    public class OneImageEditorSizeHandle : MonoBehaviour
    {
        [SerializeField] private OneImageEditorSizeHandleButton rightUp;
        [SerializeField] private OneImageEditorSizeHandleButton rightDown;
        [SerializeField] private OneImageEditorSizeHandleButton leftUp;
        [SerializeField] private OneImageEditorSizeHandleButton leftDown;

        [SerializeField] private RectTransform handleObject;

        private CurrentHandle currentHandle = CurrentHandle.None;
        private Vector3 lastMousePosition;

        private void Awake()
        {
            rightUp.OnPointerDownRx.Subscribe(_ =>
            {
                currentHandle = CurrentHandle.RightUp;
                lastMousePosition = Input.mousePosition;
            });
            rightDown.OnPointerDownRx.Subscribe(_ =>
            {
                currentHandle = CurrentHandle.RightDown;
                lastMousePosition = Input.mousePosition;
            });
            leftUp.OnPointerDownRx.Subscribe(_ =>
            {
                currentHandle = CurrentHandle.LeftUp;
                lastMousePosition = Input.mousePosition;
            });
            leftDown.OnPointerDownRx.Subscribe(_ =>
            {
                currentHandle = CurrentHandle.LeftDown;
                lastMousePosition = Input.mousePosition;
            });
        }

        private void Update()
        {
            if (currentHandle == CurrentHandle.None)
            {
                return;
            }

            // マウスを離したらハンドルをリセット
            if (Input.GetMouseButtonUp(0))
            {
                currentHandle = CurrentHandle.None;
                return;
            }

            // マウスの移動量を取得（ローカル座標系で）
            Vector2 mousePosition = Input.mousePosition;
            RectTransform parentRect = handleObject.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, mousePosition, null, out Vector2 localMousePosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, lastMousePosition, null, out Vector2 localLastMousePosition);
            Vector2 deltaLocal = localMousePosition - localLastMousePosition;

            // アスペクト比1:1を維持しながらサイズを変更
            float delta = 0f;

            switch (currentHandle)
            {
                case CurrentHandle.RightUp:
                    delta = Mathf.Max(deltaLocal.x, deltaLocal.y);
                    handleObject.offsetMax += new Vector2(delta, delta);
                    break;
                case CurrentHandle.RightDown:
                    delta = Mathf.Max(deltaLocal.x, -deltaLocal.y);
                    handleObject.offsetMax += new Vector2(delta, -delta);
                    handleObject.offsetMin += new Vector2(0, delta);
                    break;
                case CurrentHandle.LeftUp:
                    delta = Mathf.Max(-deltaLocal.x, deltaLocal.y);
                    handleObject.offsetMin += new Vector2(-delta, 0);
                    handleObject.offsetMax += new Vector2(0, delta);
                    break;
                case CurrentHandle.LeftDown:
                    delta = Mathf.Max(-deltaLocal.x, -deltaLocal.y);
                    handleObject.offsetMin += new Vector2(-delta, -delta);
                    break;
            }

            // サイズが負になるのを防ぐ
            Vector2 size = handleObject.rect.size;
            if (size.x < 0)
            {
                float offset = handleObject.offsetMin.x;
                handleObject.offsetMin = new Vector2(handleObject.offsetMax.x, handleObject.offsetMin.y);
                handleObject.offsetMax = new Vector2(offset, handleObject.offsetMax.y);
            }

            if (size.y < 0)
            {
                float offset = handleObject.offsetMin.y;
                handleObject.offsetMin = new Vector2(handleObject.offsetMin.x, handleObject.offsetMax.y);
                handleObject.offsetMax = new Vector2(handleObject.offsetMax.x, offset);
            }

            // マウス位置を更新
            lastMousePosition = mousePosition;
        }


        enum CurrentHandle
        {
            None,
            RightUp,
            RightDown,
            LeftUp,
            LeftDown
        }
    }
}