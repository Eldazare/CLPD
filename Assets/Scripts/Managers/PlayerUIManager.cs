using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

	public Text ammoText;
	public Image ammoBar;
	public Text waveText;
	public Image healthBar;
	public Image armorBar;

	private PlayerBody player;
	private LevelManager currentLevel;
	private bool isInitialized = false;

	void LateUpdate(){
		if (isInitialized) {
			healthBar.fillAmount = player.healthCurrent / player.healthMax;
			waveText.text = "WAVE\n" + currentLevel.currentWave + " / " + currentLevel.finalWave;
			_Weapon currentWeapon = player.GetCurrentWeapon ();
			if (currentWeapon != null) {
				ammoText.text = currentWeapon.name + ": " + currentWeapon.ammo + " / " + currentWeapon.ammoMax;
				ammoBar.fillAmount = currentWeapon.ammo / currentWeapon.ammoMax;
			} else {
				ammoText.text = "No weapon";
				ammoBar.fillAmount = 0;
			}
			if (player.reloading) {
				ammoBar.fillAmount = 1 - (player.reloadLeft / player.reloadMax);
				ammoText.text = "RELOADING";
			}
			if (player.armor != null) {
				armorBar.fillAmount = player.armor.armorHPCurrent / player.armor.armorHP;
			}
		}
	}

	public void Initialize(PlayerBody player, LevelManager level){
		this.player = player;
		this.currentLevel = level;
		isInitialized = true;
	}
}
