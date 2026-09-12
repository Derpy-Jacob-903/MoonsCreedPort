using BaseLib.Abstracts;
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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedBosses.MoonsCreedBossesCode.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public abstract class AriesModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;

    public virtual int TwinStrikeDamage => 0;
    public virtual int TwinStrikeHits => 2;
    public virtual int DefendBlock => 0;
    public virtual int IronWaveDamage => 0;
    public virtual int IronWaveBlock => 0;
    public virtual int HeavyStrikeDamage => 0;
    public virtual int FuryAmount => 1;
    public virtual int FuryThreshold => 9;

    protected override string VisualsPath => "res://animations/characters/ironclad.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<FuryPower>(new ThrowingPlayerChoiceContext(), Creature, FuryAmount, Creature, null);
        if (Creature.GetPower<FuryPower>() != null)
        {
            Creature.GetPower<FuryPower>()!.DynamicVars["Threshold"].BaseValue = FuryThreshold;
        }
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var twinStrikeAState = new MoveState(
            "Twin_Strike_A_Aries_" + Sub,
            TwinStrike,
            [new MultiAttackIntent(TwinStrikeDamage, TwinStrikeHits)]
        );
        
        var defendBState = new MoveState(
            "Defend_B_Aquarius_" + Sub,
            Defend, new DefendIntent());
        
        var twinStrikeCState = new MoveState(
            "Twin_Strike_C_Aries_" + Sub,
            TwinStrike,
            [new MultiAttackIntent(TwinStrikeDamage, TwinStrikeHits)]
        );
        var ironWaveState = new MoveState(
            "Iron_Wave_D_Aries_" + Sub,
            IronWave,
            [new SingleAttackIntent(IronWaveDamage), new DefendIntent()]
        );
        
        // twinStrike or defend at random
        var randState = new RandomBranchState(
            "RandState_E_Aries_" + Sub
        );
        var twinStrikeEState = new MoveState(
            "Twin_Strike_E_Aries_" + Sub,
            TwinStrike,
            [new MultiAttackIntent(TwinStrikeDamage, TwinStrikeHits)]
        );
        var defendEState = new MoveState(
            "Defend_E_Aquarius_" + Sub,
            Defend, new DefendIntent());
        //both lead to heavyBlade
        var heavyBladeState = new MoveState(
            "Heavy_Blade_Aquarius_" + Sub,
            HeavyStrike,
            new SingleAttackIntent(HeavyStrikeDamage)
        );
        var defendGState = new MoveState(
            "DefendG_Aquarius_" + Sub,
            Defend, new DefendIntent());
        
        
        twinStrikeAState.FollowUpState = defendBState;
        defendBState.FollowUpState = twinStrikeCState;
        twinStrikeCState.FollowUpState = randState;
        //Loop(
            ironWaveState.FollowUpState = randState;
            randState.AddBranch(twinStrikeEState, MoveRepeatType.CannotRepeat);
            randState.AddBranch(defendEState, MoveRepeatType.CannotRepeat);
            twinStrikeEState.FollowUpState = heavyBladeState;
            defendEState.FollowUpState = heavyBladeState;
            heavyBladeState.FollowUpState = defendGState;
            defendGState.FollowUpState = ironWaveState;
        //)

        return new MonsterMoveStateMachine(
            new List<MonsterState> { twinStrikeAState, defendBState, twinStrikeCState, ironWaveState, randState, twinStrikeEState, defendEState, heavyBladeState, defendGState },
            twinStrikeAState
        );
    }
    
    private async Task TwinStrike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(TwinStrikeDamage).WithHitCount(TwinStrikeHits)
            .FromMonster(this).WithHitFx("vfx/vfx_attack_slash")
            .Execute(null);
    }
    
    private async Task HeavyStrike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(HeavyStrikeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    private async Task IronWave(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(IronWaveDamage)
            .FromMonster(this).WithHitFx("vfx/vfx_flying_slash")
            .Execute(null);
        await CreatureCmd.GainBlock(Creature, IronWaveBlock, ValueProp.Move, null);
    }
    
    private async Task Defend(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(Creature, DefendBlock, ValueProp.Move, null);
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        var idle = new AnimState("Idle", true);
        //var attack = new AnimState("Attack_2");
        var hit = new AnimState("Hit");

        //attack.NextState = idle;
        hit.NextState = idle;

        var animator = new CreatureAnimator(idle, controller);
        //animator.AddAnyState("Beam", attack);
        animator.AddAnyState("Hit", hit);

        return animator;
    }
}