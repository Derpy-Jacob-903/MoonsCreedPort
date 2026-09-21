using BaseLib.Cards;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Leo : McpZodiacCollectiblesCard
{
    public Leo() : base(0, CardType.Curse, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(BaseLibKeywords.Purge);
    }
    
    public override Material CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(15/360f, 73/100f, 84/100f );
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
    }

    protected override void OnUpgrade()
    {

    }
}