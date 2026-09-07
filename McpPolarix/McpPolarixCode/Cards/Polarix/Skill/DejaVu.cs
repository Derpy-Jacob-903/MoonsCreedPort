using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace McpPolarix.McpPolarixCode.Cards;

public class DejaVu() : PolarixCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (WasLastCardPlayedAttack != null)
            await CardPileCmd.AddGeneratedCardToCombat(WasLastCardPlayedAttack.CreateClone(), PileType.Hand, Owner);
    }
    
    private CardModel WasLastCardPlayedAttack
    {
        get
        {
            var lastCardEntry = CombatManager.Instance.History.CardPlaysStarted
                .LastOrDefault(e =>
                    e.CardPlay.Card.Owner == Owner &&
                    e.CardPlay.Card != this);
            return lastCardEntry?.CardPlay.Card;
        }
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}