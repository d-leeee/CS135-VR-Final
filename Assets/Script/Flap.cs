using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Flap : MonoBehaviour
{
    public GameObject Bird;
    public float motionCaptureMultiplier = 0.3f; // Multiplier for controller motion capture
    public float movementSensitivity = 1f; // How much to amplify controller movement
    public float maxUpwardVelocity = 7f; // Maximum upward velocity
    public float gravity = 16f; // Gravity force
    
    private Vector3 birdVelocity = Vector3.zero;
    private Vector3 lastLeftControllerPos = Vector3.zero;
    private Vector3 lastRightControllerPos = Vector3.zero;
    private XRNode leftControllerNode = XRNode.LeftHand;
    private XRNode rightControllerNode = XRNode.RightHand;
    private float timeSinceLastMovement = 0f;
    private float movementStopDelay = 0.0f; // Time to coast before gravity kicks in

    public AudioSource flapSource;
    public AudioClip flapClip;
    private bool playedThisFlap = false;

    void Start()
    {
        // Initialize controller positions
        InputDevices.GetDeviceAtXRNode(leftControllerNode).TryGetFeatureValue(CommonUsages.devicePosition, out lastLeftControllerPos);
        InputDevices.GetDeviceAtXRNode(rightControllerNode).TryGetFeatureValue(CommonUsages.devicePosition, out lastRightControllerPos);
    }

    void Update()
    {
        // Get current controller positions
        Vector3 currentLeftPos = Vector3.zero;
        Vector3 currentRightPos = Vector3.zero;
        
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(leftControllerNode);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        
        leftController.TryGetFeatureValue(CommonUsages.devicePosition, out currentLeftPos);
        rightController.TryGetFeatureValue(CommonUsages.devicePosition, out currentRightPos);

        // Calculate vertical movement velocity from both controllers
        float leftVerticalVelocity = 0f;
        float rightVerticalVelocity = 0f;
        
        if (Time.deltaTime > 0)
        {
            leftVerticalVelocity = Mathf.Abs((currentLeftPos.y - lastLeftControllerPos.y) / Time.deltaTime * motionCaptureMultiplier);
            rightVerticalVelocity = Mathf.Abs((currentRightPos.y - lastRightControllerPos.y) / Time.deltaTime * motionCaptureMultiplier);
        }

        float avgVerticalVelocity = (leftVerticalVelocity + rightVerticalVelocity) / 2f;
        
        if (avgVerticalVelocity > 0)
        {
            birdVelocity.y += avgVerticalVelocity * movementSensitivity;
            timeSinceLastMovement = 0f;
            
            // Play flap sound once per flap motion
            if (!playedThisFlap && flapSource != null && flapClip != null)
            {
                flapSource.PlayOneShot(flapClip);
                playedThisFlap = true;
            }
        }
        else
        {
            timeSinceLastMovement += Time.deltaTime;
            playedThisFlap = false; // Reset flag when not moving
        }
        
        if (birdVelocity.y > maxUpwardVelocity)
        {
            birdVelocity.y = maxUpwardVelocity;
        }
        
        birdVelocity.y -= gravity * Time.deltaTime;
        Bird.transform.position += birdVelocity * Time.deltaTime;
        
        if (Bird.transform.position.y < -1f)
        {
            Vector3 pos = Bird.transform.position;
            pos.y = -1f;
            Bird.transform.position = pos;
            birdVelocity.y = 0f;
        }
        
        // Update last positions for next frame
        lastLeftControllerPos = currentLeftPos;
        lastRightControllerPos = currentRightPos;
    }
}
