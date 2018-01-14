using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {

	//ManagerObjectAudioSource usage:
	//	this uses [0] for BGM
	//	Wave manager uses [1] for death sounds

	private static Dictionary<string, AudioClip> soundEffects;
	private static Dictionary<string, AudioClip> musics;

	public static AudioSource bgmSource; 

	public static void Initialize(GameObject menuManagerObject){
		soundEffects = new Dictionary<string, AudioClip>{ };
		musics = new Dictionary<string, AudioClip>{ };

		bgmSource = menuManagerObject.GetComponents<AudioSource> ()[0];

		//TODO: add system to read these from level.txt
		musics.Add("menuTheme1", LoadAudio("BGM/T123-CharSelect"));
		musics.Add("battleBGM11",LoadAudio("BGM/T10-NativeFaithIntense"));
		musics.Add("battleBGM12",LoadAudio("BGM/Hanaya-BoL"));

		soundEffects.Add("enemyHit", LoadAudio("HitSounds/ding"));
		soundEffects.Add("gunFireDefault", LoadAudio("HitSounds/GunShot"));
		soundEffects.Add ("enemyDeath", LoadAudio ("HitSounds/DeathSnap"));
		soundEffects.Add ("playerOnHit", LoadAudio ("HitSounds/PlayerPunched"));
	}

	public static AudioClip GetSoundEffect(string entry){
		return soundEffects [entry];
	}

	public static void PlayMusic(string entry){
		if (entry == null) {
			bgmSource.Stop ();
		} else{
			bgmSource.Stop ();
			AudioClip music = musics [entry];
			bgmSource.clip = music;
			bgmSource.Play ();
		}
	}

	private static AudioClip LoadAudio(string file){
		AudioClip audio = (AudioClip)Resources.Load ("Sounds/"+file);
		if (audio == null) {
			Debug.LogError ("Audio not found "+file);
		}
		return audio;
	}
}
