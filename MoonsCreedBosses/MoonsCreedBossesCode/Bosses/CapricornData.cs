using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MoonsCreedBosses.MoonsCreedBossesCode.Extensions;
using MoonsCreedBosses.MoonsCreedBossesCode.Powers;
using ConditionalBranchState = MoonsCreedBosses.MoonsCreedBossesCode.Extensions.ConditionalBranchState;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public class CapricornOne : CapricornModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 231, 210);
    public override string Sub => "1";

    public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 17);
    public override int StartingEndurance => 4;
    public override int MaxEndurance => 6;

    public class CapricornTwo : CapricornModel
    {
        public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 363, 330);
        public override string Sub => "2";
        public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 18);
        public override int StartingEndurance => 5;
        public override int MaxEndurance => 7;
    }

    public class CapricornThree : CapricornModel
    {
        public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 495, 460);
        public override string Sub => "3";
        public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31, 30);
        //public int StrikeBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 31, 22);
        public override int StartingEndurance => 3;
        public override int MaxEndurance => 7;
    }

    public class CapricornOneEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 1 && act is not Underdocks;
        public override Element myElement => Element.Earth;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Capricorn";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CapricornOne>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CapricornOne>().ToMutable(), null) };
        }
    }

    public class CapricornTwoEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 2 /*&& act is not Hive*/;
        public override Element myElement => Element.Earth;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Capricorn";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CapricornTwo>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CapricornTwo>().ToMutable(), null) };
        }
    }

    public class CapricornThreeEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 3;
        public override Element myElement => Element.Earth;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Capricorn";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Capricorn.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CapricornThree>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CapricornThree>().ToMutable(), null) };
        }
    }
}