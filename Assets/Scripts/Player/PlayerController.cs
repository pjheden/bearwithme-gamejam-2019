using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [Header("Movement settings")]
  [Range(0, 20)]
  public float defaultSpeed = 10f;
  [Range(0, 20)]
  public float sprintSpeed = 15f;
  [Range(0, 20)]
  public float rotationSpeed = 10f;
  [Range(0, 20)]
  public float gravity = 10;
  [Range(0, 180)]
  [Header("Pickup settings")]
  public float pickupAngle = 90f;
  [Range(0, 5)]
  public float pickupDistance = 1f;
  public GameObject pickupObjectsParent;
  public CollectorHandler collectorHandler;
  [Header("Sounds")]
  public AudioClip[] pickupSounds;
  public AudioClip walkSound;

    [Header("Animator")]
    public Animator anim;

  private float speed;
  private GameObject[] pickupObjects;
  private Vector3 moveDirection = Vector3.zero;
  private CharacterController controller;
  private float horizontalMove;
  private float verticalMove;
  private GameObject objectWeAreHolding;
  private bool isSitting;

  void Start()
  {
    controller = GetComponent<CharacterController>();

    speed = defaultSpeed;
    pickupObjects = new GameObject[pickupObjectsParent.transform.childCount];
    int count = 0;
    foreach (Transform child in pickupObjectsParent.transform)
    {
      pickupObjects[count] = child.gameObject;
      count++;
    }
    pickupAngle *= Mathf.PI / 180; // Convert to radians
  }

  void Update()
  {
    UpdatePlayerPosition();
    CheckPlayerPickupInput();
    CheckSpeedModifierKey();

    DrawDebugLines();
  }

  void CheckPlayerPickupInput() 
  {
    if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {
      if(objectWeAreHolding == null)
        TryToPickupObject();
      else 
        DropObject();
    }
  }

  void TryToPickupObject() 
  {
    // Go through all objects, pickup the closes one that is in valid region
    // If we are already holding an object drop it
    int closestObjectIndex = 0;
    bool canWePickupAnything = false;
    for(int i = 0; i < pickupObjects.Length; i++) 
    {
      GameObject objectToCheck = pickupObjects[i];  
      if(IsObjectInValidAngle(objectToCheck) && IsObjectInValidDistance(objectToCheck))
      {
        canWePickupAnything = true;
        if(DistanceToObject(objectToCheck) <= DistanceToObject(pickupObjects[closestObjectIndex])){
          closestObjectIndex = i;
        }
      }
    }

    if(canWePickupAnything) {
      PickupObject(pickupObjects[closestObjectIndex]);
    }
  }

  void PickupObject(GameObject objectToPickUp)
  {
    objectWeAreHolding = objectToPickUp;
    collectorHandler.HighLightCorrectCollector(objectToPickUp.GetComponent<ObjectToCollect>().dropOffLocation);
    objectWeAreHolding.GetComponent<BoxCollider>().enabled = false;
    // Get rand clip
    int randIndex = Mathf.RoundToInt( Random.Range(0, pickupSounds.Length) );
    AudioClip clip = pickupSounds[randIndex];
    // Play clip
    AudioSource audioSource = GetComponent<AudioSource>();
    audioSource.clip = clip;
    audioSource.Play();
    anim.SetBool("carry", true);

    }

    void DropObject() 
  {
    objectWeAreHolding.GetComponent<BoxCollider>().enabled = true;
    collectorHandler.DisableHighlightOnCollector();
    objectWeAreHolding = null;
    anim.SetBool("carry", false);

    }

    void CheckSpeedModifierKey() 
  {
    // If left shift is pressed, set move speed to sprint speed
    if(Input.GetKey(KeyCode.LeftShift))
      speed = sprintSpeed;
    else
      speed = defaultSpeed;
  }

  void UpdatePlayerPosition()
  {
    // On key down get move direction, move player and set look direction
    if (Input.anyKey)
    {
      horizontalMove = -Input.GetAxis("Horizontal");
      verticalMove = -Input.GetAxis("Vertical");

      moveDirection = new Vector3(horizontalMove, 0.0f, verticalMove).normalized;
      moveDirection *= speed;

      controller.Move(moveDirection * Time.deltaTime);

      if (moveDirection != Vector3.zero)
      {
        PlayWalkingSound();
        DoWalkingAnimation(true);
        // Creates a rotation with the target forward and up directions
        Quaternion rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
        // Lerp rotation, from current rotation to target rotation with set speed
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            DoWalkingAnimation(false);
        }

        // Force y position
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
        }
        else
        {
            DoWalkingAnimation(false);
        }

        // Update position of object we are holding
        if (objectWeAreHolding != null) {
      objectWeAreHolding.transform.position = Vector3.Lerp(objectWeAreHolding.transform.position,transform.position + transform.forward, Time.deltaTime * 20); // Lerp object position on pickup        
    }
  }

private void PlayWalkingSound()
{
    // Play clip
    AudioSource audioSource = GetComponent<AudioSource>();
    if (!audioSource.isPlaying)
    {
        audioSource.clip = walkSound;
        audioSource.Play();
    }

}

    private void DoWalkingAnimation(bool run)
    {
        if (run)
        {    
            anim.SetBool("running", true);
            isSitting = false;
        }
        else
        {
            anim.SetBool("running", false);
            isSitting = true;
        }
    }

  bool IsObjectInValidAngle(GameObject pickupable) 
  {
      Vector3 toObject = (pickupable.transform.position - transform.position);
      float pickupAngleThreshold = Mathf.Sin(pickupAngle / 2);
      float angleToObject = Vector3.Angle(transform.forward, toObject.normalized) * Mathf.PI / 180;
      return angleToObject <= pickupAngle / 2;
  }

  bool IsObjectInValidDistance(GameObject pickupable) {
    Vector3 toObject = (pickupable.transform.position - transform.position);
    float distanceToObject = toObject.magnitude;
    return distanceToObject <= pickupDistance;
  }

  float DistanceToObject(GameObject pickupable) {
    Vector3 toObject = (pickupable.transform.position - transform.position);
    return toObject.magnitude;
  }

  void DrawDebugLines()
  {
    Vector3 leftBound = new Vector3(Mathf.Sin(pickupAngle / 2), 0, Mathf.Cos(pickupAngle / 2)).normalized;
    Vector3 rightBound = new Vector3(Mathf.Sin(-pickupAngle / 2), 0, Mathf.Cos(-pickupAngle / 2)).normalized;

    leftBound = transform.TransformDirection(leftBound);
    rightBound = transform.TransformDirection(rightBound);
    
    Debug.DrawRay(transform.position, leftBound * pickupDistance, Color.cyan);
    Debug.DrawRay(transform.position, rightBound * pickupDistance, Color.cyan);

    // Lines from player to objects
    foreach (GameObject pickupable in pickupObjects)
    {
      if (IsObjectInValidAngle(pickupable) && IsObjectInValidDistance(pickupable))
        Debug.DrawLine(transform.position, pickupable.transform.position, Color.green);
      else
        Debug.DrawLine(transform.position, pickupable.transform.position, Color.red);
    }
  }

  public bool IsSitting()
  {
        return isSitting;
  }
}
