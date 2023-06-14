using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildingHealth : MonoBehaviour
{
    // a reference to the health bar controller script.
    public EnemyHealthBarController healthBar;
    // a reference to the ui manager script.
    public UIManager uiManager;
    // some transform variables which will be used to track the missile distence and instantiate smoke.
    public Transform smokeSpawn, destroyedSmoke, missile;
    // game object variables which will be used to hold prefabs and references to each planet base.
    public GameObject smoke, smoke2, smoke3, smoke4, finalSmoke, explosionPrefab, planet1Base, planet2Base, planet3Base;
    // set heath and damage amounts.
    int p1MaxHp = 50, p2MaxHp = 100, p1HQMaxHp = 100, smokeObjNum = 1, smokeObjNum1 = 1, smokeObjNum2 = 1, maxHp, damage = 50;
    // hold current heath points.
    public int currentHp;
    // hold current distence to missile.
    public Vector3 distToMissile;
    // check if damage can be applied
    bool canApplyDamage = true;
    // damage timer
    float damageTimer = 5f;
    // audio source to hold the explosion sound effect.
    public AudioSource explosion;
    // Start is called before the first frame update
    void Start()
    {
        // set missile transform variable to game object with tag of missile.
        missile = GameObject.FindWithTag("missile").transform;
        // check if game object this script is attached to has the tag of hq or a standard building and set health accordingly.
        if (gameObject.tag == "planet1hq")
        {
            currentHp = p1HQMaxHp;
            maxHp = currentHp;
        }
        if (gameObject.tag == "planet1standard")
        {
            currentHp = p1MaxHp;
            maxHp = currentHp;
        }
        // access healthbar script and call set max health function passing in the current hp to satisfy parameter.
        healthBar.SetMaxHealth(currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        // if can apply damage is false run timer
        if (canApplyDamage == false)
        {
            damageTimer -= 1f * Time.deltaTime;
            // once timer is 0 damage can be applied so set bool to true and reset timer.
            // this is required so that damage is only applied once per collision.
            if (damageTimer <= 0)
            {
                canApplyDamage = true;
                damageTimer = 5f;
            }
        }
        // calulate distence from this game object to the missile.
        distToMissile = transform.position - missile.transform.position;
        // check that the magnitude of the missile is < 8 and a collision has occured and that damage can be applied if these conditions are met run code.
        // the distence to missile is required so that spash damage can be applied meaning greater damage to be applied to the building the closer the missile is when it explodes
        if (distToMissile.magnitude < 8 && missile.GetComponent<MissileScript>().missleCollison == true && canApplyDamage == true)
        {
            // call apply damage function and pass in a damage of 30 to be taken from the building health.
            ApplyDamageToEnemy(30);
            // update the player score ui adding on 15.
            uiManager.GetComponent<UIManager>().score += 15;
        }
        // these 2 works the same as above but pply less damage and give less score as the missile has landed further away.
        else if (distToMissile.magnitude < 10 && missile.GetComponent<MissileScript>().missleCollison == true && canApplyDamage == true)
        {
            ApplyDamageToEnemy(15);
            uiManager.GetComponent<UIManager>().score += 10;
        }
        else if (distToMissile.magnitude < 18 && missile.GetComponent<MissileScript>().missleCollison == true && canApplyDamage == true)
        {
            ApplyDamageToEnemy(10);
            uiManager.GetComponent<UIManager>().score += 5;
        }
    }
    // this will be triggered when a collision has been 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "missile")
        {
            // detected detect when the building has been directly hit by the missile.
            if (currentHp > 0)
            {
                // if it has and hp is above 0 then pass damage into apply damage function and call it.
                ApplyDamageToEnemy(damage);
                // update player score.
                uiManager.GetComponent<UIManager>().score += 30;
            }
        }
    }
    // this function handles applying damage to the enemy buldings.
    public void ApplyDamageToEnemy(int damage) // damage is taken in a float and will apply the damage if object has more than 0 HP
    {
        // if health is less than 0 return.
        if (currentHp < 0f)
        {
            return;
        }
        // take value of damage away from current health.
        currentHp -= damage;
        // update health bar UI.
        healthBar.healthSlider.value = currentHp;
        // damage has been applied set this bool back to false.
        canApplyDamage = false;
        // this block of code handles activating the fire visuals on the building to give the appearence of building damage.
        // if the building has the tag of a standard building run code.
        if (gameObject.tag == "planet1standard")
        {
            // if health is less than or equal to 40 set the prefab contained in smoke to true.
            if (currentHp <= 40)
            {
                smoke.SetActive(true);
            }
            // works the same as above.
            if (currentHp <= 25)
            {
                smoke2.SetActive(true);
            }
            if (currentHp <= 10)
            {
                smoke3.SetActive(true);
            }
        }
        // works the same as above but for the HQ buildings.
        if (gameObject.tag == "planet1hq")
        {
            if (currentHp <= 80)
            {
                smoke.SetActive(true);
            }
            if (currentHp <= 60)
            {
                smoke2.SetActive(true);
            }
            if (currentHp <= 40)
            {
                smoke3.SetActive(true);
            }
            if (currentHp <= 30)
            {
                smoke4.SetActive(true);
            }
            if (currentHp <= 0)
            {
                
            }
        }
        // once health of building has reached 0 run code.
        if (currentHp <= 0)
        {
            // if building this script is attached to is a standard building add 40 to player score.
            if (gameObject.tag == "planet1standard")
            {
                uiManager.GetComponent<UIManager>().score += 40;
            }
            // if it is a hq building add 80 to player score.
            if (gameObject.tag == "planet1hq")
            {
                uiManager.GetComponent<UIManager>().score += 80;
            }
            // create a temp game object to hold the prefab exploision instance.
            GameObject tempFlash;
            // instantiate explosion effect at the buildings transform and rotation.
            tempFlash = Instantiate(explosionPrefab, smokeSpawn.transform.position, smokeSpawn.transform.rotation);
            // instantiate fire prefab this will be left remain in the buildings place.
            Instantiate(finalSmoke, destroyedSmoke.position, destroyedSmoke.rotation);
            // stop the missile explosion audio.
            missile.GetComponent<MissileScript>().explode.Stop();
            // play the building explosion audio.
            explosion.Play();
            // destroy the bulding game object.
            Destroy(gameObject);
            //Destroy the muzzle flash effect
            Destroy(tempFlash, 4f);
            // if planet 1 base is not destoryed run code.
            if (missile.GetComponent<MissileScript>().planet1Destoryed == false)
            {
                // - 1 from the remaining buildings held in enemy base script for planet 1.
                planet1Base.GetComponent<EnemyBases>().planet1Base -= 1;
                // call check remaining buildings in enemybase script to see if base has been destoryed.
                planet1Base.GetComponent<EnemyBases>().CheckRemainingBuildings();
            }
            // if planet 2 is not destoryed but planet 1 is run code check needs to be made to see if planet 1 base has been destoryed to stop script taking away building number
            // from planet 2 prior to planet 1 bases destruction.
            else if (missile.GetComponent<MissileScript>().planet2Destroyed == false && missile.GetComponent<MissileScript>().planet1Destoryed == true)
            {
                planet2Base.GetComponent<EnemyBases>().planet2Base -= 1;
                planet2Base.GetComponent<EnemyBases>().CheckRemainingBuildings();
            }
            // if planet 3 base is not destoryed but planet 2 base is fun code.
            else if (missile.GetComponent<MissileScript>().planet3Destroyed == false && missile.GetComponent<MissileScript>().planet2Destroyed == true)
            {
                planet3Base.GetComponent<EnemyBases>().planet3Base -= 1;
                planet3Base.GetComponent<EnemyBases>().CheckRemainingBuildings();
            }
        }
    }
}
