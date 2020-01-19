using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerHelper : MonoBehaviour
{

    private void OnEnable()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
