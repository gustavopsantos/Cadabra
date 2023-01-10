using System;
using System.Threading;
using ILWeaver.Attributes;
using UnityEngine;

namespace ILWeaver.Sample
{
    public class Benchy : MonoBehaviour
    {
        private void Start()
        {
            Foo();
        }

        [Benchmark]
        private static void Foo()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }
    }
}