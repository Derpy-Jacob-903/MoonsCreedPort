using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers;

public class autoslaystuff
{
    [HarmonyPatch(typeof(NGame), nameof(NGame.IsReleaseGame))]
    public static class IsReleaseGamePatch
    {
        static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }
    
    [HarmonyPatch(typeof(AutoSlayer), "PlayMainMenuAsync")]
    public class EnableAutoSlay_Patch2
    {
        [HarmonyPrefix]
        private static bool Prefix(CancellationToken ct, ref Task __result)
        {
            __result = PatchedOnPlay(ct);
            return false;
        }
        
    }
    
    private static async Task PatchedOnPlay(CancellationToken ct)
    {
        AutoSlayLog.Action("Playing main menu");
    Control mainMenu = await WaitHelper.ForNode<Control>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/MainMenu", ct, new TimeSpan?(TimeSpan.FromSeconds(30L)));
    NButton node1 = mainMenu.GetNode<NButton>((NodePath) "MainMenuTextButtons/AbandonRunButton");
    if (node1.Visible)
    {
      AutoSlayLog.Action("Abandoning existing run");
      await UiHelper.Click((NClickableControl) node1);
      await WaitHelper.Until((Func<bool>) (() => NModalContainer.Instance?.OpenModal != null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not appear");
      NButton node2 = ((Node) NModalContainer.Instance.OpenModal).GetNode<NButton>((NodePath) "VerticalPopup/YesButton");
      AutoSlayLog.Action("Confirming abandon");
      await UiHelper.Click((NClickableControl) node2);
      await WaitHelper.Until((Func<bool>) (() => NModalContainer.Instance.OpenModal == null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not close");
    }
    NButton node3 = mainMenu.GetNode<NButton>((NodePath) "MainMenuTextButtons/SingleplayerButton");
    AutoSlayLog.Action("Clicking singleplayer");
    await UiHelper.Click((NClickableControl) node3);
    Control charSelectScreen = mainMenu.GetNodeOrNull<Control>((NodePath) "Submenus/CharacterSelectScreen");
    NButton standardButton = mainMenu.GetNodeOrNull<NButton>((NodePath) "Submenus/SingleplayerSubmenu/StandardButton");
    await WaitHelper.Until((Func<bool>) (() =>
    {
      charSelectScreen = mainMenu.GetNodeOrNull<Control>((NodePath) "Submenus/CharacterSelectScreen");
      standardButton = mainMenu.GetNodeOrNull<NButton>((NodePath) "Submenus/SingleplayerSubmenu/StandardButton");
      Control control = charSelectScreen;
      bool flag1 = control != null && control.Visible;
      NButton nbutton = standardButton;
      bool flag2 = nbutton != null && nbutton.Visible;
      return flag1 | flag2;
    }), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Neither CharacterSelectScreen nor SingleplayerSubmenu became visible");
    NButton nbutton1 = standardButton;
    if ((nbutton1 != null ? (nbutton1.Visible ? 1 : 0) : 0) != 0)
    {
      Control control = charSelectScreen;
      if ((control != null ? (!control.Visible ? 1 : 0) : 1) != 0)
      {
        AutoSlayLog.Action("Clicking standard run");
        await UiHelper.Click((NClickableControl) standardButton);
        await WaitHelper.Until((Func<bool>) (() =>
        {
          Control nodeOrNull = mainMenu.GetNodeOrNull<Control>((NodePath) "Submenus/CharacterSelectScreen");
          return nodeOrNull != null && nodeOrNull.Visible;
        }), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "CharacterSelectScreen did not become visible");
        charSelectScreen = mainMenu.GetNode<Control>((NodePath) "Submenus/CharacterSelectScreen");
        goto label_15;
      }
    }
    AutoSlayLog.Action("Skipping submenu (first run)");
label_15:
    List<NCharacterSelectButton> all = UiHelper.FindAll<NCharacterSelectButton>(charSelectScreen.GetNode((NodePath) "CharSelectButtons/ButtonContainer"));
    //foreach (NCharacterSelectButton ncharacterSelectButton in all)
      //ncharacterSelectButton.UnlockIfPossible();
    NCharacterSelectButton ncharacterSelectButton1 = all.FirstOrDefault((Func<NCharacterSelectButton, bool>) (b => b.Character is Aytek));
    AutoSlayLog.Action($"Selecting character: {ncharacterSelectButton1.Character.Id}");
    ncharacterSelectButton1.Select();
    await Task.Delay(100, ct);
    NButton button = await WaitHelper.ForNode<NButton>((Node) mainMenu, "Submenus/CharacterSelectScreen/ConfirmButton", ct);
    AutoSlayLog.Action("Confirming character");
    await UiHelper.Click((NClickableControl) button);
    }
}