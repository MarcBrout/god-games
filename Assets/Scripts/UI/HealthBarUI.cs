using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GodsGame
{
    public class HealthBarUI : MonoBehaviour
    {
        public string targetName = "Player";
        public RectTransform healthSeparatorPrefab;
        public Transform healthBar;

        [Header("HealthBar")]
        public Slider sliderHealthBar;
        public float healthShrinkTime = 0.2f;

        [Header("HithBar")]
        public Slider sliderHitBar;
        public float hitShrinkTime = 0.4f;

        public AnimationCurve speedCurve;

        private void Start()
        {
            GameObject go = GameObject.Find(targetName);
            if (go == null)
                throw new System.Exception("HealthBarUI: Target not found => " + targetName);
            Damageable damageable = go.GetComponent<Damageable>();
            if (damageable == null)
                throw new System.Exception("HealthBarUI: Target has no damageable attached => " + targetName);
            damageable.OnTakeDamage.AddListener(UpdateHealthUI);
            Init(damageable.startingHealth);
        }

        private void Init(int hp)
        {
            sliderHealthBar.maxValue = hp;
            sliderHealthBar.value = hp;
            sliderHitBar.maxValue = hp;
            sliderHitBar.value = hp;
            float barWidth = sliderHealthBar.fillRect.sizeDelta.x;
            float barDivide = barWidth / hp;

            RectTransform healthSliderfill = healthBar.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            for (int i = 0; i < hp - 1; ++i)
            {
                RectTransform separator = Instantiate(healthSeparatorPrefab);
                separator.SetParent(healthBar);
                separator.offsetMin = new Vector2(separator.offsetMin.x, 0);
                separator.offsetMax = new Vector2(separator.offsetMax.x, 0);
                --sliderHealthBar.value;
                separator.anchoredPosition = new Vector2(healthSliderfill.rect.width, 0);
            }
            sliderHealthBar.value = hp;
        }

        public void UpdateHealthUI(Damager damager, Damageable damageable)
        {
            int damage = damager.damage;
            ShrinkHealthBar(damage);
            this.DelayAction(healthShrinkTime, () => ShrinkHitBar(damage));
        }

        private void ShrinkHealthBar(int damage)
        {
            StartCoroutine(ShrinkOverSpeed(sliderHealthBar, healthShrinkTime, damage));
        }

        private void ShrinkHitBar(int damage)
        {
            StartCoroutine(ShrinkOverSpeed(sliderHitBar, hitShrinkTime, damage));
        }

        private IEnumerator ShrinkOverSpeed(Slider slider, float shrinkTime, int damage)
        {
            float timer = 0;
            float newHp = slider.value - damage;
            float sliderPos = slider.value;

            while (timer <= shrinkTime)
            {
                slider.value = Mathf.Lerp(sliderPos, newHp, speedCurve.Evaluate(timer / shrinkTime));
                timer += Time.deltaTime;
                yield return null;
            }
            slider.value = newHp;
        }
    }
}
