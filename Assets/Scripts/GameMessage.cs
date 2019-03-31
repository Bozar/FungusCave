using Fungus.GameSystem.Render;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameMessage : MonoBehaviour
    {
        private List<string> messages;

        public string[] GetText(int line)
        {
            string[] output;

            if (messages.Count > line)
            {
                IEnumerable<string> tmp = messages
                    .Skip(messages.Count - line)
                    .Take(line);
                output = tmp.ToArray();
            }
            else
            {
                output = messages.ToArray();
            }
            return output;
        }

        public void StoreText(string text)
        {
            int height = GetComponent<UILog>().LogHeight;

            messages.Add(text);
            if (messages.Count > height * 3)
            {
                IEnumerable<string> tmp = messages
                    .Skip(messages.Count - height - 1)
                    .Take(height);
                messages = tmp.ToList();
            }
        }

        private void Awake()
        {
            messages = new List<string>();
        }
    }
}
