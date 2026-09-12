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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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

public abstract class CancerModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;

    public virtual int StrikeDamage => 0;
    public virtual decimal StrikeMult => 0;
    public virtual int DefendBlock => 0;
    public virtual decimal DefendMult => 0;
    public virtual int RegrowthMaxHpGain => 0;
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
    }

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
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        Creature fuck = this._creature ?? null;
        var strikeState = new MoveState(
            "Strike_Cancer_" + Sub,
            Strike,
            [new SingleAttackIntent(CalculateDamage(StrikeDamage, StrikeMult))]
        );
        var defendState = new MoveState(
            "Defend_Cancer_" + Sub,
            Defend, new DefendIntent());
        var regrowthState = new MoveState(
            "Regrowth_Cancer_" + Sub,
            Regrowth,
            [new GainMaxHpIntent()]
        );
        var twinStrikeState = new MoveState(
            "Sword_Boomerang_Cancer_" + Sub,
            TwinStrike,
            [new MultiAttackIntent(BiteDamage, BiteHits), new HealIntent()]
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
        await DamageCmd.Attack(BiteDamage).WithHitCount(BiteHits)
            .FromMonster(this).WithHitFx("vfx/vfx_attack_slash")
            .Execute(null);
        await CreatureCmd.Heal(Creature, CalculateDamage(0, BiteHealPercent));
    }
    
    private async Task Strike(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(StrikeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
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