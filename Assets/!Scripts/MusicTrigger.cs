using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string areaName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.ChangeMusic(areaName);
        }
    }
}
