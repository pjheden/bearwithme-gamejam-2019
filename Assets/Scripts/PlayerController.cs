using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [Range(0, 20)]
  public float speed = 10f;
  [Range(0, 20)]
  public float rotationSpeed = 10f;
  [Range(0, 20)]
  public float gravity = 10;
  [Range(0, 180)]
  public float pickupAngle = 90f;
  [Range(0, 2)]
  public float pickupDistance = 1f;
  public GameObject pickupObjectsParent;

  private GameObject[] pickupObjects;
  private Vector3 moveDirection = Vector3.zero;
  private CharacterController controller;
  private float horizontalMove;
  private float verticalMove;
  private GameObject closestForPickup;
  private GameObject objectWeAreHolding;

  void Start()
  {
    controller = GetComponent<CharacterController>();

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
    DrawDebugLines();

    if(Input.GetKeyDown(KeyCode.Space)){
      if(objectWeAreHolding == null)
        TryToPickupObject();
      else {
        DropObject();
      }
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
    Debug.Log("Picked Up: " + objectToPickUp.name);
    objectWeAreHolding.GetComponent<BoxCollider>().enabled = false;
    objectWeAreHolding = objectToPickUp;
  }

  void DropObject() 
  {
    Debug.Log("Droped Object: " + objectWeAreHolding.name);
    objectWeAreHolding.GetComponent<BoxCollider>().enabled = true;
    objectWeAreHolding = null;
  }

  void UpdatePlayerPosition()
  {
    // On key down get move direction, move player and set look direction
    if (Input.anyKey)
    {
      horizontalMove = Input.GetAxis("Horizontal");
      verticalMove = Input.GetAxis("Vertical");

      moveDirection = new Vector3(horizontalMove, 0.0f, verticalMove);
      moveDirection *= speed;

      controller.Move(moveDirection * Time.deltaTime);

      if (moveDirection != Vector3.zero)
      {
        // Creates a rotation with the target forward and up directions
        Quaternion rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
        // Lerp rotation, from current rotation to target rotation with set speed
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
      }
    }

    if(objectWeAreHolding != null) {
      objectWeAreHolding.transform.position = transform.position + transform.forward;        
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
}
