using System;
using System.Collections;
using UnityEngine;

namespace Warlander.Minesweeper.Utils
{
    public class WarlanderBehaviour : MonoBehaviour
    {
        public void RunAfter(float time, Action action)
        {
            StartCoroutine(RunAfterCoroutine(time, action));
        }

        private IEnumerator RunAfterCoroutine(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }
    }
}