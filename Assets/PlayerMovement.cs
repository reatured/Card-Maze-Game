using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public PlayerInput input;
    public Transform startingPos;
    private bool canWalk = false;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPos.position; 


    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            onWalking();
        }
    }

    public void OnMove()
    {
        print("moved");
    }

    public void playerMove(InputAction.CallbackContext context)
    {
        if (context.started && canWalk == false)
        {
            Vector2 inputDirection = context.ReadValue<Vector2>();
            moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
            moveDirection = -moveDirection.normalized;
            print(moveDirection);

            startWalk();
        }
        //print(context.ReadValue<Vector2>());
    }

    private Vector3 targetPos, startPos;
    private float startTime;
    public float stepTime = 1f;

    public void startWalk()
    {
        startTime = Time.time;
        startPos = transform.position; 
        Ray ray = new Ray(transform.position, moveDirection);
        RaycastHit hit; 
        if (Physics.Raycast(ray, out hit))
        {
            targetPos = hit.point; 
        }

        int x = (int)(targetPos.x - transform.position.x);
        int z = (int)(targetPos.z - transform.position.z);
        targetPos = transform.position + new Vector3(x, 0, z);

        canWalk = true;
    }

    public AnimationCurve moveLerp;
    
    public void onWalking()
    {
        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        float journeyTime = (Time.time - startTime)/stepTime;

        transform.position = Vector3.Lerp(startPos, targetPos, moveLerp.Evaluate(journeyTime));

        if(journeyTime > 1)
        {
            endWalk(); 
        }
    }

    public void endWalk()
    {
        canWalk = false;
    }
}