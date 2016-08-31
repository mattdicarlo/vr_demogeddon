using UnityEngine;

public class PokemonLoader : MonoBehaviour
{
    public GameObject[] potentialPokemon;

    private Animator pokeAnimator;

    public void Awake()
    {
        var poke = GameObject.Instantiate(potentialPokemon[Random.Range(0, potentialPokemon.Length)]);
        poke.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        poke.transform.position = this.transform.position;
        poke.transform.rotation = this.transform.rotation;

        poke.transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));

        poke.transform.parent = this.transform;

        pokeAnimator = GetComponent<Animator>();
        pokeAnimator.enabled = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Pokeball hitBall = collision.gameObject.GetComponent<Pokeball>();
        if (hitBall != null && collision.relativeVelocity.magnitude >= 1)
        {
            Debug.Log("Hit by pokeball");
            pokeAnimator.enabled = true;
            pokeAnimator.SetBool("catch", true);
            hitBall.CatchAnimation();
            Destroy(this.gameObject, .5f);
        }
    }
}
