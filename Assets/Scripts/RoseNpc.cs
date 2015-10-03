using UnityEngine;
using System.Collections;

public class RoseNpc : RoseCharacter
{
    public RoseNpcData data;

    public void UpdateModels()
    {
        parts.Clear();
        for (var i = 0; i < data.parts.Count; ++i)
        {
            parts.Add(data.parts[i]);
        }

        skeleton = data.skeleton;

        UpdateData();

		var animator = gameObject.GetComponent<Animation> ();
		if (animator == null) {
			animator = gameObject.AddComponent<Animation> ();
		}
		for (var i = 0; i < data.animations.Count; ++i) {
			var anim = data.animations[i];
			if (anim != null) {
				animator.AddClip(anim, anim.name);
				if (animator.clip == null) {
					animator.clip = anim;
				}
			}
		}
    }
}
