﻿using System.Collections;
using UnityEngine;

namespace Toolbox
{
    public class ScreenTransitionBarsDemo : MonoBehaviour
    {
        public ScreenTransitionBars screenTransition;
        public float hideTime = 2f;
        public float restartTime = 3.5f;
        public Color[] startColors;
        public Color[] endColors;

        int curColor;

        void Start()
        {
            StartCoroutine(Loop());
        }

        IEnumerator Loop()
        {
            screenTransition.start = startColors[curColor];
            screenTransition.end = endColors[curColor];

            StartCoroutine(ShowTransition());
            StartCoroutine(HideTransition());

            curColor = (curColor + 1) % startColors.Length;

            yield return new WaitForSeconds(restartTime);

            StartCoroutine(Loop());
        }

        IEnumerator ShowTransition()
        {
            yield return new WaitForEndOfFrame();
            screenTransition.Show();
        }

        IEnumerator HideTransition()
        {
            yield return new WaitForSeconds(hideTime);
            screenTransition.Hide();
        }
    }
}