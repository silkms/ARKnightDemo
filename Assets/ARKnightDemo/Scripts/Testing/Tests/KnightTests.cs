#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class KnightTests
{
    [Test]
    public void KnightInitializesWithMaxHitPoints()
    {
        Knight knight = new GameObject().AddComponent<Knight>();
        Assert.AreEqual(knight.maxHitPoints, knight.currentHitPoints);
    }

    [TestCase(42)]
    public void CanReInitializesKnightWithMaxHitPoints(int startingHitpoints)
    {
        Knight knight = new GameObject().AddComponent<Knight>();
        knight.Construct(startingHitpoints);
        Assert.AreEqual(knight.maxHitPoints, knight.currentHitPoints);
    }

    [TestCase(100, 10, 90)]
    [TestCase(50, 10, 40)]
    [TestCase(100, 110, 0)] // Tests preventing the knight from dropping below zero hp
    public void KnightCanTakeDamage(int startingHitpoints, int damage, int expectedResult)
    {
        Knight knight = new GameObject().AddComponent<Knight>();
        knight.Construct(startingHitpoints);
        knight.ApplyDamage(damage);

        Assert.AreEqual(knight.currentHitPoints, expectedResult);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void GetRandomKnight(int numberOfKnightsToSpawn)
    {
        // Instantiate our knights
        for (int i = 0; i < numberOfKnightsToSpawn; i++)
            new GameObject().AddComponent<Knight>();

        // Get our random knight
        Knight selectedKnight = Knight.GetRandomKnight();

        if (numberOfKnightsToSpawn > 0)
            Assert.NotNull(selectedKnight);
        else
            Assert.Null(selectedKnight);
    }

    [UnityTest]
    public IEnumerator DestroyAfterDead()
    {
        Knight knight = new GameObject().AddComponent<Knight>();
        knight.Construct(100);
        knight.ApplyDamage(110);

        yield return new WaitForSeconds(6f);

        // If the knight has been destroyed in this window, the Knight.Count will be 0.
        Assert.AreEqual(Knight.Count, 0);
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