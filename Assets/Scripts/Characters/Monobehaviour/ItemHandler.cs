using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace GodsGame
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public class ItemHandler : MonoBehaviour
    {
        #region Public Var
        public GameObject goItemUi;
        public GameObject itemSocket;
        #endregion

        #region Private Var
        private Image m_ItemUIImage;
        private CooldownSkillUI m_CooldownSkillUI;
        private PlayerBehaviour player;
        #endregion

        #region Properties
        public BaseItem Item { get; private set; }
        #endregion

        public ItemHandler(GameObject itemSocket)
        {
            this.itemSocket = itemSocket;
        }

        private void Start()
        {
            player = GetComponent<PlayerBehaviour>();
            if (goItemUi != null)
            {
                m_ItemUIImage = goItemUi.GetComponent<Image>();
                m_CooldownSkillUI = goItemUi.GetComponent<CooldownSkillUI>();
            }
        }

        public void OnCollisionEnter(Collision col)
        {
            if (col.transform.tag == "Item" && !IsItemEquiped())
            {
                Item = col.transform.GetComponent<BaseItem>();
                Item.PickUpItem(player, itemSocket);
                if (m_ItemUIImage)
                {
                    m_ItemUIImage.color = Color.white;
                    m_ItemUIImage.sprite = Item.spriteUI;
                }
                if (m_CooldownSkillUI)
                    m_CooldownSkillUI.CooldownSystem = Item.CooldownSystem;
            }
        }

        public bool CanThrow()
        {
            return IsItemEquiped() && Item.IsThrowable;
        }

        public void ThrowItem()
        {
            if (!Item.Using)
            {
                Item.ThrowItem(transform, 30);
                Item = null;
                if (m_ItemUIImage)
                {
                    m_ItemUIImage.color = new Color(0, 0, 0, 0);
                    m_ItemUIImage.sprite = null;
                }
            }
        }

        IEnumerator DeactivateUsing(float duration)
        {
            yield return new WaitForSeconds(duration);
            Item.Using = false;
        }

        public bool IsItemEquiped()
        {
            return Item;
        }

        public bool CanUseItem()
        {
            return IsItemEquiped() && Item.CanUse();
        }

        public void UseItem()
        {
            Item.UseItem();
            Item.Using = true;
            StartCoroutine(DeactivateUsing(Item.Cooldown));
        }
    }
}

