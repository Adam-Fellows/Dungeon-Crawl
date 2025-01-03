using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawl.Data
{
    [CreateAssetMenu(menuName = "Data/Units/New Unit", fileName = "NewUnitStats")]
    public class UnitStats : ScriptableObject
    {
        public string unitName;

        public int HP;
        public int attackPower;
        public int defense;
    }
}
