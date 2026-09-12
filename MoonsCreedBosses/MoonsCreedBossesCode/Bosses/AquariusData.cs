using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MoonsCreedBosses.MoonsCreedBossesCode.Extensions;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public class AriesOne : AriesModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 131, 110);
    public override string Sub => "1";
    
    public override int TwinStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
    public override int IronWaveDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
    public override int IronWaveBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
    public override int HeavyStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 18);
    public override int FuryAmount => 1;
    public override int FuryThreshold => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 8);
}
public class AriesTwo : AriesModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 274, 240);
    public override string Sub => "2";
    public override int TwinStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
    public override int IronWaveDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
    public override int IronWaveBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
    public override int HeavyStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 18);
    public override int FuryAmount => 1;
    public override int FuryThreshold => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 8);
}
public class AriesThree : AriesModel
{
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 417, 370);
    public override string Sub => "3";
    public override int TwinStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
    public override int DefendBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 21);
    public override int IronWaveDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
    public override int IronWaveBlock => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
    public override int HeavyStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 21, 18);
    public override int FuryAmount => 2;
    public override int FuryThreshold => 9;
}

public class AriesOneEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 1;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aries";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AriesOne>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AriesOne>().ToMutable(), null) }; }
}
public class AriesTwoEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 2;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aries";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AriesTwo>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AriesTwo>().ToMutable(), null) }; }
}
public class AriesThreeEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == 3;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Aries";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override string CustomRunHistoryIconOutlinePath => "res://MoonsCreedBosses/images/run_history_icon/Aries.png";
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AriesThree>()];
    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters() { return new List<(MonsterModel, string)> { (ModelDb.Monster<AriesThree>().ToMutable(), null) }; }
}