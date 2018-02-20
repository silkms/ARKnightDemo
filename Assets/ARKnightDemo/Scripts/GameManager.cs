using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Knight knightTemplate;
    public float damageInterval = 1f;
    public int damageAmount = 5;

	/// <summary>
    /// Initialize the GameManager
    /// </summary>
	void Start ()
    {
        // Store the damage interval.  Prevents memory allocation incurred
        // by creating a new WaitForSeconds every loop in the coroutine.
        m_WaitForSeconds = new WaitForSeconds(damageInterval);
        StartCoroutine(DamageRandomKnight());
	}
	
    /// <summary>
    /// Main game loop
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Monitor touch input.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // New touch began. 
                InstantiateKnightWithTouchInput(touch.position);
            }
        }
    }

    /// <summary>
    /// Instantiates the knight with touch input.
    /// </summary>
    /// <returns><c>true</c>, if knight with touch input was instantiated, <c>false</c> otherwise.</returns>
    /// <param name="touchPosition">Touch position.</param>
    public bool InstantiateKnightWithTouchInput(Vector2 touchPosition)
    {
        if (knightTemplate == false)
            return false;
        
        // Get the interscetion of the touch and the AR Plane
        Vector3 posOnPlane;
        Quaternion planeRotation;
        if (Platform.AR.GetTouchPlaneIntersectionTransform(touchPosition, out posOnPlane, out planeRotation))
        {
            // Instantiate a knight at the plane intersection position.
            Instantiate(knightTemplate, posOnPlane, GetKnightRotation(posOnPlane, planeRotation));
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Find rotation so that the instantiated Knight will face the camera as best as possible.
    /// </summary>
    /// <returns>The knight rotation.</returns>
    /// <param name="knightPos">Knight position.</param>
    /// <param name="planeRotation">Plane rotation.</param>
    Quaternion GetKnightRotation(Vector3 knightPos, Quaternion planeRotation)
    {
        Vector3 up = planeRotation * Vector3.up;
        Vector3 camPos = Camera.main ? Camera.main.transform.position : Vector3.zero;
        Vector3 toCameraOnIntersectPlane = Vector3.ProjectOnPlane(camPos - knightPos, up);
        return Quaternion.LookRotation(toCameraOnIntersectPlane, up);
    }


    /// <summary>
    /// Coroutine to damage a random knight at a regular interval.
    /// </summary>
    IEnumerator DamageRandomKnight()
    {
        while (true)
        {
            yield return m_WaitForSeconds;
            Knight knight = Knight.GetRandomKnight();
            if (knight)
                knight.ApplyDamage(damageAmount);
        }
    }

    WaitForSeconds m_WaitForSeconds;
}
