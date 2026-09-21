using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class FriendlyReprisalPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.IsUpgraded)
        {
            if (Owner.Side == CombatSide.Enemy)
            {
                var c = cardPlay.Card;
                CreatureCmd.Damage(choiceContext, c.Owner.Creature, Amount, ValueProp.Unpowered, null, null);
            }
            else
            {
                if (Owner.Player is null) throw new NullReferenceException("FriendlyReprisalPower expects a player!");
                var hittableEnemies = CombatState.HittableEnemies;
                if (hittableEnemies.Count == 0)
                    return base.AfterCardPlayed(choiceContext, cardPlay);
                var c = cardPlay.Card;
                CreatureCmd.Damage(choiceContext, Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies), Amount, ValueProp.Unpowered, null, null);
            }
        }
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }
}