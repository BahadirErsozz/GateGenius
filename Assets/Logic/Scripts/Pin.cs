using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace logic
{

    [SelectionBase]
    public class Pin : MonoBehaviour
    {
        public enum HighlightState { None, Highlighted, HighlightedInvalid }


        [SerializeField] float interactionRadius;
        [SerializeField] Color defaultCol;
        [SerializeField] Color highlightedCol;
        [SerializeField] Color highlightedInvalidCol;
        [SerializeField] MeshRenderer display;
        [SerializeField] HighlightState activeHighlightState;
        public bool IsHighlighted => activeHighlightState != HighlightState.None;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SetHighlightState(activeHighlightState);
        }

        public void SetUp(string name)
        {
            GetComponent<CircleCollider2D>().radius = interactionRadius;
            gameObject.name = $"Pin ({name})";
        }


        public void SetHighlightState(HighlightState state)
        {
            activeHighlightState = state;
            Color col = state switch
            {
                HighlightState.None => defaultCol,
                HighlightState.Highlighted => highlightedCol,
                HighlightState.HighlightedInvalid => highlightedInvalidCol,
                _ => Color.black
            };

            display.material.SetColor("_Color", col);
        }

    }
}
