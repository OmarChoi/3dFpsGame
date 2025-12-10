using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyMask;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }
    }
    
    private void Shot()
    {
        Ray ray = new Ray(_firePosition.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {
            Debug.Log(hitInfo.transform.name);
        }
    }
}
