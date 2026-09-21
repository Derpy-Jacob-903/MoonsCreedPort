using BaseLib.Cards;
using BaseLib.Utils;
using Downfall.DownfallCode.CustomEnums;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Libra : McpZodiacCollectiblesCard
{
    public Libra() : base(1, CardType.Curse, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(CardKeyword.Retain);
    }
    
    public override Material CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(52/360f, 70/100f, 80/100f );
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
    }

    protected override void OnUpgrade()
    {

    }
}