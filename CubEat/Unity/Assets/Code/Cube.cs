using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    public int Layer;// { get; set; }
    public int NumberOnLayer;// { get; set; }

    private bool _isEmpty = true;
    public bool IsEmitting;// { get; set; }
    public bool IsInPlayedTime;// { get; set; }
    public bool IsOnMeasure;// { get; set; }
    public Color Color { get; set; }

    public bool IsEmpty 
    {
        get
        {
            return _isEmpty;
        }
        set
        {
            _isEmpty = value;
            if (!_isEmpty)
                this.gameObject.renderer.material.color = Color;
            else
                this.gameObject.renderer.material.color = Color.Lerp(Color, Color.black, 0.5f);
        }
    }

    public void OnMouseDown()
    {
        //IsEmpty = !IsEmpty;

    }

    public void OnMouseUp()
    {
        IsEmpty = !IsEmpty;
    }

}
