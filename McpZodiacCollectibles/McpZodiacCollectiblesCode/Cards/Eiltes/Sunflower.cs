using BaseLib.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Sunflower : McpZodiacCollectiblesCard
{
    public Sunflower() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(1);
    }
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
    }

    protected override void OnUpgrade()
    {

    }
}