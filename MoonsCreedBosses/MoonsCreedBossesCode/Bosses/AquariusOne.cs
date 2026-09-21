using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves.Managers;
using MoonsCreedBosses.MoonsCreedBossesCode.Extensions;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public class AquariusOne : AquariusModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 264, 240);
    public override int ClotheslineDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
    public override int DrownDrowning => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    public override int DrownHeirless => 0;
    public override int ClotheslineDrained => 1;
    public override int HeavyBladeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
    public override int ReprisalAmount => 3;
    public override string Sub => "1";
}
public class AquariusTwo : AquariusModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 341, 310);
    public override int ClotheslineDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
    public override int DrownDrowning => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    public override int DrownHeirless => 0;
    public override int ClotheslineDrained => 1;
    public override int HeavyBladeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 24, 22);
    public override int ReprisalAmount => 4;
    public override string Sub => "2";
}
public class AquariusThree : AquariusModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 517, 470);
    public override int ClotheslineDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 14);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 14);
    public override int DrownDrowning => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
    public override int DrownHeirless => 1;
    public override int ClotheslineDrained => 1;
    public override int HeavyBladeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 26, 24);
    public override int ReprisalAmount => 4;
    public override string Sub => "3";
    
    public override MoveState SetupDrownState(string s)
    {
        var drownState = new MoveState(
            "Drown" + s + "_Aquarius_" + Sub,
            Drown,
            new AbstractIntent[] { new StatusIntent(DrownDrowning), new DebuffIntent() }
        );
        return drownState;
    }
}

public class AquariusOneEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 1 && act is not Overgrowth;
    public override Element myElement => Element.Air;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aquarius";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AquariusOne>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AquariusOne>().ToMutable(), null) }; }
}
public class AquariusTwoEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 2 /*&& act is not Hive*/;
    public override Element myElement => Element.Air;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aquarius";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AquariusTwo>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AquariusTwo>().ToMutable(), null) }; }
}
public class AquariusThreeEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 3;
    public override Element myElement => Element.Air;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aquarius";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aquarius.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AquariusThree>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AquariusThree>().ToMutable(), null) }; }
}