using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    public class Universe : MonoBehaviour
    {
        public string Name;
        public bool IsPressed = false;
        private Vector3 _initialPos = Vector3.zero;

        void Start()
        {
            _initialPos = this.transform.localPosition;
        }

        public void OnMouseUp()
        {
            if (IsPressed)
                return;

            Press();

            Universe[] universes = GameObject.FindObjectsOfType<Universe>();

            foreach (Universe universe in universes)
            {
                if (universe != this)
                    universe.Unpress();
            }
        }

        public void Press()
        {
            IsPressed = true;
            this.transform.localPosition = new Vector3(_initialPos.x, 0.5f, _initialPos.z);

            if (!String.IsNullOrEmpty(this.Name))
            {
                Cubeat cubeat = GameObject.FindObjectOfType<Cubeat>();

                cubeat.map.ReadSamples(this.Name);
                cubeat.ComputeCameraPosition();
            }
        }

        public void Unpress()
        {
            IsPressed = false;
            this.transform.localPosition = new Vector3(_initialPos.x, 1f, _initialPos.z);
        }
    }
}
