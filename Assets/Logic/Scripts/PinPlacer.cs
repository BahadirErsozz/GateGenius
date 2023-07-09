using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace logic
{



    public class PinPlacer : MonoBehaviour
    {
        [SerializeField] private int numberOfPins;
        [SerializeField] Pin PinPrefab;
        [SerializeField] Transform PinHolder;
        [SerializeField] Transform Platform;




        List<Pin> Pins;

        private void Awake()
        {
            Pins = new List<Pin>();
            float padding = 0.0001f;
            float startingPos = Platform.position.y + Platform.GetComponent<Renderer>().bounds.size.y/2;

            for(int i = 0; i < numberOfPins; i++)
            {
                startingPos -= padding+ 0.4f;
                Pin newPin = AddPin();
                newPin.SetUp("Input Pin");
                newPin.transform.position = new Vector3(newPin.transform.position.x, startingPos, newPin.transform.position.z);
                Pins.Add(newPin);
            }

        }

       Pin AddPin()
       {
            var pin = Instantiate(PinPrefab, parent:PinHolder);
            pin.transform.position = new Vector3(PinHolder.position.x, PinHolder.position.y, PinHolder.position.z+0.38f);

           return pin;
       }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
