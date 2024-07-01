using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class TokenRenderer : MonoBehaviour
{



    public float scale = 1f;
    public Vector3 minScale = new Vector3(1, 1, 1);
    public Vector3 maxScale = new Vector3(2, 2, 2);
    // The speed at which the object pulses.
    public float pulseSpeed = 1.0f;
    private Token token;
    private float pulseTimer = 0.0f;
    Vector3 movementDelta = Vector3.zero;
    Vector3 rotationDelta = Vector3.zero;
    float rotationAmount = 5.0f;
    float rotationSpeed = 5.0f;
    public void SetToken(Token token)
    {
        this.token = token;
    }
    void Update()
    {
        if (token != null) {



            // Update the pulse timer.
            //pulseTimer += Time.deltaTime * pulseSpeed;

            // Calculate the lerp value.
            //float lerpValue = (Mathf.Sin(pulseTimer) + 1.0f) / 2.0f;

            // Interpolate between the minimum and maximum scale values.
            transform.position = Vector3.Lerp(transform.position, token.transform.position, Time.deltaTime * 15f);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale,scale,scale), Time.deltaTime * 20f);

            //Vector3 movement = (transform.position - token.transform.position);
            //movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
            //Vector3 movement = (transform.position - token.transform.position);
            //movementDelta = Vector3.Lerp(movementDelta, movement, 5 * Time.deltaTime);
            //Vector3 movementRotation =  movementDelta * rotationAmount;
            //rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));

            //transform.LookAt(movement, transform.forward);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
