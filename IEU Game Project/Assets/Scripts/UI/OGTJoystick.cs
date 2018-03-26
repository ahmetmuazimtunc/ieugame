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
            transform.position = Input.mousePosition;
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

            transform.position = new Vector3(currentX, currentY);

            //RenderAngle(); //BURADA KALDIN .... ACİLARİ DA ALDIR BİTSİN!...
        }

        /// <summary>
        /// iki vektoru cizip acisini aliyor
        /// </summary>
        private void RenderAngle(Vector3 vectorOne , Vector3 vectorTwo)
        {
            OgtMathHelper.DebugAngleAndVisualizeVectors(vectorOne, vectorTwo);
        }

        public void OnPointerUp(PointerEventData data)
        {
            Debug.Log("up triggered");
            transform.position = _origin.position;
        }


        public void OnPointerDown(PointerEventData data)
        {

        }
    }
}