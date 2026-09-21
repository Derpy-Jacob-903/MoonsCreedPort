using BaseLib.Cards;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Scorpio : McpZodiacCollectiblesCard
{
    public Scorpio() : base(0, CardType.Curse, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(BaseLibKeywords.Purge);
    }
    
    public override Material CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(219/360f, 93/100f, 1f);
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
    }

    protected override void OnUpgrade()
    {

    }
}