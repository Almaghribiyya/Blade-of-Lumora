using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sourcePlayerSword;
    public AudioSource sourceEnemySword;
    public AudioSource sourcePlayerDeath;
    public AudioSource sourceEnemyDeath;
    public AudioSource sourceEnemyHurt;
    public AudioSource sourcePlayerHurt;
    public AudioSource sourcePlayerJump;
    public AudioSource sourceCoinCollected;

    public void PlayAudioPlayerSword()
    {
        sourcePlayerSword.Play();
    }

    public void PlayAudioEnemySword()
    {
        sourceEnemySword.Play();
    }

    public void PlayAudioPlayerDeath()
    {
        sourcePlayerDeath.Play();
    }

    public void PlayAudioEnemyDeath()
    {
        sourceEnemyDeath.Play();
    }

    public void PlayAudioPlayerHurt()
    {
        sourcePlayerHurt.Play();
    }

    public void PlayAudioPlayerJump()
    {
        sourcePlayerJump.Play();
    }

    public void PlayAudioEnemyHurt()
    {
        sourceEnemyHurt.Play();
    }

    public void PlayAudioCoinCollected()
    {
        sourceCoinCollected.Play();
    }
}
