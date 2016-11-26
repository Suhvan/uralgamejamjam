
using UnityEngine;

public	class ZombieAgro : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		var z = other.gameObject.GetComponent<Zombie>();
		if (z != null)
		{
			z.Aggred = true;
		}
	}


	void OnTriggerExit2D(Collider2D other)
	{
		var z = other.gameObject.GetComponent<Zombie>();
		if (z != null)
		{
			z.Aggred = false;
		}
	}
}

