﻿//Timmy Chan and Arne-Martin
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public abstract partial class Enemy : MonoBehaviour, IDamagable
{
	NavMeshAgent agent;
	public Transform goal;
	public float startHealth = 40f;
	protected float health;
	public GameObject HealthBarPrefab;
	private Transform HeadsetPosition;
	private EnemyHealthBar healthBar;
	private EnemyManager enemyManager;
	private bool dying = false;
	private SkinnedMeshRenderer enemySkinnedMeshRenderer;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.destination = goal.position;
		enemyManager = transform.parent.GetComponent<EnemyManager>();

		//initiate healthbar and finding headsetposition, Arne-Martin
		healthBar = Instantiate(HealthBarPrefab, new Vector3(transform.position.x, transform.position.y + 2.4f, transform.position.z), Quaternion.identity)
			.GetComponent<EnemyHealthBar>();
		healthBar.transform.parent = transform;
		HeadsetPosition = Player.instance.trackingOriginTransform;
		health = startHealth;

		//Vector3 left = Quaternion.Inverse(InputTracking.GetLocalRotation(VRNode.LeftEye)) * InputTracking.GetLocalPosition(VRNode.LeftEye);
		SoundStart();

		float h, s, v; // hue, saturation, value (brightness)
		float oldV;
		enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

		for (int i = 0; i < enemySkinnedMeshRenderer.materials.Length; i++)
		{
			Color.RGBToHSV(enemySkinnedMeshRenderer.materials[i].color, out h, out s, out v);
			oldV = v;
			v = Random.Range(oldV * 0.5f, oldV * 1.5f);

			if (v > 1f)
				v = 1f;

			enemySkinnedMeshRenderer.materials[i].color = Color.HSVToRGB(h, s, v);
		}
	}

	void Update()
	{
		//To scale healthbar to health, Arne-Martin
		float healthPercentage = health / startHealth;
		healthBar.display(healthPercentage);
		//Rotate healthbar towards player, Arne-Martin
		Vector3 UpdatedHeadsetPosition = HeadsetPosition.position;
		healthBar.transform.LookAt(UpdatedHeadsetPosition);

		if (agent.remainingDistance < 1f)
		{
			print("goal");
			enemyManager.ReachedGoal(1);
			Destroy(gameObject);
		}

		if (health <= 0f)
		{
			//play die animation
			//instantiate particles
			if (!dying)
			{
				GetComponent<Animator>().Play("Die");
				GetComponent<NavMeshAgent>().speed = 0f;
				dying = true;
				transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
				GetComponentInChildren<MeshCollider>().enabled = false;
				Destroy(gameObject, 4f);
				PlayDeathSound();
			}
		}

		HandleIdleSound();
	}

	public void InflictDamage(float damage)
	{
		health -= damage;

		PlayHurtSound();
	}
}
