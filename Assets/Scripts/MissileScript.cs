using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MissileScript : MonoBehaviour
{
    // these are the various game objects which the missile will need to be able to access from this script in order to function.
    public GameObject fpCam, fpCamHolder, tpCam, eBaseCam, planet1Cam, planet2Cam, planet3Cam, zoomCam, explosionPrefab, spawn, planet1Spawn, planet2Spawn, rocketFire, rocketSmoke, uiManager, pointLight, planet1, planet2,
        planet1orbit, planet2Orbit, planet2Base, planet3Base, enemyBase3, winPanel;
    // these will be used to store the world position of the spawn location and the previous launch position.
    public Transform previousLaunchPos, spawnLocation;
    // these floats will be used to store the min and max velocities the missile can achieve along with the coordinates it launch from for UI use.
    public float minVel = 0, maxVel = 1600f, minusVel = -800f, xCoor, zCoor, xCoorPrev, zCoorPrev;
    // this will control how quickly the missile is able to rotate when the player is lining up a shot.
    float rotationSpeed = 0.5f;
    // this will store the velocity of the missile
    public float velocity;
    // these will store bools so that true or false conditions can be used for various reasons.
    bool tpCamActive, fpCamActive, eCamActive, launched, isHomePlanetSpawn = true, isPlanet1Spawn, isPlanet2Spawn, stopExplosion, moveMissile;
    // these have been made public so they can be accessed from the inspector for testing purposes.
    public bool missleCollison, planet1Destoryed, planet2Destroyed, planet3Destroyed, isZoomed;
    // these are to be used for timers for when delays in processes are required.
    float rocketFireTimer = 9f, respawnTimer = 4f, stopSoundTimer = 2.5f, sendMissileTimer = 1.3f, winScreenTimer = 4f;
    // this will act as a true or flase statement.
    int sendMissile = 1;
    // this will hold an instance of the thrust bar for UI purposes.
    public ThrustBarController thrustBar;
    // these will hold the audio sound effects associated with the missile.
    public AudioSource launch, explode;
    private void Start()
    {
        // when this sctipt is started set spawnLocation to the transform of the spawn game object of the homeplanet
        spawnLocation = spawn.transform;
        // set initial max thrust
        thrustBar.SetThrust(maxVel);
        // set the initial camera as the first person cam
        fpCamActive = true;
    }
    // Update is called once per frame
    private void Update()
    {
        // if the number of building left on planet 3 is > 0 game has not been won so run missile code.
        if (enemyBase3.GetComponent<EnemyBases>().planet3Base > 0)
        {
            // call various missile functions
            GetUserInput();
            GetCoordinates();
            MovePlanets();
            Upgrades();
        }
        // if planet 3 has been destroyed run code.
        if (planet3Destroyed == true)
        {
            // start winScreenTimer so that the panel does not appear as soon as the final building has been destoryed
            winScreenTimer -= 1f * Time.deltaTime;
            // once the timer reaches 0.
            if (winScreenTimer <= 0f)
            {
                // activate game ui panel as this is where the win panel is stored this is to ensure that if the player is in fpCam or planetCam when final build is destoryed it will still display the planel.
                uiManager.SetActive(true);
                winPanel.SetActive(true);
            }
        }
    }
    void FixedUpdate()
    {
        // get the player input for the launch angle this is in the fixed update as this is not called every frame and will allow for more precise aiming.
        GetLaunchAngle();
    }
    // this function is used to increase the max vel that the player can use once planet 1 base is destoryed.
    void Upgrades()
    {
        // if stop explosion is true run code.
        if (stopExplosion)
        {
            // start timer to stop sound
            stopSoundTimer -= 1f * Time.deltaTime;
            if (stopSoundTimer <= 0)
            {
                // stop the explosion sound
                explode.Stop();
                // set stop explosion back to false.
                stopExplosion = false;
                // reset timer.
                stopSoundTimer = 2.5f;
            }
        }
        // if planet 1 has been destoryed
        if (planet1Destoryed)
        {
            // set planet 2 base as active so that it is now visible to the player.
            planet2Base.SetActive(true);
            // increase max vel.
            maxVel = 1800;
            // adjust this in the thrust bar controller script.
            thrustBar.GetComponent<ThrustBarController>().thrustSlider.maxValue = maxVel;
            // change the minusVel to balence this change in velocity.
            minusVel = -400f;
        }
        // same as above but no increase to max vel is added in this case.
        if (planet2Destroyed)
        {
            planet3Base.SetActive(true);
        }
    }
    // this is used to reset the missile if the player has used the self destruct button.
    void ResetMissile()
    {
        // if the missile is not launched.
        if (!launched)
        {
            // set the previous launch location to the current position of this missile.. this is to stop it getting an error if the player self destructs on prefore launching.
            previousLaunchPos = this.transform;
        }
        // provides a variable to hold the explosion visual effect.
        GameObject tempFlash;
        // instantiate the explosion prefab at the position/ rotation of the missile.
        tempFlash = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        // wait 4 seconds and destory the game object.
        Destroy(tempFlash, 4f);
        // freeze the rigidbody contraints so that the missile doesnt get pulled towards a planet prior to the player launching it.
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        // deactive the fire and smoke particle systems.
        rocketSmoke.SetActive(false);
        rocketFire.SetActive(false);
        // get the position to respawn the missle at.
        GetSpawnLocation();
        // set the velocity back to 0.
        velocity = 0;
        // update this on the UI.
        thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
        uiManager.GetComponent<UIManager>().thrustAmount = velocity;
        // reset the timers.
        rocketFireTimer = 9f;
        respawnTimer = 4f;
        // enable the mesh renderer so the missile can be seen again.
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        // reset remaining variables back to there origial values.
        launched = false;
        sendMissile = 1;
        sendMissileTimer = 1.3f;
        missleCollison = false;
    }
    // this function will handle the missile moving between planets once it has been opened to the player.
    void MovePlanets()
    {
        // if player presses tab key and the missile is not launched and the eCamActive is not active (Planet Cam) run code.
        if (Input.GetKeyDown(KeyCode.Tab) && !launched && !eCamActive)
        {
            // if planet 1 has been destoryed and missile is on home planet run code.
            if (planet1Destoryed && isHomePlanetSpawn)
            {
                // set the eBaseCam (Planet Cam) to planet2Cam.
                eBaseCam = planet2Cam;
                // change the gravity and mass values of the planet to match the home planets values else it would have a negative impact on gameplay.
                planet1.GetComponent<Attractor>().G = 300;
                planet1.GetComponent<Rigidbody>().mass = 5;
                // set the planet 1 orbit gameobject to false this is the areas in which the missile will lose velocity and the planets gravity will take over.
                planet1orbit.SetActive(false);
                // set the new spawn location for the missile to the game object position contained within planet1Spawn.
                spawnLocation = planet1Spawn.transform;
                // move the missile to the position and rotation of the defined spawn location.
                transform.position = spawnLocation.position;
                transform.rotation = spawnLocation.rotation;
                // set isPlanet1Spawn to true and homePlanetSpawn to false as missile will now be on planet 1.
                isPlanet1Spawn = true;
                isHomePlanetSpawn = false;
            }
            // if this is not the case check these conditions (works mostly the same as above)
            else if (planet2Destroyed && isPlanet1Spawn)
            {
                eBaseCam = planet3Cam;
                // set the mass and gravity back to origianl number now that the missile has moved to another planet.
                planet1.GetComponent<Attractor>().G = 2000;
                planet1.GetComponent<Rigidbody>().mass = 10;
                planet2.GetComponent<Attractor>().G = 300;
                planet2.GetComponent<Rigidbody>().mass = 5;
                planet1orbit.SetActive(true);
                planet2Orbit.SetActive(false);
                spawnLocation = planet2Spawn.transform;
                transform.position = spawnLocation.position;
                transform.rotation = spawnLocation.rotation;
                isPlanet1Spawn = false;
                isPlanet2Spawn = true;
            }
            // this provides a way for the player to move back to their home planet.
            // if missile is at planet 2 spawn and planet 2 is destoryed move to home planet.
            // or if missile is at planet 1 spawn and planet 2 has not been destoryed move back to home planet
            else if (isPlanet2Spawn && planet2Destroyed || isPlanet1Spawn && !planet2Destroyed)
            {
                eBaseCam = planet1Cam;
                // if planet 2 has been destroyed and misisle is currently on planet 2 then set mass and gravity values back to original
                if (planet2Destroyed)
                {
                    planet2.GetComponent<Attractor>().G = 2000;
                    planet2.GetComponent<Rigidbody>().mass = 10;
                    planet2Orbit.SetActive(true);
                }
                // the rest of this fucntion works the same as above.
                planet1.GetComponent<Attractor>().G = 2000;
                planet1.GetComponent<Rigidbody>().mass = 10;
                planet1orbit.SetActive(true);
                spawnLocation = spawn.transform;
                transform.position = spawnLocation.position;
                transform.rotation = spawnLocation.rotation;
                isPlanet1Spawn = false;
                isPlanet2Spawn = false;
                isHomePlanetSpawn = true;
            }
        }
    }
    // this function is used to get the launch angle from the players input.
    void GetLaunchAngle()
    {
        // if the missile is not launched and the active cam is fpCam run code.
        // this is so that the launch angle can only be changed if the player is using the fpCam as the other cameras use the arrow keys for movement input and without this the missile would move with those inputs also.
        if (launched == false && eCamActive == false && tpCamActive == false)
        {
            // rotate the missile angle along the positive x axis
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Rotate(0.3f * rotationSpeed, 0, 0);
            }
            // rotate the missile angle along the negative x axis
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(-0.3f * rotationSpeed, 0, 0);
            }
            // rotate the missile angle along the positive z axis
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -0.3f * rotationSpeed);
            }
            // rotate the missile angle along the negative z axis
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, 0.3f * rotationSpeed);
            }
        }
    }
    // this function is used to get the users input for anything that is not the launch angle.
    void GetUserInput()
    {
        // same condition as in the launch angle function
        if (launched == false && eCamActive == false && tpCamActive == false)
        {
            if (Input.GetKeyDown("w"))
            {
                // add 100 to velocity.
                velocity += 100;
                // update the UI components.
                uiManager.GetComponent<UIManager>().thrustAmount = velocity;
                thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
                
                if (velocity > maxVel)
                {
                    // if the player sets velocity above the maximum allowed amount set velocity back to maximum allowed
                    velocity = maxVel;
                    uiManager.GetComponent<UIManager>().thrustAmount = velocity;
                    thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
                }
            }
            if (Input.GetKeyDown("s"))
            {
                // this works the same as above but in reverse.
                velocity -= 100;
                uiManager.GetComponent<UIManager>().thrustAmount = velocity;
                thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
                if (velocity < minVel)
                {
                    velocity = minVel;
                    uiManager.GetComponent<UIManager>().thrustAmount = velocity;
                    thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
                }
            }
            if (Input.GetKeyDown("r"))
            {
                // this is to provide a quick and easy way to reset the missile launch angle to 0 on all axis.
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (Input.GetKeyDown("z"))
            {
                // if the camera is not currently zoomed in.
                if (!isZoomed)
                {
                    // set the current camera as the zoomed in camera.
                    isZoomed = true;
                    zoomCam.SetActive(true);
                    fpCam = zoomCam;
                }
                else
                {
                    // if zoom camera is currently active set camera back to fpCam.
                    isZoomed = false;
                    zoomCam.SetActive(false);
                    // a holder was created for the camera so that the fpCam game object could be swapped between fpCam and zoomedCam this was put in place to shorten the code which handles which cam is active.
                    fpCam = fpCamHolder;
                }
            }
        }

        if (Input.GetKeyDown("d"))
        {
            // if the d key is pressed reset the missile and play the explosion visual/ sound effect... also will stop launch sound if it is playing.
            launch.Stop();
            explode.Play();
            stopExplosion = true;
            ResetMissile();
        }
        // when e is pressed determine which camera should be active.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // if fpCam is the current cam run code.
            if (fpCamActive == true && tpCamActive == false && eCamActive == false)
            {
                // deactive point light... this has been included so that the player can see where the missile is in if they are using planetCam to get an idea of how to take the shot.
                pointLight.SetActive(false);
                // deactive HUD this is so that the visuals can be seen without the HUD being in the way.
                uiManager.SetActive(false);
                // set the third person cam as active cam and turn off the first person cam.
                fpCam.SetActive(false);
                tpCam.SetActive(true);
                // set bools accordingly
                tpCamActive = true;
                fpCamActive = false;
            }
            // this works the same as above but is used to active the eBaseCam (PlanetCam)
            else if (tpCamActive == true && fpCamActive == false && eCamActive == false)
            {
                // if missile is launched and this camera is used then turn off point light.
                if (launched)
                {
                    pointLight.SetActive(false);
                }
                else
                {
                    // else activate point light.
                    pointLight.SetActive(true);
                }
                fpCam.SetActive(false);
                tpCam.SetActive(false);
                eBaseCam.SetActive(true);
                tpCamActive = false;
                eCamActive = true;
                uiManager.SetActive(false);
            }
            // this works same as above but is used to set the first person cam as active cam.
            else if (eCamActive == true && fpCamActive == false && tpCamActive == false)
            {
                pointLight.SetActive(false);
                uiManager.SetActive(true);
                eBaseCam.SetActive(false);
                fpCam.SetActive(true);
                eCamActive = false;
                fpCamActive = true;
            }
        }
        // set conditions for the player to be able to launch the missile
        if (Input.GetKey(KeyCode.Space) && launched == false && eCamActive == false && tpCamActive == false && velocity > 0)
        {
            // play launch sound effect.
            launch.Play();
            // if camera is zoomed in set camrea back to fpCam.
            if (isZoomed)
            {
                isZoomed = false;
                zoomCam.SetActive(false);
                fpCam = fpCamHolder;
            }
            // set previous launch position as the current transform so that the selected launch angle will be retained for use when the missile is respawned.
            previousLaunchPos = this.transform;
            // get previous coordinates for x and z so that they can be displayed on the UI.
            xCoorPrev = xCoor;
            zCoorPrev = zCoor;
            // activate particle effects for rocket fire and smoke.
            rocketFire.SetActive(true);
            rocketSmoke.SetActive(true);
            // set launched to true.
            launched = true;
        }
        // if the missile has been launched run code.
        if (launched)
        {
            // begin timers.
            rocketFireTimer -= 1 * Time.deltaTime;
            sendMissileTimer -= 1f * Time.deltaTime;
            // once the timer has reached 0 stop the rocket fire visual.
            if (rocketFireTimer <= 0)
            {
                rocketFire.SetActive(false);
            }
            // this timer is used to delay the missile to give the effect that the missile is building velocity to launch.
            if (sendMissileTimer <= 0)
            {
                if (sendMissile > 0)
                {
                    sendMissile -= 1;
                    // unfreeze the contraints so that the missile can move and gravitational forces can act on it
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    // apply forces to missile in the direction provided by the player.
                    GetComponent<Rigidbody>().AddForce(transform.up * velocity);
                }
            }

        }
        // if missile has collided with something run code.
        if (missleCollison)
        {
            // move camera back so that it does not clip into the collided object.
            fpCam.transform.localPosition = new Vector3(0, 1.5f, 0.112f);
            // freeze all constraints again so the missile will not move as it has now collided with an object thus it should stop movement.
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // disable the mesh renderer to simulate it being destroyed.
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            // begin respawn timer so that the player can watch the missile explosion take place.
            respawnTimer -= 1f * Time.deltaTime;
            if (respawnTimer <= 2f)
            {
                // once timer hits 2 seconds disable rocket smoke.
                rocketSmoke.SetActive(false);
            }
            if (respawnTimer <= 1.5f)
            {
                // stop explosion sound effect so it doesnt keep playing once missile has respawned.
                explode.Stop();
            }
            if (respawnTimer <= 0)
            {
                // get the position for the missile to respawn at.
                GetSpawnLocation();
                // reset values and update UI.
                sendMissile = 1;
                velocity = 0;
                thrustBar.GetComponent<ThrustBarController>().thrustSlider.value = velocity;
                uiManager.GetComponent<UIManager>().thrustAmount = velocity;
                rocketFireTimer = 9f;
                respawnTimer = 4f;
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                launched = false;
                missleCollison = false;
                sendMissileTimer = 1.3f;
                fpCam.transform.localPosition = new Vector3(0, 2.8f, 0.112f);
            }
        }
    }
    // provides the spawn location for the missile.
    void GetSpawnLocation()
    {
        // get the position of the current value of spawnLocation.
        transform.position = spawnLocation.position;
        // get the rotation of the previous launch angle so that the player can reuse the angle with slight adjustments on a near miss.
        transform.rotation = previousLaunchPos.rotation;
    }
    // function is used to get numeric data to update the UI with.
    void GetCoordinates()
    {
        // get the transform ganles of z and z and save them to variables.
        xCoor = transform.eulerAngles.x;
        zCoor = transform.eulerAngles.z;
        // send this data to the UI manager so that it can be updated onto the UI.
        uiManager.GetComponent<UIManager>().xCoor = xCoor;
        uiManager.GetComponent<UIManager>().zCoor = zCoor;
    }
    // this will run when the ridgidbody detects a collision with another objec and store the object in other so that it can be accessed.
    private void OnTriggerEnter(Collider other)
    {
        // the the tag of the hit game object is orbit run code.
        if (other.tag == "orbit")
        {
            // get the missiles rigidbody component and take away some velocity to slow the missile down to simulate it being caught in the planets gravity.
            GetComponent<Rigidbody>().AddForce(transform.up * minusVel);
        }
        else
        {
            // if it collides with anything else such as a planet or moon run this.
            // stop launch and explode sound effects.
            launch.Stop();
            explode.Play();
            // set missile collision to true and deactivate rocketfire
            missleCollison = true;
            rocketFire.SetActive(false);
            // this works same as detailed earlier to provide an explosion visual when the missile is destoryed.
            GameObject tempFlash;
            tempFlash = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(tempFlash, 3f);
        }
    }
}
