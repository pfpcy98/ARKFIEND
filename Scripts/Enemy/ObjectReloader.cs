using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReloader : MonoBehaviour
{
    [SerializeField]
    private GameObject reloadObject;

    [SerializeField]
    private float reloadTime = 30.0f;

    private float stack_ReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        stack_ReloadTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadObject != null)
        {
            if (!reloadObject.activeInHierarchy)
            {
                stack_ReloadTime += Time.deltaTime;
                if (stack_ReloadTime >= reloadTime)
                {
                    reloadObject.SetActive(true);
                    stack_ReloadTime = 0.0f;
                }
            }
        }
    }
}
