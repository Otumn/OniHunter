using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    public class VersionCode : ScriptableWizard
    {
        [SerializeField] private int newVersionCode = 1;

        [MenuItem("OniHunter/Version code")]
        private static void CreateTower()
        {
            VersionCode versionCode = ScriptableWizard.DisplayWizard<VersionCode>("Set version code", "Set");
            versionCode.newVersionCode = PlayerSettings.Android.bundleVersionCode;
        }

        private void OnWizardCreate()
        {
            PlayerSettings.Android.bundleVersionCode = newVersionCode;
        }
    }
}
