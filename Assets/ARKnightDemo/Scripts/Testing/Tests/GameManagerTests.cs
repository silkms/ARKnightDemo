#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class GameManagerTests
{
    [Test]
    public void InstantiateKnight()
    {
        // Get the GameManager template from test resources.
        GameManager gameManagerTemplate = TestResources.GetAssetByType<GameManager>();
        if (gameManagerTemplate == false)
        {
            Assert.Fail();
            return;
        }

        GameManager gameManager = Object.Instantiate(gameManagerTemplate);
        Knight knight = null;
        if (gameManager.InstantiateKnightWithTouchInput(new Vector2()))
            knight = Object.FindObjectOfType<Knight>();

        Assert.NotNull(knight);
    }

    [Test]
    public void KnightGetRotation()
    {
        // TODO: Setup test for validating GameManager.GetKnightRotation
    }

    IEnumerator ApplyKnightDamageIntervalTrial(float damageInterval)
    {
        // Get the GameManager template from test resources.
        GameManager gameManagerTemplate = TestResources.GetAssetByType<GameManager>();
        if (gameManagerTemplate == false)
        {
            Assert.Fail();
            yield break;
        }

        GameManager gameManager = Object.Instantiate(gameManagerTemplate);
        gameManager.damageInterval = damageInterval;

        Knight knight = new GameObject().AddComponent<Knight>();
        knight.Construct(100);
        int startingHitPoints = knight.currentHitPoints;

        if (gameManager.damageInterval > 0f)
        {
            // Wait for half way through the damage interval and test hit point equality
            yield return new WaitForSeconds(gameManager.damageInterval * 0.5f);
            Assert.AreEqual(startingHitPoints, knight.currentHitPoints);
            // Wait for damage interval and test hit point equality
            // Damage should have been applied
            yield return new WaitForSeconds(gameManager.damageInterval);
            Assert.AreEqual(startingHitPoints - gameManager.damageAmount, knight.currentHitPoints);
        }
        else
        {
            Assert.AreEqual(startingHitPoints, knight.currentHitPoints);
            yield return null;
            yield return null;
            Assert.AreEqual(startingHitPoints - gameManager.damageAmount, knight.currentHitPoints);
        }
    }

    [UnityTest]
    public IEnumerator ApplyKnightDamageInterval()
    {
        yield return ApplyKnightDamageIntervalTrial(0f);
        Cleanup();
        //CleanupTests();
        yield return ApplyKnightDamageIntervalTrial(1f);
        //CleanupTests();
        //yield return ApplyKnightDamageIntervalTrial(3f);
    }

    /// <summary>
    /// Runs after every test and cleans up any game objects left over
    /// </summary>
    [TearDown]
    public void Cleanup()
    {
        CleanupTests.Cleanup();
    }
}
#endif
