using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public enum SonidosGenerales {Comer, Exito, Fallo, Vender, RegaloHerramienta };

    // Attributes
    #region Attributes
    public AudioSource musicAS;
	public AudioSource fxAS;
    private static SoundManager instance;
	public AudioClip [] clips;
    public AudioClip exito;
    public AudioClip herramientaAcabada;
    public AudioClip regaloHerramienta;
    public AudioClip sndComer;
    public AudioClip sndVender;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<SoundManager>();
                    singleton.name = "SoundManager";             
                    Debug.Log("[SoundManager] Se agrego un nuevo SoundManager a la escena");
                }
            }
            return instance;
        }
        
    }
    #endregion

    // Methods
    #region Methods
    // Use this for initialization
    void Start () {
        instance = this;
		NotificationCenter.DefaultCenter().AddObserver(this, "PlaySound");
		NotificationCenter.DefaultCenter().AddObserver(this, "PlaySoundWithDelay");
		NotificationCenter.DefaultCenter().AddObserver(this, "PlaySoundLoop");
		NotificationCenter.DefaultCenter().AddObserver(this, "StopSoundLoop");
	}

	public void PlaySound(Notification notification) {
		if(notification.data != null) {
			fxAS.PlayOneShot(clips[(int)notification.data]);
		}
	}

	public void PlaySoundWithDelay(Notification notification) {
		StartCoroutine("DelaySound", notification.data as object[]);
	}

    internal static void PlayAndThenInvoke(Component source, int index, Action p)
    {
        PlaySound(source, index);
        Instance.Invoke(p, Instance.clips[index].length);
        
    }

    IEnumerator DelaySound(object [] _notification) {

		if(_notification != null) {
			yield return new WaitForSeconds((float)_notification[0]);
			fxAS.PlayOneShot(clips[(int)_notification[1]]);
		}
	}

	public void PlaySoundLoop(Notification notification) {
		if(notification.data != null) {
			fxAS.loop = true;
			fxAS.clip = clips[(int)notification.data];
			fxAS.Play();
		}
	}

	public void StopSoundLoop(Notification notification) {
		if(notification.data != null) {
			fxAS.loop = false;
			fxAS.Stop();
		}
	}

    public static void PlaySound(Component sender, int index)
    {
        NotificationCenter.DefaultCenter().PostNotification(sender, "PlaySound", index);
    }

    public static void PlaySound(AudioClip audioClip)
    {
        if(audioClip != null)
            Instance.fxAS.PlayOneShot(audioClip);
    }

    public static void PlaySound(SonidosGenerales sonido)
    {
        try
        {
            switch (sonido)
            {
                case SonidosGenerales.Comer:
                    PlayComer();
                    break;
                case SonidosGenerales.Exito:
                    PlayExito();
                    break;
                case SonidosGenerales.Fallo:
                    break;
                case SonidosGenerales.Vender:
                    PlayVender();
                    break;
                case SonidosGenerales.RegaloHerramienta:
                    PlayRegaloHerramienta();
                    break;
                default:
                    break;
            }
        }
        catch (TsiunasException e)
        {
            e.Tratar();
        }
    }

    internal static void PlayComer()
    {
        if (Instance.sndComer != null)
                PlaySound(Instance.sndComer);
            else
                throw new TsiunasException("No se ha establecido el sonido de COMIDA", false, "SOUND_MANAGER", "TODOS");        
    }

    internal static void PlayVender()
    {
        if (Instance.sndVender != null)
            PlaySound(Instance.sndVender);
        else
            throw new TsiunasException("No se ha establecido el sonido de VENDER", false, "SOUND_MANAGER", "TODOS");
    }

    public static void PlayExito()
    {
        if (Instance.exito != null)
            PlaySound(Instance.exito);
        else
            throw new TsiunasException("No se ha establecido el sonido de EXITO", false, "SOUND_MANAGER", "TODOS");       
        
    }

    public static void PlayHerramientaAcabada()
    {
        try
        {
            if (Instance.herramientaAcabada != null)
                PlaySound(Instance.herramientaAcabada);
            else
                throw new TsiunasException("No se ha establecido el sonido de HERRAMIENTA ACABADA", false, "SOUND_MANAGER", "TODOS");
        }
        catch (TsiunasException e)
        {
            e.Tratar();
        }
    }

    public static void PlayRegaloHerramienta()
    {
        try
        {
            if (Instance.regaloHerramienta != null)
                PlaySound(Instance.regaloHerramienta);
            else
                throw new TsiunasException("No se ha establecido el sonido de REGALO HERRAMIENTA", false, "SOUND_MANAGER", "TODOS");
        }
        catch (TsiunasException e)
        {
            e.Tratar();
        }
    }
    #endregion
}
