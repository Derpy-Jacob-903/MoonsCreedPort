using BaseLib.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Beholder : McpZodiacCollectiblesCard
{
    public Beholder() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithDamage(5, 2);
        WithPower<DebilitatePower>(1, 1);
        WithTip<VulnerablePower>();
        WithTip<WeakPower>();
    }
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await CommonActions.Apply<DebilitatePower>(ctx, this, cardPlay);
    }
}