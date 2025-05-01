using UnityEngine;

public class ProximityMusicTrigger : MonoBehaviour
{
    public AudioClip proximityMusic;
    public float triggerRadius = 5f;
    public Transform player;

    private bool isPlayerInside = false;

    void Update()
    {
        if (player == null || proximityMusic == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < triggerRadius && !isPlayerInside)
        {
            isPlayerInside = true;
            AudioManager.instance.OverrideMusic(proximityMusic);
        }
        else if (distance >= triggerRadius && isPlayerInside)
        {
            isPlayerInside = false;
            AudioManager.instance.RevertToAreaMusic();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
