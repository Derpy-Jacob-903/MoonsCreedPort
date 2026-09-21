using BaseLib.Abstracts;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedBosses.MoonsCreedBossesCode.Extensions;
using MoonsCreedBosses.MoonsCreedBossesCode.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public class OphanimModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 660, 600);
    public virtual int ReprisalAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 165, 240);

    public override string CustomVisualPath => "res://MoonsCreedBosses/Animations/Boss/Aquarius/kova.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<HardenedShellPower>(new ThrowingPlayerChoiceContext(), Creature, ReprisalAmount, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var randAState = new RandomBranchState(
            "RandA_Ophanim"
        );
        var randBState = new RandomBranchState(
            "RandB_Ophanim"
        );
        var randCState = new RandomBranchState(
            "RandC_Ophanim"
        );
        var randDState = new RandomBranchState(
            "RandD_Ophanim"
        );
        
        var BludgeonAState = new MoveState(
            "Bludgeon_Ophanim_A",
            Bludgeon,
            new SingleAttackIntent(BludgeonDamage)
        );
        randAState.AddBranch(BludgeonAState, MoveRepeatType.CanRepeatForever);
        BludgeonAState.FollowUpState = randBState;
        
        var BludgeonCState = new MoveState(
            "Bludgeon_Ophanim_C",
            Bludgeon,
            new SingleAttackIntent(BludgeonDamage)
        );
        randCState.AddBranch(BludgeonCState, MoveRepeatType.CanRepeatForever);
        BludgeonCState.FollowUpState = randDState;
        
        var AquarisDebuffState = new MoveState(
            "AquarisDebuff_Ophanim",
            AquarisDebuff,
            new BuffIntent()
        );
        randAState.AddBranch(AquarisDebuffState, MoveRepeatType.CanRepeatForever);
        AquarisDebuffState.FollowUpState = randCState;
        
        var ClotheslineAState = new MoveState(
            "Clothesline_Ophanim_A",
            Clothesline,
            new SingleAttackIntent(ClotheslineDamage)
        );
        randAState.AddBranch(ClotheslineAState, MoveRepeatType.CanRepeatForever);
        ClotheslineAState.FollowUpState = randBState;
        
        var ClotheslineCState = new MoveState(
            "Clothesline_Ophanim_C",
            Clothesline,
            new SingleAttackIntent(ClotheslineDamage)
        );
        randCState.AddBranch(ClotheslineCState, MoveRepeatType.CanRepeatForever);
        ClotheslineCState.FollowUpState = randDState;
        
        var IronWaveAState = new MoveState(
            "IronWave_Ophanim_A",
            Clothesline,
            new SingleAttackIntent(IronWaveDamage), new DefendIntent()
        );
        randAState.AddBranch(IronWaveAState, MoveRepeatType.CanRepeatForever);
        IronWaveAState.FollowUpState = randDState;
        
        var IronWaveCState = new MoveState(
            "IronWave_Ophanim_C",
            Clothesline,
            new SingleAttackIntent(IronWaveDamage), new DefendIntent()
        );
        randCState.AddBranch(IronWaveCState, MoveRepeatType.CanRepeatForever);
        IronWaveCState.FollowUpState = randDState;
        
        var AriesBuffState = new MoveState(
            "AriesBuff_Ophanim",
            AriesBuff,
            new BuffIntent()
        );
        randAState.AddBranch(AriesBuffState, MoveRepeatType.CanRepeatForever);
        AriesBuffState.FollowUpState = randCState;

        return new MonsterMoveStateMachine(
            new List<MonsterState> 
                { randAState, randBState, randCState, randDState, 
                    BludgeonAState, BludgeonCState, ClotheslineAState, ClotheslineCState, IronWaveAState, IronWaveCState,
                    AquarisDebuffState, AriesBuffState },
            randAState
        );
    }

    private async Task Clothesline(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(ClotheslineDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(null);
        foreach (var target in targets.Where(t => t.IsAlive))
        {
            await Cmd.Wait(0.2f);
            await PowerCmd.Apply<DrainedPower>(new ThrowingPlayerChoiceContext(), target, 1m, Creature, null);
        }
    }
    public int ClotheslineDamage => 25;
    
    private async Task Bludgeon(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(BludgeonDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(null);
    }

    public int BludgeonDamage => 50;
    
    
    private async Task IronWave(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(IronWaveDamage)
            .FromMonster(this).WithHitFx("vfx/vfx_flying_slash")
            .Execute(null);
        await CreatureCmd.GainBlock(Creature, IronWaveBlock, ValueProp.Move, null);
    }
    public int IronWaveBlock => 33;
    public int IronWaveDamage => 33;

    private async Task AquarisDebuff(IReadOnlyList<Creature> targets)
    {
         await PowerCmd.Apply<ReprisalPower>(new ThrowingPlayerChoiceContext(), Creature, 3, null, null);
    }
    public int Reprisal => 3;
    
    private async Task AriesBuff(IReadOnlyList<Creature> targets)
    {
        await PowerCmd.Apply<FuryPower>(new ThrowingPlayerChoiceContext(), Creature, 1, null, null);
        if (Creature.GetPower<FuryPower>() != null)
        {
            Creature.GetPower<FuryPower>()!.DynamicVars["Threshold"].BaseValue = 9;
        }
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        var idle = new AnimState("Idle", true);
        var attack = new AnimState("MeleAttack"); //[sic]
        var cast = new AnimState("Cast");
        var cast2 = new AnimState("Cast2");
        var hit = new AnimState("Hit");
        var died = new AnimState("Death");
        
        attack.NextState = idle;
        cast.NextState = idle;
        cast2.NextState = idle;
        cast.NextState = idle;

        var animator = new CreatureAnimator(idle, controller);
        
        animator.AddAnyState("idle_loop", idle);
        animator.AddAnyState("Dead", died);
        animator.AddAnyState("Attack", attack);
        animator.AddAnyState("Cast", cast);
        animator.AddAnyState("CastTwo", cast2);
        animator.AddAnyState("Hit", hit);
        animator.AddAnyState("Dead", died);
        return animator;
    }
}

public class OphanimFourEncounter() : ZodiacEncounterModel()
{
    public override bool IsValidForAct(ActModel act) => /*act.ActNumber() == 4 ||*/ act is OphanimAct;
    public override Element myElement => Element.Chaos;
    public override string BossNodePath => "res://MoonsCreedBosses/images/map_icons/Ophanim";
    public override string CustomRunHistoryIconPath => "res://MoonsCreedBosses/images/run_history_icon/Ophanim.png";

    public override string CustomRunHistoryIconOutlinePath =>
        "res://MoonsCreedBosses/images/run_history_icon/Ophanim.png";

    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<OphanimModel>()];

    protected override IReadOnlyList<(MonsterModel, string)> GenerateMonsters()
    {
        return new List<(MonsterModel, string)> { (ModelDb.Monster<OphanimModel>().ToMutable(), null) };
    }
}

