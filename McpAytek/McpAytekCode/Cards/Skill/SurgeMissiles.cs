using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class SurgeMissiles() : AytekCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "beta_art_12.png".CardImagePath();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        for (var i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            var card = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault(WasLastCardPlayedSkill);
            if (card == null)
                return;
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }
    private bool WasLastCardPlayedSkill(CardModel c) => c.Tags.Contains(AytekStuff.Missile);
    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
}