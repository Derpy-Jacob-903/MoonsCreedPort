using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MoonsCreedBosses.MoonsCreedBossesCode.Extensions;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public class CancerOne : CancerModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 220, 200);
    public override string Sub => "1";

    public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
    public override decimal StrikeMult => 7;
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
    public override decimal DefendMult => 3;
    public override int RegrowthMaxHpGain => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
    public override int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
    public override decimal BiteHealPercent => 0.04m;
    public override int GraspAmount => 8;

    public class CancerTwo : CancerModel
    {
        public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 363, 330);
        public override string Sub => "2";
        public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
        public override decimal StrikeMult => 4;
        public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
        public override decimal DefendMult => 3;

        public override int RegrowthMaxHpGain =>
            AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);

        public override int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
        public override decimal BiteHealPercent => 0.02m;
        public override int GraspAmount => 10;
    }

    public class CancerThree : CancerModel
    {
        public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 506, 460);
        public override string Sub => "3";
        public override int StrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
        public override decimal StrikeMult => 2;
        public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
        public override decimal DefendMult => 3;

        public override int RegrowthMaxHpGain =>
            AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 27, 25);

        public override int BiteDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
        public override decimal BiteHealPercent => 0.01m;
        public override int GraspAmount => 12;
    }

    public class CancerOneEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 1 && act is not Overgrowth;
        public override Element myElement => Element.Water;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Cancer";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CancerOne>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CancerOne>().ToMutable(), null) };
        }
    }

    public class CancerTwoEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 2 /*&& act is not Hive*/;
        public override Element myElement => Element.Water;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Cancer";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CancerTwo>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CancerTwo>().ToMutable(), null) };
        }
    }

    public class CancerThreeEncounter() : ZodiacEncounterModel()
    {
        public override bool IsValidForAct(ActModel act) => act.ActNumber() == 3;
        public override Element myElement => Element.Water;
        public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Cancer";
        public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override string CustomRunHistoryIconOutlinePath =>
            "res://MoonsCreedBosses/images/run_history_icon/Cancer.png";

        public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<CancerThree>()];

        protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
        {
            return new List<(MonsterModel, string)> { (ModelDb.Monster<CancerThree>().ToMutable(), null) };
        }
    }
}