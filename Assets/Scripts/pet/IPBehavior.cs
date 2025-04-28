using System.Collections.Generic;
using UnityEngine;

public interface IPetBehavior
{
    void PerformAction();
    void AssignReferences(Transform player, List<Transform> enemies, List<Transform> referencePoints, LayerMask enemyLayer, GameObject projectilePrefab, float projectileScale);
}

