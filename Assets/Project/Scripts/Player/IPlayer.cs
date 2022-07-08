using UnityEngine;

namespace Project.Scripts.Player
{
    public interface IPlayer
    {
        public void OnDamage(GameObject cause);
    }
}