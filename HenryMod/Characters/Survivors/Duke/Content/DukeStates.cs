using DukeMod.Survivors.Duke.SkillStates;

namespace DukeMod.Survivors.Duke
{
    public static class DukeStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));
            Modules.Content.AddEntityState(typeof(RoyalGun));

            Modules.Content.AddEntityState(typeof(SimpleAmbush));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
            Modules.Content.AddEntityState(typeof(KineticReplicator));
        }
    }
}
