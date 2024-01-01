using MelonLoader;
using UnityEngine;
using Il2Cpp;
using HarmonyLib;

namespace VehicleFov
{
    public class Patches : MelonMod
    {
        private static float currentFov = 0f;

        [HarmonyPatch(typeof(vp_FPSCamera), "Start")]
        private static class GetInitialFov
        {
            private static void Postfix(vp_FPSCamera __instance)
            {
                currentFov = __instance.m_UnzoomedFieldOfView;
            }
        }

        [HarmonyPatch(typeof(Panel_OptionsMenu), "OnConfirmGraphicsOptions")]
        private static class SettingsChange
        {
            private static void Postfix(Panel_OptionsMenu __instance)
            {
                float fovSetting = __instance.m_FieldOfViewMin + ((__instance.m_FieldOfViewMax - __instance.m_FieldOfViewMin) * __instance.m_FieldOfViewSlider.value);
                float aspectRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;        
                currentFov = Camera.HorizontalToVerticalFieldOfView(fovSetting, aspectRatio);
            }
        }

        [HarmonyPatch(typeof(PlayerInVehicle), "EnterVehicle")]
        private static class SetCurrentFov
        {
            private static void Postfix(PlayerInVehicle __instance)
            {
                __instance.m_FOV = currentFov;
            }
        }       
    }
}