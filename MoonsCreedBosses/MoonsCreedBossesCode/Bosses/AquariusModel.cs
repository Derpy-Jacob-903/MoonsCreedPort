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

public abstract class AquariusModel : CustomMonsterModel
{
    public override int MinInitialHp => MaxInitialHp;
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 265, 250);

    public virtual int ReprisalAmount => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 3);
    public virtual int ClotheslineDamage => 13; //=> AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);
    public virtual int ClotheslineDrained => 1; //=> AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);
    public virtual int DefendBlock => 16;
    public virtual int DrownDrowning => 2;
    public virtual int DrownHeirless => 0;
    public virtual int HeavyBladeDamage => 22; //=> AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 10);

    public override string CustomVisualPath => "res://MoonsCreedBosses/Animations/Boss/Aquarius/kova.tscn";
    public virtual string Sub => "";

    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<ReprisalPower>(new ThrowingPlayerChoiceContext(), Creature, ReprisalAmount, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var randState = new RandomBranchState(
            "RandState"
        );
        
        //Damage and Stagger
        var clotheslineAState = new MoveState(
            "ClotheslineA",
            Clothesline,
            [new SingleAttackIntent(ClotheslineDamage), new DebuffIntent()]
        );
        //Damage and Stagger
        var clotheslineState = new MoveState(
            "Clothesline",
            Clothesline,
            [new SingleAttackIntent(ClotheslineDamage), new DebuffIntent()]
        );
        
        //Block
        var defendXState = new MoveState(
            "Defend",
            Defend, new DefendIntent());
        var defendYState = new MoveState(
            "DefendY",
            Defend, new DefendIntent()
        );
        
        //Status
        var drownXState = SetupDrownState(""); //need to override for Act 3 Aquarius
        var drownYState = SetupDrownState("Y"); //need to override for Act 3 Aquarius
        
        var heavyBladeState = new MoveState(
            "Heavy_Blade",
            HeavyBlade,
            new SingleAttackIntent(HeavyBladeDamage)
        );
        
        clotheslineAState.FollowUpState = randState;
        randState.AddBranch(defendXState, MoveRepeatType.CannotRepeat);
        randState.AddBranch(drownXState, MoveRepeatType.CannotRepeat);
        defendXState.FollowUpState = drownYState;
        drownXState.FollowUpState = defendYState;
        defendYState.FollowUpState = clotheslineState;
        drownYState.FollowUpState = clotheslineState;
        clotheslineState.FollowUpState = heavyBladeState;
        heavyBladeState.FollowUpState = randState;

        return new MonsterMoveStateMachine(
            new List<MonsterState> { clotheslineAState, randState, defendXState, drownXState, defendYState, drownYState, clotheslineState, heavyBladeState },
            clotheslineAState
        );
    }

    public virtual MoveState SetupDrownState(string s)
    {
        var drownState = new MoveState(
            "Drown" + s,
            Drown,
            new AbstractIntent[] { new StatusIntent(DrownDrowning) }
        );
        return drownState;
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
            if (Creature.IsPlayer)
                await PowerCmd.Apply<DrainedPower>(new ThrowingPlayerChoiceContext(), target, 1m, Creature, null);
            else
                await PowerCmd.Apply<FriendlyAquariusDebuffPower>(new ThrowingPlayerChoiceContext(), target, 1m, Creature, null);
        }
    }
    
    private async Task HeavyBlade(IReadOnlyList<Creature> targets)
    {
        await DamageCmd.Attack(HeavyBladeDamage)
            .FromMonster(this)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(null);
    }

    protected async Task Drown(IReadOnlyList<Creature> targets)
    {
        //if (Creature.Side == CombatSide.Enemy)
        //{
            foreach (var playerCreature in targets.Where(t => t.Player != null))
            {
                var combatState = playerCreature.CombatState;
                var statusCards = new CardPileAddResult[DrownDrowning];
                for (int i = 0; i < DrownDrowning; i++)
                {
                    if (combatState == null || playerCreature.Player == null) continue;
                    var burn = makeDrowning(playerCreature.Player);
                    burn.UpgradeInternal();
                    burn.FinalizeUpgradeInternal();
                    statusCards[i] = await CardPileCmd.AddGeneratedCardToCombat(burn, PileType.Draw, null, CardPilePosition.Random);
                }
                CardCmd.PreviewCardPileAdd(statusCards, style: CardPreviewStyle.HorizontalLayout);
                if (DrownHeirless <= 0) continue;
                if (Creature.IsPlayer)
                    await PowerCmd.Apply<HeirlessPower>(new ThrowingPlayerChoiceContext(), playerCreature, (decimal)DrownHeirless, Creature, null);
                else
                    await PowerCmd.Apply<FriendlyAquariusDebuffPower>(new ThrowingPlayerChoiceContext(), playerCreature, (decimal)DrownHeirless, Creature, null);

            }
        //}
    }

    private CardModel makeDrowning(Player player)
    {
        if (player.Creature.CombatState != null)
            throw new NullReferenceException("Can't create a card without a CombatState!");
        if (Creature.Side == CombatSide.Player)
        {
            return player.Creature.CombatState!.CreateCard<Shiv>(player);
        }
        return player.Creature.CombatState!.CreateCard<Drowning>(player);
    }

    private async Task Defend(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.GainBlock(Creature, DefendBlock, ValueProp.Move, null);
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