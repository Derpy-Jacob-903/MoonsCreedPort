using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class KingsArtilleryPower : MoonsCreedPortPower
{
  protected override object InitInternalData() => new Data();
  public override PowerType Type => PowerType.Buff;
  public override PowerStackType StackType => PowerStackType.Counter;
  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return Task.CompletedTask;
    this.GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    KingsArtilleryPower serpentFormPower = this;
    if (!AytekStuff.IsGun(cardPlay.Card)) return;
    int damage;
    if (cardPlay.Card.Owner != serpentFormPower.Owner.Player || !serpentFormPower.GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out damage) || damage <= 0)
      return;
    await Cmd.CustomScaledWait(0.1f, 0.2f);
    ICombatState combatState = serpentFormPower.Owner.CombatState;
    if (combatState != null)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies,
        (Decimal)damage, ValueProp.Unpowered, serpentFormPower.Owner);
    }
  }

  private class Data
  {
    /// <summary>
    /// Keep track of the cards we've seen played and the power amount at the time they were played.
    /// This lets Serpent Form avoid triggering on cards that started play before it was applied, and avoid
    /// dealing extra damage on multiple plays of Serpent Form.
    /// </summary>
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}