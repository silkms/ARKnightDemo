using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class Knight : MonoBehaviour
{
    const string k_TakeDamageAnimName = "TakeDamage";
    const string k_DieAnimName = "Die";

    #region Static
    static List<Knight> m_Instances = new List<Knight>();

    /// <summary>
    /// Gets the number of active Knights
    /// </summary>
    /// <value>The count.</value>
    public static int Count { get { return m_Instances.Count; } }

    /// <summary>
    /// Gets a random knight from the pool of all instantiated knights.
    /// </summary>
    /// <returns>The random knight.</returns>
    public static Knight GetRandomKnight()
    {
        if (m_Instances.Count > 0)
        {
            // Note: Random.Range Max is exclusive so pass the max index plus 1.
            int index = Random.Range(0, m_Instances.Count);
            return m_Instances[index];
        }

        return null;
    }
    #endregion

    public int maxHitPoints = 100;
    public int currentHitPoints { get; private set; }

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Awake()
    {
        Construct(maxHitPoints);
	}

    public void Construct(int hitPoints)
    {
        maxHitPoints = hitPoints;
        currentHitPoints = maxHitPoints;
        m_Text = GetComponentInChildren<Text>();
        m_Anim = GetComponent<SimpleAnimation>();
    }

    /// <summary>
    /// New Knight created/enabled.
    /// Add this knight to our list of Knight instances.
    /// </summary>
    private void OnEnable()
    {
        m_Instances.Add(this);
    }

    /// <summary>
    /// Knight was disabled.
    /// Remove this Knight from our list of instances.
    /// This will also be called when the Knight gameObject is destroyed.
    /// </summary>
    private void OnDisable()
    {
        m_Instances.Remove(this);
    }

    /// <summary>
    /// Applies damage to the knight.
    /// </summary>
    /// <param name="amount">Amount.</param>
    public void ApplyDamage(int amount)
    {
        currentHitPoints -= Mathf.Abs(amount);
        if (currentHitPoints > 0)
        {
            // Damage the knight
            // Play damage animation
            if (m_Anim && m_Anim.GetState(k_TakeDamageAnimName) != null)
                m_Anim.CrossFade(k_TakeDamageAnimName, 0.1f);

            // Update UI
            if (m_Text && maxHitPoints > 0)
            {
                m_StartColor = m_Text.color;
                m_Text.color = Color.red;
                m_Text.text = Mathf.RoundToInt(100 * (float)currentHitPoints / (float)maxHitPoints) + "%";
            }

            // Start coroutine to crossfade idle animation and reset color.
            StartCoroutine(ResumeIdle());
        }
        else
        {
            // Knight dies...
            currentHitPoints = 0;
            // Play dead animation
            if (m_Anim && m_Anim.GetState(k_DieAnimName) != null)
                m_Anim.CrossFade(k_DieAnimName, 0.1f);

            // Update UI
            if (m_Text)
            {
                m_StartColor = m_Text.color;
                m_Text.color = Color.red;
                m_Text.text = "Dead...";
            }


            // Remove this knight instance from out list of available knights
            m_Instances.Remove(this);

            StartCoroutine(DestroyAfterTime());
        }
    }

    IEnumerator ResumeIdle()
    {
        yield return new WaitForSeconds(0.1f);
        if (m_Anim && m_Anim.GetState(k_TakeDamageAnimName) != null)
            m_Anim.CrossFade("Default", 0.3f);
        
        if (m_Text)
            m_Text.color = m_StartColor;
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        //TODO: Create object pool for recycling Knights
        Destroy(gameObject);
    }

    Color m_StartColor;
    private Text m_Text;
    SimpleAnimation m_Anim;
}
