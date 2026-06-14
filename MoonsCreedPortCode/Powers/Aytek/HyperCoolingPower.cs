using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class HyperCoolingPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
 
   public override PowerStackType StackType => PowerStackType.Counter;
 
   public override Task AfterPowerAmountChanged(
     PlayerChoiceContext choiceContext,
     PowerModel power,
     Decimal amount,
     Creature? applier,
     CardModel? cardSource)
   {
     if (power is not HyperCoolingPower || power.Owner != this.Owner)
       return Task.CompletedTask;
     var player = this.Owner.Player;
     object obj;
     if (player == null)
     {
       obj = (object) null;
     }
     else
     {
       var playerCombatState = player.PlayerCombatState;
       obj = playerCombatState?.AllCards;
     }
     if (obj == null)
       obj = Array.Empty<CardModel>();
     foreach (CardModel card in (IEnumerable<CardModel>) obj)
       TryAddReplays(card, (int) amount);
     return Task.CompletedTask;
   }

   public override Task AfterCardEnteredCombat(CardModel card)
   {
     if (card.IsClone)
       return Task.CompletedTask;
     TryAddReplays(card, this.Amount);
     return Task.CompletedTask;
   }

   public override Task AfterRemoved(Creature oldOwner)
   {
     var player = oldOwner.Player;
     object obj;
     if (player == null)
     {
       obj = null;
     }
     else
     {
       PlayerCombatState playerCombatState = player.PlayerCombatState;
       obj = playerCombatState?.AllCards;
     }
     if (obj == null)
       obj = Array.Empty<CardModel>();
     foreach (CardModel cardModel in (IEnumerable<CardModel>) obj)
     {
       if (!cardModel.Keywords.Contains(AytekStuff.GunKeyword))
         cardModel.BaseReplayCount -= this.Amount;
     }
     return Task.CompletedTask;
   }

   private bool TryAddReplays(CardModel card, int amount)
   {
     if (card.Owner != this.Owner.Player || !card.Keywords.Contains(AytekStuff.GunKeyword))
       return false;
     card.BaseReplayCount += amount;
     return true;
   }
}