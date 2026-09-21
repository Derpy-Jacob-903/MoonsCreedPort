using BaseLib.Cards;
using BaseLib.Utils;
using Collector.CollectorCode.Cards.Token;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Cancer : Collectible<CancerOne.CancerOneEncounter>
{
    public Cancer() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(CardKeyword.Exhaust);
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