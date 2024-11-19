using NUnit.Framework;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class GameManagerEditorTest
{
    [Test]
    public void GameManager_Singleton_Instance_Should_Be_Unique()
    {
        // Els� Gamemanager l�trehoz�sa
        var firstInstance = new GameObject().AddComponent<GameManager>(); ;
        firstInstance.Awake();

        Assert.IsNotNull(GameManager.Instance);
        Assert.AreEqual(firstInstance, GameManager.Instance);

        // M�sodik mock gamemanager l�trehoz�sa
        var secondInstance = new GameObject().AddComponent<GameManager>();

        secondInstance.Awake();

        // A m�sodik p�ld�nynak �resnek k�ne lenni.
        Assert.IsTrue(secondInstance == null);
    }

    [Test]
    public void OnPlayerKilled_Should_Decrease_Player_Health()
    {
        // Mock player l�trehoz�sa
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        // Spriterendere hozz�ad�sa k�l�nben hib�ra fut
        playerObject.AddComponent<SpriteRenderer>();
        player.acquireSpriteRenderer();

        player.health = 3;

        var gameManager = new GameObject().AddComponent<GameManager>();

        gameManager.OnPlayerKilled();

        Assert.AreEqual(2, player.health);
    }

    [Test]
    public void WeaponUpgrade_Should_Not_Go_Over_Limit()
    {
        var gameManager = new GameObject().AddComponent<GameManager>();
        
        // Mock player l�trehoz�sa
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        
        // Mock weapon template-k l�trehoz�sa
        for (int i = 0; i < 3; i++)
        {
            player.upgradeTemplates.Add(new GameObject().AddComponent<Player>());
        }
        player.currentTemplate = -1;

        gameManager.GetType()
            .GetField("player", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameManager, player);


        gameManager.UpgradeWeapons();
        Assert.AreEqual(0, player.currentTemplate);
        
        gameManager.UpgradeWeapons();
        gameManager.UpgradeWeapons();

        Assert.AreEqual(2, player.currentTemplate);

        // T�lt�lt�s tesztel�se
        gameManager.UpgradeWeapons();
        Assert.AreEqual(2 ,player.currentTemplate);

    }

}