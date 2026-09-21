using BaseLib.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class SentinelGuardian : McpZodiacCollectiblesCard
{
    public SentinelGuardian() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(14);
        WithCalculatedDamage(14, (card, _) => card.Owner.Creature.Block);
    } 
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(ctx);
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}