using System;
using System.Threading;
using UnityEngine;

namespace Cadabra.Sample
{
    public class Player : MonoBehaviour
    {
        [Benchmark] 
        private void Start()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(120));
        }
    }
}