using System;
using System.Drawing;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.SDK.MenuUI;

namespace EnsoulAbarca
{
    public static class MainCheat
    {
        #region Properties

        public static string Id { get; private set; } = "EnsoulAbarca";
        public static string SkinsId { get; private set; } = $@"{Id}Skins";
        public static string SkinChangerId { get; private set; } = $@"{Id}SkinChanger";

        public static Menu MainMenu
        {
            get
            {
                _mainMenu = new Menu(Id, Id, true);
                _mainMenu.Attach();
                _mainMenu.Add(new MenuBool(SkinChangerId, "Use Skin Changer?", false));
                _mainMenu.Add(SkinMenuList);

                return _mainMenu;
            }
        }
        private static Menu _mainMenu;
        
        public static MenuList SkinMenuList
        {
            get
            {
                _skinMenuList = new MenuList(SkinsId, "Skins", ChampionSkinData.Skins[ObjectManager.Player.CharacterName].Keys.ToArray());
                _skinMenuList.ValueChanged += SkinListOnValueChanged;
                
                return _skinMenuList;
            }
        }
        private static MenuList _skinMenuList;


        #endregion
        #region Methods

        public static void OnGameLoad()
        {
            Game.OnNotify += OnNotify;
            Game.Print($@"{Id} loaded", Color.Coral);

            var index = MainMenu[SkinsId].GetValue<MenuList>().Index;

            if (index >= SkinMenuList.Items.Length)
            {
                index = 0;
            }

            ObjectManager.Player.SetSkin(ChampionSkinData.Skins[ObjectManager.Player.CharacterName][SkinMenuList.SelectedValue]);
        }

        private static void SetSkinId()
        {
            if (!_mainMenu[SkinChangerId].GetValue<MenuBool>().Enabled)
            {
                return;
            }

            ObjectManager.Player.SetSkin(ChampionSkinData.Skins[ObjectManager.Player.CharacterName][_mainMenu[SkinsId].GetValue<MenuList>().SelectedValue]);
        }

        private static void OnNotify(GameNotifyEventArgs args)
        {
            switch (args.EventId)
            {
                case GameEventId.OnReincarnate:
                case GameEventId.OnResetChampion:
                    SetSkinId();
                    break;
            }
        }

        private static void SkinListOnValueChanged(object sender, EventArgs e) => SetSkinId();

        #endregion
    }
}
