using RoR2;
using DukeMod.Modules.Achievements;

namespace DukeMod.Survivors.Duke.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class DukeMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = DukeSurvivor.DUKE_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = DukeSurvivor.DUKE_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => DukeSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}