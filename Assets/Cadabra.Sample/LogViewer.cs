using System.Collections.Generic;
using UnityEngine;

namespace Cadabra.Sample
{
    public class LogViewer : MonoBehaviour
    {
        private readonly List<string> _messages = new();

        private void Awake()
        {
            Application.logMessageReceived += (message, trace, type) => { _messages.Add(message); };
        }

        private void OnGUI()
        {
            const int margin = 8;
            const int doubleMargin = margin * 2;

            var area = new Rect(margin, margin, Screen.width - doubleMargin, Screen.height - doubleMargin);
            using (new GUILayout.AreaScope(area))
            {
                foreach (var message in _messages)
                {
                    GUILayout.Label(message);
                }
            }
        }
    }
}