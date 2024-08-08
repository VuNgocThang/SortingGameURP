using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEvent : MonoBehaviour
{
    public Hammer hammer;

    public void ActiveVFX()
    {
        hammer.ActiveVFX();
    }

    public void DeactiveVFX()
    {
        hammer.DeactiveVFX();
    }
}
