using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestObjectControl : MonoBehaviour
{
    public _TestObjectComponentA componentA;
    public _TestObjectComponentB componentB;
    public List<_TestObjectComponentsBase> derrivedComponents;
    public List<_TestObjectComponentsBase> components;

    private void Awake()
    {
        SetUpRefs();
    }

    private void SetUpRefs()
    {
        derrivedComponents.Add(componentA = GetComponent<_TestObjectComponentA>());
        derrivedComponents.Add(componentB = GetComponent<_TestObjectComponentB>());
        foreach (var component in GetComponents<_TestObjectComponentsBase>())
        {
            components.Add(component);
        }
    }

    private void Update()
    {
        foreach (var component in derrivedComponents)
        {
            var t = component.GetType();
            var c = component as _TestObjectComponentB;
        }

        foreach (var component in components)
        {
            var t = component.GetType();
        }
    }

}
