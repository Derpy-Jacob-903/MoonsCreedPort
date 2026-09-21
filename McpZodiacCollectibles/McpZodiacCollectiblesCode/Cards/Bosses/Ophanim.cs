using BaseLib.Cards;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public class Ophanim : McpZodiacCollectiblesCard
{
    public Ophanim() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }
    
    public override Material CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(310/360f, 73/100f, 64/100f );
    
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var allCards = new List<CardModel>();

        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(CardFactory.GetDistinctForCombat(this.Owner, CardFactory
            .FilterForCombat(ModelDb.CardPool<Collector.CollectorCode.Core.CollectibleCardPool>().AllCards).Where((Func<CardModel, bool>) (c => !c.Keywords.Contains(CardKeyword.Unplayable))), 1, this.Owner.RunState.Rng.CombatCardGeneration), PileType.Hand, Owner);
        foreach (var card in combat)
        {
            if (!(card.cardAdded.Type != CardType.Power ||
                card.cardAdded.Keywords.Contains(BaseLibKeywords.Purge)))
            {
                CardCmd.ApplyKeyword(card.cardAdded, [CardKeyword.Exhaust]);
            }

            if (this.IsUpgraded)
            {
                CardCmd.Upgrade(card.cardAdded);
            }
            await CardCmd.AutoPlay(ctx, card.cardAdded, cardPlay.Target);
            /*if (card.cardAdded.Type != CardType.Power || card.cardAdded.Keywords.Contains(Collector.CollectorCode.CustomEnums.CollectorKeyword.Flicker) || card.cardAdded.Keywords.Contains(BaseLibKeywords.Purge))
            {
                await CardCmd.Exhaust(ctx, card.cardAdded);
            }*/
        }
    }

    protected override void OnUpgrade()
    {

    }
}