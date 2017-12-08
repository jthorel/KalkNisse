using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NissensAnimScript : MonoBehaviour {

	Rigidbody2D rb;
	public bool IsGrounded;
	public Animator anim;
	public float animationKoeff; //0.5
	private AudioSource sourceWind;
	private AudioSource sourceSpeed;
	private AudioSource sourceCrash;
	private AudioSource sourceWoohoo;
	private AudioSource sourceFalling;

	public float soundVolumeSpeedSound; //0.003
	public float speedVolume; //0.5
	private bool crashed;
	private bool inTheAir;

	public float windVolume; //1
	public float crashVolume; //0.2
	public float wohooVolume; //1
	public float jabbaDabbaDo; //0.007
	public float fallingVolume; //

	public float speedLimitsLower; //0
	public float speedLimitsUpper; // 100?

	private bool alreadyPlayed;

	// Use this for initialization
	void Start () {
		//print ("Start");
		rb = GetComponent<Rigidbody2D>();
		anim.enabled = false;

		AudioSource[] audioFiles = GetComponents<AudioSource>();
		sourceWind = audioFiles[0];
		sourceWind.volume = windVolume;
		sourceSpeed = audioFiles[1];
		sourceSpeed.volume = speedVolume;
		sourceCrash = audioFiles[2];
		sourceCrash.volume = crashVolume;
		sourceWoohoo = audioFiles[3];
		sourceWoohoo.volume = wohooVolume;
		sourceFalling = audioFiles[4];
		sourceFalling.volume = fallingVolume;

		alreadyPlayed = false;
		sourceWind.Play ();
		inTheAir = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsGrounded) {
			anim.speed = rb.velocity.x * animationKoeff;
			speedVolume = Mathf.Clamp(anim.speed * soundVolumeSpeedSound,speedLimitsLower ,speedLimitsUpper );
			sourceSpeed.Play();
			sourceSpeed.volume = speedVolume;
			if (speedVolume > jabbaDabbaDo) {
				if (!alreadyPlayed) {
					sourceWoohoo.Play ();
					//print ("Jabbadabbadoooooooo");
					alreadyPlayed = true;
				}
			}else{
				alreadyPlayed = false;
			}
		}
	}
		
	void OnCollisionStay2D(Collision2D collisionInfo)
	{
		if(!IsGrounded){
			sourceCrash.Play();
			Handheld.Vibrate ();
			crashed = false;
			sourceFalling.Stop ();
		}
		IsGrounded = true;
		anim.enabled = true;
		inTheAir = false;
	}
		
	void OnCollisionExit2D(Collision2D collisionInfo)
	{
		IsGrounded = false;
		crashed = false;
		sourceSpeed.Stop ();

		if(!inTheAir){
			sourceFalling.Play();	
			inTheAir = true;
		}
	}
}











//source.pitch = Random.Range (lowPitchRange,highPitchRange);
//float hitVol = coll.relativeVelocity.magnitude * velToVol;
//if (coll.relativeVelocity.magnitude < velocityClipSplit)
	//source.PlayOneShot(crashSoft,hitVol);