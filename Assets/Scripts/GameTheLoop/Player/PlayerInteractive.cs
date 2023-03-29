using UnityEngine;
using UnityEngine.Events;
public class PlayerInteractive : MonoBehaviour
{
    public static PlayerInteractive instance { get; private set; }
    public Transform Transform => _transform;
    public PlayerComponentManager ComponentManager => _componentManager;
    public int Health => _health;

    public UnityAction<int> OnGetBerk;
    public UnityAction<int> OnFallingIntoDark;

    public UnityAction<Drop> OnGetBonus;
    public UnityAction OnDeath;

    [SerializeField] private GameObject _boom;
    [SerializeField] private PlayerComponentManager _componentManager;

    private Transform _transform;
    private Quaternion _quaternionIdentity;
    private int _health;
    private bool _isAlive;

    private void Awake()
    {
        instance = this;
        _isAlive = true;
        _health = 1;
        _transform = GetComponent<Transform>();
        _quaternionIdentity = Quaternion.identity;
    }

    private void GetDamage(int damage = 1)
    {
        if (BonusesManager.instance.TimeOfBonus[(int)Bonuses.Shield] == 0)
        {
            if (MenuStats.gameMode == (int)GameMods.Test) return;
            
            _health -= damage;
            if (_health == 0) Death();
        }
    }
    private void Death()
    {
        if (_isAlive)
        {
            _isAlive = false;
            OnDeath?.Invoke();
            Instantiate(_boom, _transform.position, _quaternionIdentity);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Asteroid asteroid) || collision.TryGetComponent(out EnemyLazer enemyLazer)) GetDamage();
        if (collision.TryGetComponent(out Enemy enemy) || collision.TryGetComponent(out InvertedLazer invertedLazer)) GetDamage(_health);
        if (collision.TryGetComponent(out EventHorizon horizon))
        {
            if ((!horizon.IsIvert && BonusesManager.instance.TimeOfBonus[(int)Bonuses.Shield] > 0) || horizon.IsTernsite)
            {
                _componentManager.InitiateHoleFlight();
                OnFallingIntoDark?.Invoke(horizon.IsTernsite?1:2);
                SceneDataSaver.SaveInt(nameof(_health), _health);
            }
            else
            {
                OnDeath?.Invoke();
                gameObject.SetActive(false);
            }
        }
        if(collision.TryGetComponent(out Snowflake snowflake))
        {
            OnGetBonus?.Invoke(snowflake);
            Destroy(collision.gameObject);
        }
        if(collision.TryGetComponent(out Crystal crystal))
        {
            OnGetBonus?.Invoke(crystal);
            Destroy(collision.gameObject);
        }
        if(collision.TryGetComponent(out Bonus bonus))
        {
            if (bonus.IsInverse) return;

            var index = bonus.ID;
            OnGetBerk?.Invoke(index);
            if (index < 3) _componentManager.InitiateManageBonus(index);
            if (index == 5) _componentManager.InitiateHyperdriveFlight();
            Destroy(collision.gameObject);
        }
    }
}