using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MoonsCreedBosses.MoonsCreedBossesCode.Powers;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public abstract class CapricornModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;

    public virtual int StrikeDamage => 0;
    public virtual int MaxEndurance => 1;
    private int _MaxEndurance => _creature?.CombatState?.Players.Count * MaxEndurance ?? MaxEndurance;
    public virtual int StartingEndurance => 0;

    protected override string VisualsPath => "res://animations/characters/ironclad.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<EndurancePower>(new ThrowingPlayerChoiceContext(), Creature, StartingEndurance, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var mainState = new ConditionalBranchState("MainState");
        var strikeState = new MoveState(
            "Strike",
            Strike,
            [new SingleAttackIntent(StrikeDamage), new BuffIntent()]
        );
        var fightMeState = new MoveState(
            "FightMe",
            StrikeTwo,
            [new SingleAttackIntent(StrikeDamage), new UnknownIntent(), new BuffIntent()]
        );
        
        mainState.AddState(fightMeState, () => _creature is not null && _MaxEndurance <= _creature.GetPowerAmount<EndurancePower>());
        mainState.AddState(strikeState, () => !(_creature is not null && _MaxEndurance <= _creature.GetPowerAmount<EndurancePower>()));
        
        strikeState.FollowUpState = mainState;
        fightMeState.FollowUpState = mainState;

        return new MonsterMoveStateMachine(
            new List<MonsterState> { mainState, strikeState, fightMeState },
            mainState
        );
    }
    
    private async Task Strike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StrikeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        await PowerCmd.Apply<EndurancePower>(new ThrowingPlayerChoiceContext(), Creature, _creature?.CombatState?.Players.Count ?? 1, Creature, null);
    }
    
    private async Task StrikeTwo(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StrikeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        await PowerCmd.Remove<EndurancePower>(Creature);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, 3, Creature, null);
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