using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class SfxArray : EntityComponentDefinition
    {
        public List<AudioClip> files = new List<AudioClip>();
        public float volume = 1f;
    }
}