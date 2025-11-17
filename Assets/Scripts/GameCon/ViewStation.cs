using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewStation : MonoBehaviour
{
    [Serializable]
    public class AnimatorEntire
    {
        public string Name;
        public Animator Animator;
    }
    public AnimatorEntire[] animator;
    private Dictionary<string,Animator>
        animatorDict = new Dictionary<string,Animator>();

    private void Start()
    {
        foreach (var Animator in animator)
        {
            if (!animatorDict.ContainsKey(Animator.Name))
            {
                animatorDict[Animator.Name] = Animator.Animator;
            }
        }
    }
}
