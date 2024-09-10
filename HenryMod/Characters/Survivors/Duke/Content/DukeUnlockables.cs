using DukeMod.Survivors.Duke.Achievements;
using RoR2;
using UnityEngine;

namespace DukeMod.Survivors.Duke
{
    public static class DukeUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                DukeMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(DukeMasteryAchievement.identifier),
                DukeSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
