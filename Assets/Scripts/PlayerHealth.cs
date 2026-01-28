using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 20;
    public int currentHP;

    public event Action<int, int> OnHPChanged;
    public event Action OnPlayerDeath;

    private bool isDead = false;

    void Awake()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        
        OnHPChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnPlayerDeath?.Invoke();
    }
}
