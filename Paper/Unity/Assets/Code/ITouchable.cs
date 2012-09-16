using UnityEngine;
using System.Collections;

public interface ITouchable
{
    void MouseDown(RaycastHit hit);
    void MouseUp(RaycastHit hit);
    void MouseMove(RaycastHit hit);
}
