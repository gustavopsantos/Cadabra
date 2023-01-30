using System;
using System.Threading;
using ILWeaver.Attributes;
using UnityEngine;

namespace ILWeaver.Sample
{
    public class Benchy : MonoBehaviour
    {
        [Benchmark]
        private void Start()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }
    }
}