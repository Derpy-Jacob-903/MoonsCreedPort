using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedBosses.MoonsCreedBossesCode.Powers;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

public abstract class CancerModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;

    public virtual int StrikeDamage => 0;
    public virtual decimal StrikeMult => 0;
    public virtual int DefendBlock => 0;
    public virtual decimal DefendMult => 0;
    public virtual int RegrowthMaxHpGain => 0;
    public int _regrowthMaxHpGain => RegrowthMaxHpGain;
    public virtual int BiteDamage => 0;
    public virtual int BiteHits => 3;
    public virtual decimal BiteHealPercent => 0;
    public virtual int GraspAmount => 0;

    protected override string VisualsPath => "res://animations/characters/ironclad.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<GraspPower>(new ThrowingPlayerChoiceContext(), Creature, GraspAmount, Creature, null);
        await PowerCmd.Apply<CancerVigorPower>(new ThrowingPlayerChoiceContext(), Creature, StrikeMult, Creature, null);
    }

    /// DON'T USE FOR DAMAGE
    public int CalculateDamage(decimal baseBlock, decimal maxHpMult)
    {
        if (_creature is null) return (int)Math.Floor(baseBlock + this.MaxInitialHp * maxHpMult);
        return (int)Math.Floor(baseBlock + _creature.MaxHp * maxHpMult);
    }
    
    public class GainMaxHpIntent : AbstractIntent
    {
        public override IntentType IntentType => IntentType.Heal;

        protected override string IntentPrefix => "MOONSCREEDBOSSES-GAINMAXHP";

        protected override string SpritePath => "atlases/intent_atlas.sprites/intent_heal.tres";
        
        ///Uses HealIntent's Animation
        public override string GetAnimation(IEnumerable<Creature> targets, Creature owner) => _cachedAnimationName ??= "HEAL".ToLowerInvariant();
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        
        var strikeState = new MoveState(
            "Strike",
            Strike,
            new SingleAttackIntent(StrikeDamage), new BuffIntent()
        );
        var defendState = new MoveState(
            "Defend",
            Defend, new DefendIntent()
        );
        var regrowthState = new MoveState(
            "Regrowth",
            Regrowth,
            new GainMaxHpIntent()
        );
        var twinStrikeState = new MoveState(
            "Sword_Boomerang",
            TwinStrike,
            new HealIntent(), new MultiAttackIntent(BiteDamage, BiteHits), new BuffIntent()
        );
        
        strikeState.FollowUpState = defendState;
        defendState.FollowUpState = regrowthState;
        regrowthState.FollowUpState = twinStrikeState;
        twinStrikeState.FollowUpState = strikeState;

        return new MonsterMoveStateMachine(
            new List<MonsterState> { strikeState, defendState, regrowthState, twinStrikeState },
            strikeState
        );
    }

    private async Task Regrowth(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainMaxHp(Creature, RegrowthMaxHpGain);
    }

    private async Task TwinStrike(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.Heal(Creature, CalculateDamage(0, BiteHealPercent));
        await DamageCmd.Attack(BiteDamage).WithHitCount(BiteHits)
            .FromMonster(this).WithHitFx("vfx/vfx_attack_slash")
            .Execute(null);
        await PowerCmd.Apply<CancerVigorPower>(new ThrowingPlayerChoiceContext(), Creature, StrikeMult, Creature, null);
    }
    
    private async Task Strike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StrikeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        
        await PowerCmd.Apply<CancerVimPower>(new ThrowingPlayerChoiceContext(), Creature, DefendMult, Creature, null);
    }
    
    private async Task Defend(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(Creature, CalculateDamage(DefendBlock, DefendMult), ValueProp.Move, null);
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