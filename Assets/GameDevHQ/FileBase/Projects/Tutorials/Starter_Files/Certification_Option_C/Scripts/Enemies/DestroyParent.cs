using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private void Update()
    {
        CheckChildren();
    }

    private void CheckChildren()
    {
        if (transform.childCount < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
