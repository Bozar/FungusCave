using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameMessage : MonoBehaviour
    {
        private int logHeight;
        private List<string> messages;

        public string[] GetMessageText(int line)
        {
            string[] output;

            if (messages.Count > line)
            {
                IEnumerable<string> tmp = messages
                    .Skip(messages.Count - line - 1)
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
            messages.Add(text);

            if (messages.Count > logHeight * 3)
            {
                IEnumerable<string> tmp = messages
                    .Skip(messages.Count - logHeight - 1)
                    .Take(logHeight);
                messages = tmp.ToList();
            }
        }

        private void Awake()
        {
            messages = new List<string>();
            logHeight = 15;
        }
    }
}
