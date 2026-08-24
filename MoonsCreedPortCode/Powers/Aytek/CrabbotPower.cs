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
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class CrabbotPower : MoonsCreedPortPower
{
    private const int _baseCardsLeft = 4;
  private const string _cardsLeftKey = "CardsLeft";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.DynamicVars["CardsLeft"].IntValue;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CardsLeft", _baseCardsLeft), new PowerVar<PlatingPower>(4) ];

  protected override object InitInternalData() => (object) new Data();

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CrabbotPower panachePower = this;
    if (!cardPlay.Card.IsUpgraded) return;
    Data data;
    if (cardPlay.Card.Owner == panachePower.Owner.Player)
    {
      data = panachePower.GetInternalData<Data>();
      if (data.alreadyApplied)
      {
        --panachePower.DynamicVars["CardsLeft"].BaseValue;
        panachePower.InvokeDisplayAmountChanged();
        if (panachePower.DynamicVars["CardsLeft"].IntValue <= 0)
        {
          await Cmd.Wait(0.5f);
          await PowerCmd.Apply<PlatingPower>(choiceContext, Owner, DynamicVars["PlatingPower"].BaseValue, null, null);
          /*IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext,
            (IEnumerable<Creature>)panachePower.CombatState.HittableEnemies, (Decimal)panachePower.Amount,
            ValueProp.Unpowered, panachePower.Owner);*/
          panachePower.DynamicVars["CardsLeft"].BaseValue = _baseCardsLeft;
          panachePower.InvokeDisplayAmountChanged();
        }
      }

      data.alreadyApplied = true;
    }
    else
    {
    }

    data = (Data) null;
  }

  private class Data
  {
    /// <summary>
    /// We track this so we don't count the Panache card towards itself.
    /// </summary>
    public bool alreadyApplied;
  }
}