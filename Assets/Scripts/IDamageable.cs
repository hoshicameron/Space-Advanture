using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public interface IDamageable
{
    void GotHit(int damage,Vector3 position);
}
