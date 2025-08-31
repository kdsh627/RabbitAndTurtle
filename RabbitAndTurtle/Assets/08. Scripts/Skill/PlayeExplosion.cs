using UnityEngine;

public class PlayeExplosion : MonoBehaviour
{
    public void PlaySfx()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.WhaleLand);
    }
}
