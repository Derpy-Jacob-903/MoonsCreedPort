using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class LostTechnology : CustomEventModel
{
    public override ActModel[] Acts => []; //ModelDb.Acts.Except(ModelDb.Acts.Where(act => act.ActNumber() == 1)).ToArray();
    public override string CustomInitialPortraitPath => "events/lost_technology.png".ImagePath();
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.All(p => p.Character is Aytek); //runState.Players.All(p => p.Character is Aytek);
    }
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        var options = new List<EventOption>();
        options.Add(Option(PrecisionSight, "INITIAL",
            HoverTipFactory.FromEnchantment<PrecisionSightEnchant>()
                .Concat(HoverTipFactory.FromRelic<PrecisionSightRelic>())
                .ToArray()));
        options.Add(Option(HeavyLoad, "INITIAL",
            HoverTipFactory.FromEnchantment<PrecisionSightEnchant>()
                .Concat(HoverTipFactory.FromRelic<PrecisionSightRelic>())
                .ToArray()));
        options.Add(Option(Leave, "INITIAL"));
        return options;
    }
    
    private async Task PrecisionSight()
    {
        await RelicCmd.Obtain(ModelDb.Relic<PrecisionSightRelic>().ToMutable(), Owner);
        SetEventFinished(PageDescription("GUN"));
    }

    private async Task HeavyLoad()
    {
        await RelicCmd.Obtain(ModelDb.Relic<HeavyLoadRelic>().ToMutable(), Owner);
        SetEventFinished(PageDescription("MISSILE"));
    }

    private async Task Leave()
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard(Rng.NextItem(ModelDb.CardPool<RegentCardPool>().AllCards), Owner), PileType.Deck), 2f);
        SetEventFinished(PageDescription("LEAVE"));
    }
}

public class PrecisionSightEnchant : CustomEnchantmentModel
{
    public override bool CanEnchant(CardModel card) =>
        base.CanEnchant(card) && card.Keywords.Contains(AytekStuff.GunKeyword) && card.Type == CardType.Attack;
    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() ? 1M : 0.8M;
    }
}
[Pool(typeof(EventRelicPool))]
public class PrecisionSightRelic : MoonsCreedSharedRelic
{
    //Upon pickup, Enchant all Gun cards in your Deck with Precision Sight. Whenever you add a card that has Gun to your Deck, Enchant it with Precision Sight.
    public override RelicRarity Rarity => RelicRarity.Event;
    public override Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic && card.Tags.Contains<CardTag>(CardTag.Strike) && ModelDb.Enchantment<PrecisionSightEnchant>().CanEnchant(card))
            {
                EnchantCard(card);
                NCardEnchantVfx child = NCardEnchantVfx.Create(card);
                if (child != null)
                {
                    NRun instance = NRun.Instance;
                    if (instance != null)
                        instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) child);
                }
            }
        }
        return Task.CompletedTask;
    }

    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != this.Owner)
            return false;
        this.EnchantValidCards(cardRewards);
        return true;
    }

    public override void ModifyMerchantCardCreationResults(
        Player player,
        List<CardCreationResult> cards)
    {
        if (player != this.Owner)
            return;
        this.EnchantValidCards(cards);
    }

    public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
    {
        newCard = (CardModel) null;
        if (card.Owner != this.Owner || !ModelDb.Enchantment<PrecisionSightEnchant>().CanEnchant(card))
            return false;
        newCard = this.EnchantCard(card);
        return true;
    }

    private void EnchantValidCards(List<CardCreationResult> options)
    {
        PrecisionSightEnchant nimble = ModelDb.Enchantment<PrecisionSightEnchant>();
        foreach (CardCreationResult option in options)
        {
            CardModel card = option.Card;
            if (nimble.CanEnchant(card))
                option.ModifyCard(this.EnchantCard(card), (RelicModel) this);
        }
    }

    private CardModel EnchantCard(CardModel card)
    {
        CardModel card1 = this.Owner.RunState.CloneCard(card);
        CardCmd.Enchant<PrecisionSightEnchant>(card1, 1);
        return card1;
    }
}
public class HeavyLoadEnchant : CustomEnchantmentModel
{
    //Upon pickup, Enchant all Gun cards in your Deck with Precision Sight. Whenever you add a card that has Gun to your Deck, Enchant it with Precision Sight.
    
    public override bool CanEnchant(CardModel card) =>
        base.CanEnchant(card) && card.Tags.Contains(AytekStuff.Missile) && card.Type == CardType.Attack;
    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() ? 1M : 1.2M;
    }
}
[Pool(typeof(EventRelicPool))]
public class HeavyLoadRelic : MoonsCreedSharedRelic
{
    //Upon pickup, Enchant all cards containing “Missile” in your Deck with Heavy Load. Whenever you add a card containing “Missile” to your Deck, Enchant it with Heavy Load.
    public override RelicRarity Rarity => RelicRarity.Event;
    public override Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic && card.Tags.Contains<CardTag>(CardTag.Strike) && ModelDb.Enchantment<HeavyLoadEnchant>().CanEnchant(card))
            {
                CardCmd.Enchant<HeavyLoadEnchant>(card, 1M);
                NCardEnchantVfx child = NCardEnchantVfx.Create(card);
                if (child != null)
                {
                    NRun instance = NRun.Instance;
                    if (instance != null)
                        instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) child);
                }
            }
        }
        return Task.CompletedTask;
    }
}

[Pool(typeof(EventRelicPool))]
public class LightReapRelic : MoonsCreedSharedRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(3)
    ];
}

[Pool(typeof(EventRelicPool))]
public class DarkReapRelic : MoonsCreedSharedRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(3)
    ];
}

[Pool(typeof(EventRelicPool))]
public class TheLastKnightsWillRelic : MoonsCreedSharedRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(40),
        new PowerVar<DoomPower>(60),
        new HealVar(5)
    ];
}