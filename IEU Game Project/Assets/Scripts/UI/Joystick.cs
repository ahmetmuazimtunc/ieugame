using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public int MovementRange = 100;

        [SerializeField]
        private Vector3 _origin = Vector3.zero;

        [SerializeField]
        private Vector3 _xAxis = new Vector3(0, 2, 0);

        [SerializeField]
        private Vector3 _yAxis = new Vector3(2, 0, 0);

        void Start()
        {
            _origin = transform.position;

            DrawDebugAxis();
        }

        private void DrawDebugAxis()
        {
            Debug.DrawLine(transform.position, _xAxis);
            Debug.DrawLine(transform.position, _yAxis);
        }

        public void OnDrag(PointerEventData data)
        {

        }

        public void OnPointerUp(PointerEventData data)
        {
            transform.position = _origin;
        }


        public void OnPointerDown(PointerEventData data) { }
    }
}