using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T:GenericSingleton<T>
{
   public static T Instance;
    public virtual void Awake()
    {
        if(Instance!=null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this as T;
        }

    }



}
