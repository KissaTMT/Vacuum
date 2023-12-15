using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    public Transform Transform => _transform;
    public PlayerComponentManager ComponentManager => _componentManager;

    public UnityAction<int> GetBerk;
    public UnityAction<int> FallingIntoDark;

    public UnityAction<Drop> GetBonus;
    public UnityAction DeathPlayer;

    [SerializeField] private GameObject _boom;

    private Transform _transform;
    private PlayerComponentManager _componentManager;
    private Quaternion _quaternionIdentity;

    public void Initialize()
    {
        instance = this;
        _transform = GetComponent<Transform>();
        _componentManager = GetComponent<PlayerComponentManager>();
        _quaternionIdentity = Quaternion.identity;
    }
    public void Death()
    {
        DeathPlayer?.Invoke();
        Instantiate(_boom, _transform.position, _quaternionIdentity);
        gameObject.SetActive(false);
    }
    private void GetDamage()
    {
        if (MenuStats.gameMode == (int)GameMods.Test) return;
        if (BonusesManager.instance.TimeOfBonus[(int)Bonuses.Shield] == 0) Death();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out EnemyLazer enemyLazer)) GetDamage();
        if (collision.TryGetComponent(out Enemy enemy) || collision.TryGetComponent(out InvertedLazer invertedLazer)) GetDamage();
        if (collision.TryGetComponent(out EventHorizon horizon))
        {
            if ((!horizon.IsIvert && BonusesManager.instance.TimeOfBonus[(int)Bonuses.Shield] > 0) || horizon.IsTernsite)
            {
                _componentManager.InitiateHoleFlight();
                FallingIntoDark?.Invoke(horizon.IsTernsite?1:2);
            }
            else
            {
                DeathPlayer?.Invoke();
                gameObject.SetActive(false);
            }
        }
        if(collision.TryGetComponent(out Drop drop))
        {
            if(drop is Crystal ||  drop is Snowflake)
            {
                GetBonus?.Invoke(drop);
                Destroy(collision.gameObject);
            }
            else if(drop is Bonus bonus)
            {
                if (bonus.IsInverse) return;

                var index = bonus.ID;
                GetBerk?.Invoke(index);
                if (index < 3) _componentManager.InitiateManageBonus(index);
                if (index == 5) _componentManager.InitiateHyperdriveFlight();
                Destroy(collision.gameObject);
            }
            
        }
    }
}