/*
public class EvilMapPointTypeCounts : MapPointTypeCounts
{
    public override int NumOfElites => 0;
    public override int NumOfShops => 1;
    public override int NumOfUnknowns => 0;
    public override int NumOfRests => 1;
    public EvilMapPointTypeCounts(int unknownCount, int restCount) : base(unknownCount, restCount)
    {
    }

    public EvilMapPointTypeCounts(ActMap existingMap) : base(existingMap)
    {
    }
}*/

public class DummyModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;
    public override int MaxInitialHp => 1;

    public override string CustomVisualPath => "res://MoonsCreedBosses/Animations/Boss/Aquarius/kova.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), Creature, 2, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        
        var BludgeonAState = new MoveState(
            "Dummy_A",
            Clothesline,
            new SleepIntent()
        );
        BludgeonAState.FollowUpState = BludgeonAState;

        return new MonsterMoveStateMachine(
            new List<MonsterState> {BludgeonAState}, BludgeonAState
        );
    }

    private async Task Clothesline(IReadOnlyList<Creature> targets)
    {
        
    }
}

public class OphanimAct() : CustomActModel(4)
{
    public override IEnumerable<EncounterModel> GenerateAllEncounters()
    {
        return [];
    }

    public override IEnumerable<AncientEventModel> AllAncients => _AllAncients();
    
    public IEnumerable<AncientEventModel> _AllAncients()
    {
        /*foreach (var a in AllAncients)
        {
            if (a is Darv)
            {
                continue;
            }
            //
        }*/
        return [ModelDb.AncientEvent<Darv>()];
    }
    public override IEnumerable<EventModel> AllEvents => [];
    protected override int BaseNumberOfRooms => 2;
    protected override string CustomMapTopBgPath => ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_top_{this.FilePathIdentifier}.png");
    protected override string CustomMapMidBgPath => ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_middle_{this.FilePathIdentifier}.png");
    protected override string CustomMapBotBgPath => ImageHelper.GetImagePath($"packed/map/map_bgs/{this.FilePathIdentifier}/map_bottom_{this.FilePathIdentifier}.png");
    protected override string CustomRestSiteBackgroundPath => SceneHelper.GetScenePath($"rest_site/{this.FilePathIdentifier}_rest_site");
    public override MapPointTypeCounts GetMapPointTypes(Rng mapRng)
    {
        return new MapPointTypeCounts(0, 1);
    }
    public ActMap CreateMap()
    {
        return (ActMap)(object)new TheEndingMap();
    }
    
    private class TheEndingMap : ActMap
    {
        protected override MapPoint[,] Grid { get; }

        public override MapPoint StartingMapPoint { get; }

        public override MapPoint BossMapPoint { get; }

        public TheEndingMap()
        {
            MapPoint[,] array = new MapPoint[7, 13];
            MapPoint val = new MapPoint(3, 0)
            {
                PointType = MapPointType.RestSite
            };
            array[3, 1] = new MapPoint(3, 1)
            {
                PointType = MapPointType.Shop
            };
            /*array[3, 2] = new MapPoint(3, 2)
            {
                PointType = MapPointType.Elite
            };*/
            MapPoint val2 = new MapPoint(3, 2)
            {
                PointType = MapPointType.Boss
            };
            base.startMapPoints.Add(val);
            val.AddChildPoint(array[3, 1]);
            //array[3, 1].AddChildPoint(array[3, 2]);
            array[3, 1].AddChildPoint(val2);
            Grid = array;
            StartingMapPoint = val;
            BossMapPoint = val2;
        }
    }
}

[HarmonyPatch(typeof(ActModel), "CreateMap")]
class CreateAct4Map_Before_CreateMap
{
    [HarmonyPrefix]
    static bool MapPrefix(ActModel __instance, ref ActMap __result)
    {
        if (__instance is OphanimAct theEnding)
        {
            __result = theEnding.CreateMap();
            return false;
        }
        return true;
    }
}