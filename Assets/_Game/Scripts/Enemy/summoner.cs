using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summoner : Enemy
{
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	private Vector2 targetPosition;
	private Animator anim;

	public float timeBetweenSummons;
	private float summonTime;

	public Enemy enemyToSummon;

	public float meleeAttackSpeed;
	public float stopDistance;
	private float attackTime;

	public override void Start()
	{
		base.Start();
		float randomX = Random.Range(minX, maxX);
		float randomY = Random.Range(minY, maxY);
		targetPosition = new Vector2(randomX, randomY);
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (player != null) 
		{
			if (Vector2.Distance(transform.position, targetPosition) > .5f)
			{
				transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
				anim.SetBool("isRunning", true);
			}
			else
			{
				anim.SetBool("isRunning",false);

				if (Time.time >= summonTime)
				{
					summonTime = Time.time + timeBetweenSummons;
					anim.SetTrigger("summon");
				}
			}

			if (Vector2.Distance(transform.position, player.transform.position) < stopDistance)
			{
				if (Time.time > attackTime)
				{
					attackTime = Time.time + timeBetweenAttacks;
					StartCoroutine(Attack());
				}
			}

		} 

	}
	public void Summon()
	{
		if (player != null)
		{
			Instantiate(enemyToSummon, transform.position, transform.rotation);
		}
	}
	IEnumerator Attack()
	{
		player.GetComponent<Player>().TakeDamage(damage);

		Vector2 originalPosition = transform.position;
		Vector2 targetPosition = player.position;

		float percent = 0;
		while (percent <= 1)
		{
			percent += Time.deltaTime * meleeAttackSpeed;
			float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
			transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
			yield return null;
		}
	}

}
