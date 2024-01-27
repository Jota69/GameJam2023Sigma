using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    private Mapa inputs;
    [SerializeField] private PlayerController player;
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 6;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;
    [SerializeField] private float timeToGrapplin;
    [SerializeField] Transform lookPointObject;


    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = false;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;
    [SerializeField] private float coolDownTime;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    private bool objetc = false;
    private GameObject grappeledObject;
    private bool inCoolDown;
    IEnumerator solt;

    [Header("SoundFX")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip ganchoAudio;


    private void Awake()
    {
        inputs = new Mapa();
        inputs.Enable();
    }
    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();


    }


    private void OnEnable()
    {
        inputs.Player.gancho.performed += OnEngagePerformed;
        inputs.Player.gancho.canceled += OnEngageCanceled;
        inputs.Player.gancho.started += OnEngageStarted;
    }
    private void OnDisable()
    {
        inputs.Player.gancho.performed -= OnEngagePerformed;
        inputs.Player.gancho.canceled -= OnEngageCanceled;
        inputs.Player.gancho.started -= OnEngageStarted;
        audioSource.Stop();
    }
    



    private void Update()
    {
        
        if (player.isActive)
        {
            if (!grappleRope.enabled)
            {
                if (Gamepad.current!=null) 
                {
                    Cursor.visible = false;
                    lookPointObject.gameObject.SetActive(true);
                    Vector2 gamepadInput = inputs.Player.Apuntado.ReadValue<Vector2>();
                    Vector3 newPosition = lookPointObject.position + (Vector3)(gamepadInput * Time.deltaTime * rotationSpeed);

                    // restringe newPosition a estar dentro de la circunferencia
                    Vector2 direction = (Vector2)newPosition - (Vector2)gunPivot.position;
                    if (direction.magnitude > maxDistnace)
                    {
                        Vector2 clampedPosition = (Vector2)gunPivot.position + direction.normalized * maxDistnace;
                        newPosition = new Vector3(clampedPosition.x, clampedPosition.y, lookPointObject.position.z);
                    }

                    lookPointObject.position = newPosition;
                    RotateGun(lookPointObject.position, true);
                }
                else
                {
                    Cursor.visible = true;
                    lookPointObject.gameObject.SetActive(false);
                    RotateGun(m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()),true);
                }
                
            }
            else
            {
                RotateGun(grapplePoint, false);
            }
            if (grappeledObject != null&&objetc&& launchToPoint && grappleRope.isGrappling)
            {
                // Mover el objeto hacia el personaje
                grappeledObject.transform.position = Vector2.Lerp(grappeledObject.transform.position, gunHolder.position, Time.deltaTime * launchSpeed);
                grapplePoint = grappeledObject.transform.position;
                //grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            }
        }
    }
    
    private void OnEngagePerformed(InputAction.CallbackContext value)
    {
        if (player.isActive)
        {
            if (launchToPoint && grappleRope.isGrappling&&!objetc)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
    }

    private void OnEngageCanceled(InputAction.CallbackContext value)
    {
        if (player.isActive)
        {
            //grappleRope.enabled = false;
            objetc = false;
            
            m_springJoint2D.enabled = false;
            if (grappleRope.enabled&&grappleRope.strightLine)
            {
                inCoolDown = true;
                grappleRope.EnableReverse();
                StartCoroutine(CuerdaCoolDown());
            }
            else
            {
                grappleRope.enabled = false;
            }
            //StartCoroutine(cuerdaReturn());
            m_rigidbody.gravityScale = 1;
            audioSource.Stop();
            StopCoroutine(solt);
        }
    }
    IEnumerator CuerdaCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        inCoolDown = false;
    }

    private void OnEngageStarted(InputAction.CallbackContext value)
    {
        if (player.isActive)
        {
            if (!inCoolDown)
            {
                if (grappleRope.isReturning)
                {
                    grappleRope.enabled = false;
                }
                SetGrapplePoint();
                if (ganchoAudio != null && grappleRope.enabled)
                {
                    audioSource.PlayOneShot(ganchoAudio);
                }
                solt = Soltar();
                StartCoroutine(solt);
            }

        }
    }


    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private IEnumerator Soltar()
    {
        yield return new WaitForSeconds(ganchoAudio.length);
        //grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        grappleRope.EnableReverse();
        inCoolDown = true;
        objetc = false;
        m_rigidbody.gravityScale = 1;
        audioSource.Stop();
        StartCoroutine(CuerdaCoolDown());
    }
    void SetGrapplePoint()
    {
        Vector2 distanceVector;
        if (Gamepad.current != null)
        {
            Cursor.visible = false;
            Vector2 gamepadInput = inputs.Player.Apuntado.ReadValue<Vector2>();
            Vector3 newPosition = lookPointObject.position + (Vector3)(gamepadInput * Time.deltaTime * rotationSpeed);
            distanceVector = (Vector2)newPosition - (Vector2)gunPivot.position;
        }
        else 
        { 
            distanceVector = m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gunPivot.position; 
        }
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);

        if (hit)
        {
           //Visualizar el raycast en la escena
            //Debug.DrawRay(firePoint.position, distanceVector.normalized * hit.distance, Color.green, 0.2f);

            if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;

                    // Mensaje de depuración para imprimir la información del Raycast
                }
            }else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Objetos"))
            {
                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                    //objetc = true;
                    grappeledObject = hit.transform.gameObject;
                    objetc = true;
                    // Mensaje de depuración para imprimir la información del Raycast
                }
                
            }
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (firePoint != null && hasMaxDistance)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
    //    }
    //}

}
