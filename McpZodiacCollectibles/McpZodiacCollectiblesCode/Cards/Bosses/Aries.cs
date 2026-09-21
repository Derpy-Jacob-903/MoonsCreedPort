using BaseLib.Cards;
using BaseLib.Utils;
using Collector.CollectorCode.Cards.Token;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Aries : Collectible<AriesOneEncounter>
{
    public Aries() : base(0, CardType.Curse, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
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