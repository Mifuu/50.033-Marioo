using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    public PlayerGC player;

    [System.Serializable]
    public struct PlayerGC
    {
        [Header("Health")]
        public int maxHealth;
        public float dmgITime;

        [Header("Movement")]
        public float speed;
        public float maxSpeed;
        public float smoothTime;

        [Header("Wall Stuck Fix")]
        public int wsRaycastCount;
        public float wsCheckDist;
        public LayerMask wsLayerMask;

        [Header("Jump")]
        public float upSpeed;
        public float upSpeedLiftKeyFactor;
        public float coyoteTime;
        public float groundCheckDist;
        public LayerMask groundMask;

        [Header("Enemy")]
        public LayerMask enemyMask;
    }
}
