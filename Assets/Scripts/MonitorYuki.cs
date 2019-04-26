using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class MonitorYuki : MonoBehaviour
    {
        public void MonitorHaruhi_HaruhiSays(object sender, SendMessageEventArgs e)
        {
            string haruhi = $"Hello, {e.Message}!";
            string yuki = "Hello, Haruhi!";

            Debug.Log(haruhi);
            Debug.Log(yuki);
        }
    }
}
