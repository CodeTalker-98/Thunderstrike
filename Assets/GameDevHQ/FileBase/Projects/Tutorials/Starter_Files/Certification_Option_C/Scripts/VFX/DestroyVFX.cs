using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    //Play Sound @ start??

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
