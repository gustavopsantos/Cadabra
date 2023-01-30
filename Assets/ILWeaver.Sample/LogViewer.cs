using System.Collections.Generic;
using UnityEngine;

namespace ILWeaver.Sample
{
    public class LogViewer : MonoBehaviour
    {
        private readonly List<string> _messages = new();

        private void Awake()
        {
            Application.logMessageReceived += (message, _, _) => { _messages.Add(message); };
        }

        private void OnGUI()
        {
            using (new GUILayout.AreaScope(new Rect(0, 0, Screen.width, Screen.height)))
            {
                foreach (var message in _messages)
                {
                    GUILayout.Label(message);
                }
            }
        }
    }
}
