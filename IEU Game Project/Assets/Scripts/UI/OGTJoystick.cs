using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class OGTJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public int _halfRadius = 100;

        [SerializeField]
        private Transform _origin = null;

        private Vector3 _xCoordinate = new Vector3(10, 0, 0);
        private Vector3 _yCoordinate = new Vector3(0, 10, 0);

        private /*const*/ float _angleOrigin_xAxis = 0f;        

        ////custom Koordinat sistemimizi cizdirmek icin ne kadar kaydiracagiz orjinal koordinat sistemi merkezininin x'ini
        //[SerializeField]
        //private float _coorXOffset = 0f;

        ////custom Koordinat sistemimizi cizdirmek icin ne kadar kaydiracagiz orjinal koordinat sistemi merkezininin y'sini
        //[SerializeField]
        //private float _coorYOffset = 0f;

        void Start()
        {
            //_coorXOffset = _origin.position.x / 2;
            //_coorYOffset = _origin.position.y / 2;
            _angleOrigin_xAxis = Vector3.Angle(_origin.position, _xCoordinate);
        }

        void Update()
        {
            DrawCoordinateSystem();
            DrawJoystickRadiusAndVectors();
        }

        //joystickimizin sinirlarini gosteren kareyi View'a cizdiren method
        private void DrawJoystickRadiusAndVectors()
        {
            for (int i = 0; i < 4; i++)
            {
                //Kare kenarlarımızın koordinat noktalari
                float xOne, yOne;
                //eger ilk veya son kenar ise; x = orjin - bir kenar / 2
                if (i == 0 | i == 3)
                {
                    xOne = _origin.position.x - _halfRadius; 
                }
                else
                {
                    xOne = _origin.position.x + _halfRadius;
                }
                //eger 1. ve 2. kenari cizidiriyorsak (yani index 0 ve 1)
                if (i <= 1)
                {
                    //kenarlarin baslangic noktasi ayni ve karenin ust yarisinda olacak
                    yOne = _origin.position.y + _halfRadius; 
                }
                else//3. ve 4. kenari cizidiriyorsak
                {
                    //kenarlarin baslangic noktasi ayni ve karenin alt kisminda olacak
                    yOne = _origin.position.y - _halfRadius;
                }
                //cizginin ilk noktasi
                Vector3 CoordinateOne = new Vector3(xOne, yOne, 0);
                //cizginin ikinci noktasi
                Vector3 CoordinateTwo = new Vector3(CoordinateOne.x, CoordinateOne.y, 0);

                //eger i cift ise yani 0 veya 2 ise; yani 1. veya 3. kenarlar cizilmekte ise
                if (i % 2 == 0)
                {
                    //1. kenar ise toplayarak bir kenar uzunlugu ekle 
                    //2.kenar ise cikararak bir kenar uzunlugu ekle 
                    CoordinateTwo.x = xOne + ((i == 2) ? (-(_halfRadius * 2)) : (_halfRadius * 2));
                }
                else //eger tek ise
                {
                    //y'sine bir kenar uzunlugu ekle (2r)
                    //ama i == 1 ise cikararak , 3 ise toplayarak ekle
                    CoordinateTwo.y = yOne + ((i == 1) ? (-_halfRadius * 2) : (_halfRadius * 2));
                }
                Debug.DrawLine(CoordinateOne, CoordinateTwo);
            }
        }

        //Aci hesabinda kullandiğimiz koordinat sistemimizi ekrana cizdiren method
        private void DrawCoordinateSystem()
        {
            Debug.DrawLine(new Vector3(_origin.position.x - _halfRadius * 2, _origin.position.y, 0), new Vector3(_origin.position.x + _halfRadius * 2, _origin.position.y, 0));
            Debug.DrawLine(new Vector3(_origin.position.x, _origin.position.y - _halfRadius * 2, 0), new Vector3(_origin.position.x, _origin.position.y + _halfRadius * 2, 0));
        }

        public void OnDrag(PointerEventData data)
        {
            transform.position = data.position;
            float currentX = transform.position.x;
            float currentY = transform.position.y;
            //x kontrol
            if (currentX > _origin.position.x + _halfRadius)
            {
                currentX = _origin.position.x + _halfRadius;
            }
            else if (currentX < _origin.position.x - _halfRadius)
            {
                currentX = _origin.position.x - _halfRadius;
            }
            //y kontrol
            if (currentY > OgtMathHelper.ConvertToPositive(_origin.position.y + _halfRadius))
            {

                currentY = _origin.position.y + _halfRadius;
            }
            else if (currentY < _origin.position.y - _halfRadius)
            {
                currentY = _origin.position.y - _halfRadius;
            }

            Vector3 newPos = new Vector3(currentX, currentY);

            transform.position = newPos;

            float angle = CalculateJoystickAngle(newPos);

        }

        /// <summary>
        /// iki vektoru cizip acisini aliyor
        /// </summary>
        private float CalculateJoystickAngle(Vector3 joyStickVector)
        {
            //jostick'in vektoru ile , x ekseni arasındaki aci
            float angleJoystick_xAxis = Vector3.Angle(_xCoordinate, joyStickVector);
            //aci hesabi
            float angle = _angleOrigin_xAxis - angleJoystick_xAxis;
            //eger joystick-xEkeseni arasiindaki aci  , origin-xEkkseni arasindaki acidan buyuk ise
            if (angleJoystick_xAxis > _angleOrigin_xAxis)
            {
                //orjinin x-eksen ile yaptigi aciyi , 90dan cikarip;
                angle = 90 - _angleOrigin_xAxis;
                //sonuctan da joystick ile y ekseni arasindaki aciyi cikartiyoruz
                angle = angle - Vector3.Angle(joyStickVector, _yCoordinate);
            }
            return angle;
        }

        public void OnPointerUp(PointerEventData data)
        {
            //Debug.Log("up triggered");
            transform.position = _origin.position;
        }


        public void OnPointerDown(PointerEventData data)
        {

        }
    }
}