using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utility;
using Utility.PoolManager;

namespace Units {
    public class Spawner : PoolableObject {
        public Transform spriteRenderer;
        public Transform spriteMask;
        public SpriteRenderer icon;

        private Vector3 _firstScale = Vector3.one * 0.1f;
        private Vector3 _offset = Vector3.one * 0.015f;

        public static float timeToSpawn = 2.5f;
        private float _time;

        public void UpdateTime() {
            if (!IsActive()) return;
            _time -= Time.deltaTime;
            float lerp = Mathf.InverseLerp(0f, timeToSpawn, _time);
            spriteRenderer.localScale = lerp * _firstScale;
            spriteMask.localScale = lerp * _firstScale - _offset;
            if(_time > timeToSpawn)
                DeInit();
        }

        public override void Init() {
            base.Init();
            spriteRenderer.localScale = _firstScale;
            spriteMask.localScale = _firstScale - _offset;
            _time = timeToSpawn;
        }

        public void Init(EntityType unitType) {
            Init();
            icon.sprite = GameSingleton.Instance.entityTypeToSprite.GetEntitySprite(unitType);
        }

        public override void DeInit() {
            base.DeInit();
            SetAnimating(false);
        }

        private List<Tween> _animations = new List<Tween>();
        public void SetAnimating(bool state) {
            if (state == false) {
                for(int i = _animations.Count-1; i >= 0; --i)
                    _animations[i].Kill();
                _animations.Clear();
            }
            else {
                _animations.Add(spriteRenderer.DOScale(_offset, 0.5f).SetLoops(-1, LoopType.Yoyo));
                _animations.Add(spriteMask.DOScale(Vector3.zero, 0.5f).SetLoops(-1, LoopType.Yoyo));
            }
        }
    }
}
