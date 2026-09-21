using BaseLib.Cards;
using BaseLib.Utils;
using Collector.CollectorCode.Cards.Token;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MoonsCreedBosses.MoonsCreedBossesCode.Bosses;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Aquarius : Collectible<AquariusOneEncounter>
{
    public Aquarius() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1);
    }
    
    public override Material CreateCustomFrameMaterial  => ShaderUtils.GenerateHsv(52/360f, 70/100f, 80/100f );
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
    }

    protected override void OnUpgrade()
    {

    }
}