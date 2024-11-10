using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ImageEditor
{
    public class OneImageEditorSizeHandleButton : MonoBehaviour, IPointerDownHandler
    {
        public IObservable<PointerEventData> OnPointerDownRx => _onPointerDown;
        private readonly Subject<PointerEventData> _onPointerDown = new();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown.OnNext(eventData);
        }
    }
}