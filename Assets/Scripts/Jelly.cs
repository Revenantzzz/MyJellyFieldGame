using UnityEngine;

namespace JellyField
{
    public class Jelly : MonoBehaviour
    {
        JellyTypeSO jellyType;

        public void SetValue(JellyTypeSO jelly)
        {
            GetComponent<SpriteRenderer>().sprite = jelly.sprite;
            jellyType = jelly;
        }

        public JellyTypeSO GetValue()
        {
            return jellyType;
        }
    }
}
