using System;
using System.Drawing;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulAbarca.Champions;

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
                if (_mainMenu == null)
                {
                    _mainMenu = new Menu(Id, Id, true)
                    {
                        SkinChangerMenu,
                        SkinMenu
                    };
                }

                return _mainMenu;
            }
        }
        private static Menu _mainMenu;
        
        public static MenuSlider SkinMenu
        {
            get
            {
                if (_skinMenu == null)
                {
                    _skinMenu = new MenuSlider(SkinsId, "Skins", 0, 0, 100);
                    _skinMenu.ValueChanged += SkinListOnValueChanged;
                }
                
                return _skinMenu;
            }
        }
        private static MenuSlider _skinMenu;

        public static MenuBool SkinChangerMenu { get; private set; } = new MenuBool(SkinChangerId, "Use Skin Changer?", false);

        #endregion
        #region Methods

        public static void OnGameLoad()
        {
            Game.OnNotify += OnNotify;
            Game.OnUpdate += OnUpdate;
            Game.Print($@"{Id} loaded", Color.Coral);
            MainMenu.Attach();
            ObjectManager.Player.SetSkin(SkinMenu.Value);

        }

        private static void SetSkinId()
        {
            if (!SkinChangerMenu.Enabled)
            {
                return;
            }

            ObjectManager.Player.SetSkin(SkinMenu.Value);
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

        private static void OnUpdate(EventArgs args)
        {
            switch (ObjectManager.Player.CharacterName)
            {
                case "MasterYi":
                    MasterYi.AutoAttack();
                    break;
            }            
        }

        private static void SkinListOnValueChanged(object sender, EventArgs e) => SetSkinId();

        #endregion
    }
}
