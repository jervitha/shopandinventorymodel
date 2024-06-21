using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
    
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shopCancellationaudioClip;
    [SerializeField] private AudioClip shopConfirmationaudioClip;


    public void PlaySound(SoundType soundType)
    {
        audioSource.clip = GetSound(soundType);
        audioSource.Play();

    }

    private AudioClip GetSound(SoundType soundType)
    {
        switch(soundType)
        {
            case SoundType.Cancellation:
                return shopCancellationaudioClip;
            case SoundType.Confirmation:
                return shopConfirmationaudioClip;
            default:
                return null;

        }
    }

   
}


public enum SoundType
{
    Cancellation,
    Confirmation

};