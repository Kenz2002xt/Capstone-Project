using UnityEngine;

namespace Hunger.Gameplay
{
    public class RoomItemManager : MonoBehaviour
    {
        public GameObject[] roomItems;

        public int itemsToActivate = 2;

        public void GenerateRoomItems()
        {
            foreach (GameObject item in roomItems)
            {
                item.SetActive(false);
            }

            for (int i = 0; i < itemsToActivate; i++)
            {
                int rand = Random.Range(0, roomItems.Length);
                roomItems[rand].SetActive(true);
            }
        }
    }
}